using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static AdMob;
using System.Linq;
using System;
public enum PlayerNum
{
    All = 100,
    None = -1,
    Player1 = 0,
    Player2 = 1,
    Player3 = 2,
    Player4 = 3,
}
public class Main : MonoBehaviourPunCallbacks
{
    public static Main main;
    DebugSystem debug;
    public SaveData saveData;


    public byte MaxPlayer { get; set; }

    public bool IsPressPlay { get; set; }
    public bool IsGameStart { get; set; }
    public bool IsGameEnd { get; set; }
    public bool IsSuddenDeath { get; set; }


    public bool isMaster { get; set; }


    List<PlayerNum> WinnerList = new List<PlayerNum>();
    public PlayerNum Winner { get; set; } = PlayerNum.None;

    public string PlayerName { get; set; }


    public PlayerNum TurnPlayer { get; set; }

    //1P 2Pを割り振る
    public PlayerNum playerNum { get; set; }



    public int timeLimit { get; set; }




    public GameObject HomeScreen;
    public GameObject ConnectionScreen;
    public GameObject BattleScreen;
    public GameObject ResultScreen;
    public GameObject PasswordScreen;

    public GameObject ConfigScreen;

    public List<GameObject> P1Objects = new List<GameObject>();
    public List<GameObject> P2Objects = new List<GameObject>();


