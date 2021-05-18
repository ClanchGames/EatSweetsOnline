using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static AdMob;
using System.Linq;
using System;

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

    public List<GameObject> P1Objects = new List<GameObject>();
    public List<GameObject> P2Objects = new List<GameObject>();
    public bool IsP1Stop { get; set; } = false;
    public bool IsP2Stop { get; set; } = false;


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
        ChangeTurn();
    }

    private void ChangeTurn()
    {
        //ターンプレイヤーのみが通れる。
        if (TurnPlayer != PlayerNum.None && TurnPlayer != playerNum)
        {
            return;
        }

        //切り替え
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

        photonView.RPC(nameof(SetTurnPlayer), RpcTarget.AllBuffered, (int)TurnPlayer);
    }
    [PunRPC]
    void SetTurnPlayer(int num)
    {
        TurnPlayer = (PlayerNum)Enum.ToObject(typeof(PlayerNum), num);

        //ターンプレイヤーになった人はプレイヤー生成
        if (TurnPlayer == playerNum)
        {
            GeneratePlayer();
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
            //  Debug.Log("limit" + timeLimit);
        }
        ChangeTurn();
    }


    public void AddPlayerToList(GameObject player, PlayerNum num)
    {

        if (playerNum == PlayerNum.Player1)
        {
            P1Objects.Add(player);
            Debug.Log("p1objadd");
        }


        if (playerNum == PlayerNum.Player2)
        {
            P2Objects.Add(player);
            Debug.Log("p2objadd");
        }

    }
    public void AfterShot()
    {
        StopCoroutine(countDown);
        photonView.RPC(nameof(StartCheckPlayerMotion), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void StartCheckPlayerMotion()
    {
        StartCoroutine(CheckPlayerMotion());
    }
    IEnumerator CheckPlayerMotion()
    {
        int a = 0;
        while (a < 1000000)
        {
            yield return new WaitForSeconds(0.3f);
            if (!IsGameStart) yield break;
            a++;
            if (playerNum == PlayerNum.Player1)
            {
                IsP1Stop = true;
                if (P1Objects.Count > 0)
                {
                    foreach (var p1 in P1Objects)
                    {
                        if (p1 == null)
                        {
                            continue;
                        }
                        Debug.Log(p1.name);
                        CharacterController controller = p1.GetComponent<CharacterController>();
                        if (!controller.IsStop)
                        {
                            IsP1Stop = false;
                        }
                    }
                }
                if (IsP1Stop)
                {
                    photonView.RPC(nameof(ConfirmStop), RpcTarget.AllBuffered, (int)PlayerNum.Player1);
                }
            }

            if (playerNum == PlayerNum.Player2)
            {
                IsP2Stop = true;
                if (P2Objects.Count > 0)
                {
                    Debug.Log("in1");
                    foreach (var p2 in P2Objects)
                    {
                        if (p2 == null)
                        {
                            continue;
                        }

                        Debug.Log(p2.name);
                        CharacterController controller = p2.GetComponent<CharacterController>();
                        if (!controller.IsStop)
                        {
                            IsP2Stop = false;
                        }
                    }
                }
                if (IsP2Stop)
                {
                    photonView.RPC(nameof(ConfirmStop), RpcTarget.AllBuffered, (int)PlayerNum.Player2);
                    Debug.Log("in2");
                }
            }

            //全員が止まってたらOK
            if (IsP1Stop && IsP2Stop)
            {
                Debug.Log("all stop");
                if (TurnPlayer == playerNum)
                {
                    ChangeTurn();
                }
                yield break;
            }
        }
    }
    [PunRPC]
    void ConfirmStop(int num)
    {
        if (num == (int)PlayerNum.Player1)
            IsP1Stop = true;
        else if (num == (int)PlayerNum.Player2)
            IsP2Stop = true;
        Debug.Log("confirm" + " p1" + IsP1Stop + " p2" + IsP2Stop);
    }


    //playerを生成
    public void GeneratePlayer()
    {
        //スタート地点
        Vector3 position = new Vector3(0, -10, 0);
        if (TurnPlayer == PlayerNum.Player1)
        {
            GameObject player1 = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
        }
        else if (TurnPlayer == PlayerNum.Player2)
        {
            GameObject player2 = PhotonNetwork.Instantiate("Player2", position, Quaternion.identity);
        }

    }

    public void PlayerDead(PlayerNum player, GameObject playerObject)
    {
        if (player == PlayerNum.Player1)
        {
            Player1Life--;
            P1Objects.Remove(playerObject);
        }
        else if (player == PlayerNum.Player2)
        {
            Player2Life--;
            P2Objects.Remove(playerObject);
        }
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
        if (P1Objects.Count > 0)
        {
            foreach (var player in P1Objects)
            {
                Destroy(player);
            }
        }
        if (P2Objects.Count > 0)
        {
            foreach (var player in P2Objects)
            {
                Destroy(player);
            }
        }
        P1Objects = new List<GameObject>();
        P2Objects = new List<GameObject>();
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
