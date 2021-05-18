using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static AdMob;
using System.Linq;

public class Main : MonoBehaviourPunCallbacks
{
    public static Main main;
    DebugSystem debug;
    public SaveData saveData;

    public bool right;
    public bool left;

    public bool IsGameStart { get; set; }

    public bool isMaster { get; set; }
    public int Player1Life { get; set; }
    public int Player2Life { get; set; }
    int playerStartLife = 2;

    public string PlayerName { get; set; }

    public enum PlayerNum
    {
        None = 0,
        Player1 = 1,
        Player2 = 2,
    }
    public PlayerNum TurnPlayer { get; set; }
    public PlayerNum playerNum { get; set; }

    public bool IsYourTurn
    {
        get
        {
            return isMaster && TurnPlayer == PlayerNum.Player1 || !isMaster && TurnPlayer == PlayerNum.Player2;
        }
    }

    public int timeLimit { get; set; }
    private IEnumerator countDown;



    public GameObject HomeScreen;
    public GameObject ConnectionScreen;
    public GameObject BattleScreen;
    public GameObject ResultScreen;

    public List<GameObject> AllPlayers = new List<GameObject>();
    public void Right()
    {
        right = true;
        left = false;
    }
    public void Left()
    {
        left = true;
        right = false;
    }
    public void Stop()
    {
        right = false;
        left = false;
    }
    private void Awake()
    {

        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        debug = GetComponent<DebugSystem>();
        //まだセーブファイルが作成されてないとき初期化
        if (SaveSystem.Load() == null)
        {
            Debug.Log("first");
            InitializeClass();
        }
        else //ファイルが作られているならLoad
        {
            MainLoad();
        }
    }
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MatchMaking.matchMake.StartMatchMaking(PlayerName);
        }

        if (IsGameStart)
        {
            if (Player1Life <= 0 || Player2Life <= 0)
            {
                GameSet();
            }
        }

    }
    IEnumerator MainSave()
    {
        int x = 0;
        while (x < 10000)
        {
            x++;
            SaveSystem.Save(saveData);
            yield return new WaitForSeconds(1f);
        }
    }
    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void Play()
    {
        IsGameStart = true;
        MatchMaking.matchMake.StartMatchMaking("aaa");
        ChangeActive(HomeScreen, ConnectionScreen);
        ResetGame();
    }

    public void ChangeActive(GameObject falseObj, GameObject trueObj)
    {
        if (falseObj.activeSelf)
            falseObj.SetActive(false);
        if (!trueObj.activeSelf)
            trueObj.SetActive(true);
    }

    public void GameStart()
    {
        photonView.RPC(nameof(ChangeTurn), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ChangeTurn()
    {
        switch (TurnPlayer)
        {
            case PlayerNum.None:
                TurnPlayer = PlayerNum.Player1;
                break;
            case PlayerNum.Player1:
                TurnPlayer = PlayerNum.Player2;
                break;
            case PlayerNum.Player2:
                TurnPlayer = PlayerNum.Player1;
                break;
        }

        if (TurnPlayer == playerNum)
        {
            countDown = null;
            countDown = CountDown();
            StartCoroutine(countDown);
        }
    }


    public IEnumerator CountDown()
    {
        timeLimit = 10;
        while (timeLimit > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLimit--;
            Debug.Log("limit" + timeLimit);
        }
        photonView.RPC(nameof(ChangeTurn), RpcTarget.AllBuffered);
    }


    public void AddPlayerToList(GameObject player)
    {
        AllPlayers.Add(player);
    }
    public void AfterShot()
    {
        Debug.Log("ismaster" + isMaster);
        StopCoroutine(countDown);
        StartCoroutine(CheckPlayerIsMoveCoroutine());
    }

    IEnumerator CheckPlayerIsMoveCoroutine()
    {
        int a = 0;
        while (a < 1000000)
        {
            yield return new WaitForSeconds(0.5f);
            a++;
            bool IsAllPlayerStop = true;
            foreach (var player in AllPlayers)
            {

                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb.velocity != Vector3.zero)
                {
                    IsAllPlayerStop = false;
                }
            }
            //全員が止まってたらOK
            if (IsAllPlayerStop)
            {
                photonView.RPC(nameof(ChangeTurn), RpcTarget.AllBuffered);

                yield break;
            }

        }
    }

    //playerを生成
    public void GeneratePlayer()
    {
        //スタート地点
        Vector3 position = new Vector3(0, -10, 0);
        if (TurnPlayer == PlayerNum.Player1)
        {
            if (isMaster)
            {
                GameObject player1 = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
            }
        }
        else if (TurnPlayer == PlayerNum.Player2)
        {
            if (!isMaster)
            {
                GameObject player2 = PhotonNetwork.Instantiate("Player2", position, Quaternion.identity);
            }
        }

    }

    public void PlayerDead(PlayerNum player, GameObject playerObject)
    {
        if (player == PlayerNum.Player1)
        {
            Player1Life--;
        }
        else if (player == PlayerNum.Player2)
        {
            Player2Life--;
        }
        AllPlayers.Remove(playerObject);
    }



    public void GameSet()
    {
        IsGameStart = false;
        Debug.Log("game set");
        Disconnect();
        ChangeActive(BattleScreen, ResultScreen);

    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public void ReturnHome()
    {
        if (BattleScreen.activeSelf)
            ChangeActive(BattleScreen, HomeScreen);
        else if (ResultScreen.activeSelf)
            ChangeActive(ResultScreen, HomeScreen);

        ResetGame();
    }
    public void Retry()
    {
        ChangeActive(ResultScreen, ConnectionScreen);
        ResetGame();
        MatchMaking.matchMake.StartMatchMaking(PlayerName);
    }

    void InitGame()
    {
        Player1Life = playerStartLife;
        Player2Life = playerStartLife;
    }
    void ResetGame()
    {
        if (AllPlayers.Count > 0)
        {
            Debug.Log("players destroy");
            foreach (var player in AllPlayers)
            {
                Destroy(player);
            }
        }
        AllPlayers = new List<GameObject>();
        InitGame();

    }



    //ここから下テスト用

    public void Save()
    {
        SaveSystem.Save(saveData);
    }
    public void MainLoad()
    {
        saveData = SaveSystem.Load();
        // Debug.Log(saveData.playerdata.level);
    }
    public void MainDelete()
    {
        InitializeClass();
        Save();
        DebugSystem.debug.Log("delete");
    }
    void InitializeClass()
    {
        saveData = new SaveData();
        SaveData.SD.playerdata = new PlayerData();
    }

    public void ShowNormalBanner()
    {
        adMob.ShowNormalBanner();
    }
    public void ShowLargeBanner()
    {
        adMob.ShowLargeBanner();
    }

    public void HideNormalBanner()
    {
        adMob.HideNormalBanner();
    }
    public void HideLargeBanner()
    {
        adMob.HideLargeBanner();
    }
    public void Banner()
    {
        adMob.RequestBanner();
    }
    public void HideBanner()
    {
        adMob.HideBanner();
    }
    public void ShowBanner()
    {
        adMob.ShowBanner();
    }
    public void Interstitial()
    {
        adMob.ShowInterstitial();
    }
    public void Reward()
    {
        adMob.ShowRewardAd();
    }
}