    //スタート地点
    Vector3 Player1StartPos = new Vector3(-0.6f, -3.5f, 0);
    Vector3 Player2StartPos = new Vector3(0.6f, -3.5f, 0);
    Vector3 Player3StartPos = new Vector3(5, -11, -4);
    Vector3 Player4StartPos = new Vector3(-5, 11, -4);




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
            InitializeClass();
        }
        else //ファイルが作られているならLoad
        {
            MainLoad();
        }
    }
    void Start()
    {
        SE.se.PlayBGM(BGM.Home, true);
        HideBanner();
        HideLargeBanner();

        StartCoroutine(MainSave());
    }

    float startTime;

    bool isOpponentLeft = false;
    void Update()
    {


        if (IsGameStart)
        {
            if (IsGameEnd)
            {
                if (!isOpponentLeft)
                    Winner = CheckWinner();
                else
                {
                    isOpponentLeft = false;
                    Winner = playerNum;
                }
                GameSet();

            }
        }

    }
    IEnumerator MainSave()
    {
        while (true)
        {
            SaveSystem.Save(saveData);
            yield return new WaitForSeconds(1f);
        }
    }
    /// <summary>
    /// スタートボタンを押す処理
    /// </summary>
    /// 
    bool isPrivate = false;
    public void Play(bool isprivate)
    {
        Disconnect();
        //広告非表示
        HideBanner();
        HideLargeBanner();

        isPrivate = isprivate;
        MatchMaking.matchMake.isPrivate = isPrivate;
        if (isPrivate)
        {
            if (MatchMaking.matchMake.password.Length != MatchMaking.matchMake.passwordLengthMax)
            {
                isPrivate = false;
                return;
            }
        }
        MaxPlayer = 2;
        IsPressPlay = true;
        InitGame();
        ChangeActive(HomeScreen, ConnectionScreen);
        ChangeActive(PasswordScreen, ConnectionScreen);
        MatchMaking.matchMake.StartMatchMaking();
    }

    public void Retry()
    {
        PhotonNetwork.LeaveRoom();
        Disconnect();
        ChangeActive(ResultScreen, ConnectionScreen);
        ChangeActive(BattleScreen, null);
        Play(isPrivate);
    }

    public void ChangeActive(GameObject falseObj, GameObject trueObj)
    {
        if (falseObj != null)
        {
            if (falseObj.activeSelf)
            {
                falseObj.SetActive(false);
            }
        }
        if (trueObj != null)
        {
            if (!trueObj.activeSelf)
            {
                trueObj.SetActive(true);
            }
        }
    }

    public void OpenPasswordScreen()
    {
        ChangeActive(HomeScreen, PasswordScreen);
    }
    public void ClosePasswordScreen()
    {
        ChangeActive(PasswordScreen, HomeScreen);
    }


    [PunRPC]
    public void GameStart()
    {
        SE.se.PlayBGM(BGM.Home, false);
        SE.se.PlayBGM(BGM.Battle, false);
        StartCoroutine(GameStartDelay());
        GeneratePlayer();
    }


    IEnumerator GameStartDelay()
    {
        SE.se.CountDown();
        yield return new WaitForSeconds(3f);
        IsGameStart = true;
        StartCoroutine(TimeLimitCount());
        if (isMaster)
        {
            StartCoroutine(GenerateSweets());
            StartCoroutine(GenerateBomb());
        }
        SE.se.PlayBGM(BGM.Battle, true);

    }

    //制限時間のカウントダウン
    IEnumerator TimeLimitCount()
    {
        while (true)
        {
            timeLimit--;
            if (timeLimit <= 0 || isOpponentLeft)
            {
                IsGameEnd = true;
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }


    PlayerNum CheckWinner()
    {
        PlayerNum winnerNum;
        if (PlayerScore[0] > PlayerScore[1])
        {
            winnerNum = PlayerNum.Player1;
        }
        else if (PlayerScore[0] < PlayerScore[1])
        {
            winnerNum = PlayerNum.Player2;
        }
        else
        {
            winnerNum = PlayerNum.All;
        }
        return winnerNum;
    }
    void StartSuddenDeath()
    {
        IsSuddenDeath = true;
        IsGameEnd = false;
    }


    public void AddPlayerToList(GameObject player, PlayerNum num)
    {

        if (playerNum == PlayerNum.Player1)
        {
            P1Objects.Add(player);
        }


        if (playerNum == PlayerNum.Player2)
        {
            P2Objects.Add(player);
        }

    }




    //playerを生成
    public void GeneratePlayer()
    {

        if (playerNum == PlayerNum.Player1)
        {
            GameObject player1 = PhotonNetwork.Instantiate("Player1", Player1StartPos, Quaternion.identity);
        }
        else if (playerNum == PlayerNum.Player2)
        {
            GameObject player2 = PhotonNetwork.Instantiate("Player2", Player2StartPos, Quaternion.identity);
        }
    }

    public Sprite SweetsSprite { get; set; }


    public void ChangeSprite()
    {
        //画像の中からランダムにスイーツを一つ選ぶ
        int sweetsNum = UnityEngine.Random.Range(0, SweetsList.Length);
        photonView.RPC(nameof(SetSprite), RpcTarget.AllBuffered, sweetsNum);
    }
    [PunRPC]
    void SetSprite(int sweetsNum)
    {
        SweetsSprite = SweetsList[sweetsNum];
    }

    public void GameSet()
    {
        IsGameStart = false;
        IsPressPlay = false;
        IsGameEnd = false;
        ChangeActive(null, ResultScreen);
        if (Winner == playerNum)
        {
            SE.se.Win();
        }
        else
        {
            SE.se.Lose();
        }

        //広告の表示
        ShowBanner();
        ShowLargeBanner();

    }
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

    }
    public void DisconnectInGame()
    {
        Disconnect();
        IsGameEnd = true;
        Winner = playerNum;
    }
    public void OpponentLeft()
    {
        isOpponentLeft = true;
        Winner = playerNum;
    }
    public void ReturnHome()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        Disconnect();
        if (BattleScreen.activeSelf)
            ChangeActive(BattleScreen, HomeScreen);
        if (ResultScreen.activeSelf)
            ChangeActive(ResultScreen, HomeScreen);
        if (ConnectionScreen.activeSelf)
            ChangeActive(ConnectionScreen, HomeScreen);

        SE.se.PlayBGM(BGM.Home, true);
        SE.se.PlayBGM(BGM.Battle, false);
        InitGame();
    }


    void InitGame()
    {
        WinnerList.Clear();
        Winner = PlayerNum.None;
        PlayerScore = new int[4];
        timeLimit = 30;
    }



    public GameObject SweetsPrefab;
    public GameObject BombPrefab;
    //x:-1.9~1.9 y:5.5


    public Sprite[] SweetsList;

    IEnumerator GenerateSweets()
    {
        while (IsGameStart)
        {
            float xpos = UnityEngine.Random.Range(-1.9f, 1.9f);
            GameObject Sweets = PhotonNetwork.Instantiate("Sweets", new Vector3(xpos, 5.5f, 0), Quaternion.identity);
            float delay = UnityEngine.Random.Range(1f, 2f);
            yield return new WaitForSeconds(delay);

        }
    }

    IEnumerator GenerateBomb()
    {
        while (IsGameStart)
        {
            float delay = UnityEngine.Random.Range(2f, 4f);
            yield return new WaitForSeconds(delay);
            float xpos = UnityEngine.Random.Range(-1.9f, 1.9f);
            GameObject Bomb = PhotonNetwork.Instantiate("Bomb", new Vector3(xpos, 5.5f, 0), Quaternion.identity);

        }
    }

    public int[] PlayerScore = new int[4];

    /// <summary>
    /// ([PlayerNum,Score])
    /// </summary>
    /// <param name="PlayerAndScore"></param>
    [PunRPC]
    public void GetScore(int[] PlayerAndScore)
    {
        PlayerScore[PlayerAndScore[0]] += PlayerAndScore[1];
        if (IsSuddenDeath)
        {
            IsSuddenDeath = false;
            IsGameEnd = true;
        }
    }

    public void OpenConfig()
    {
        ChangeActive(HomeScreen, ConfigScreen);
    }

    public void CloseConfig()
    {
        ChangeActive(ConfigScreen, HomeScreen);
    }
    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/intent/user?user_id=1298189028353650690");
    }





    //ここから共通機能



    public void Save()
    {
        SaveSystem.Save(saveData);
    }
    public void MainLoad()
    {
        saveData = SaveSystem.Load();
        AudioListener.volume = saveData.volume;
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
        //SaveData.SD.playerdata = new PlayerData();
    }

    //広告関係
    //今回はBannerとLargeBannerだけ使う

    public void ShowNormalBanner()
    {
        adMob.ShowNormalBanner();
    }
    public void ShowLargeBanner()
    {
        adMob.ShowLargeBanner();
    }
    public void ShowBanner()
    {
        adMob.ShowBanner();
    }


    public void HideNormalBanner()
    {
        adMob.HideNormalBanner();
    }
    public void HideLargeBanner()
    {
        adMob.HideLargeBanner();
    }

    public void HideBanner()
    {
        adMob.HideBanner();
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
