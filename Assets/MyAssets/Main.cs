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
    public PlayerNum playerNum { get; set; }



    public int timeLimit { get; set; }




    public GameObject HomeScreen;
    public GameObject ConnectionScreen;
    public GameObject BattleScreen;
    public GameObject ResultScreen;

    public List<GameObject> P1Objects = new List<GameObject>();
    public List<GameObject> P2Objects = new List<GameObject>();


    //�X�^�[�g�n�_
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
        //�܂��Z�[�u�t�@�C�����쐬����ĂȂ��Ƃ�������
        if (SaveSystem.Load() == null)
        {
            Debug.Log("first");
            InitializeClass();
        }
        else //�t�@�C��������Ă���Ȃ�Load
        {
            MainLoad();
        }
    }
    void Start()
    {

    }


    void Update()
    {
        // Debug.Log("turnplayer  " + TurnPlayer);

        if (IsGameStart)
        {
            if (IsGameEnd)
            {
                if (CheckScore())
                {
                    GameSet();
                }
                else
                {
                    StartSuddenDeath();
                }
                Debug.Log(CheckScore());

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
    /// �X�^�[�g�{�^������������
    /// </summary>
    public void Play()
    {
        MaxPlayer = 2;
        IsPressPlay = true;
        MatchMaking.matchMake.StartMatchMaking("Game");
        ChangeActive(HomeScreen, ConnectionScreen);
        InitGame();
    }

    public void ChangeActive(GameObject falseObj, GameObject trueObj)
    {
        if (falseObj.activeSelf)
            falseObj.SetActive(false);
        if (!trueObj.activeSelf)
            trueObj.SetActive(true);
    }




    [PunRPC]
    public void GameStart()
    {
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

    }

    //�������Ԃ̃J�E���g�_�E��
    IEnumerator TimeLimitCount()
    {
        while (true)
        {
            timeLimit--;
            if (timeLimit <= 0)
            {
                IsGameEnd = true;
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    bool CheckScore()
    {
        int highestScore = 0;
        //�N����ԓ��_�����������ׂ� �����ɂ���l�����܂�
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            //�n�C�X�R�A�ȏ�Ȃ�m�F
            if (highestScore <= PlayerScore[i])
            {
                //�Ƃ肠�����n�C�X�R�A�ۑ�
                highestScore = PlayerScore[i];

                //�����Ă���΁A���҂͎��������ɂ���
                if (highestScore < PlayerScore[i])
                {
                    WinnerList.Clear();
                }
                //������ʂȂ牽�����Ȃ�
                else if (highestScore == PlayerScore[i])
                {

                }
                WinnerList.Add((PlayerNum)Enum.ToObject(typeof(PlayerNum), i));
                if (highestScore == 0)
                {
                    WinnerList.Clear();
                }
            }
        }

        Debug.Log("winner count" + WinnerList.Count);
        if (WinnerList.Count == 1)
        {
            Winner = WinnerList[0];
            return true;
        }
        else
        {
            return false;
        }

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




    //player�𐶐�
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




    public void GameSet()
    {
        IsGameStart = false;
        IsPressPlay = false;
        IsGameEnd = false;
        ChangeActive(BattleScreen, ResultScreen);



    }
    public void Disconnect()
    {
        //PhotonNetwork.Destroy(photonView);
        PhotonNetwork.Disconnect();
    }
    public void DisconnectInGame()
    {
        Disconnect();
        IsGameEnd = true;
        Winner = playerNum;
    }
    public void OpponentLeft()
    {
        IsGameEnd = true;
        Winner = playerNum;
    }
    public void ReturnHome()
    {
        PhotonNetwork.LeaveRoom();
        Disconnect();
        if (BattleScreen.activeSelf)
            ChangeActive(BattleScreen, HomeScreen);
        else if (ResultScreen.activeSelf)
            ChangeActive(ResultScreen, HomeScreen);

        InitGame();
    }
    public void Retry()
    {
        PhotonNetwork.LeaveRoom();
        ChangeActive(ResultScreen, ConnectionScreen);
        Play();
    }

    void InitGame()
    {
        WinnerList.Clear();
        Winner = PlayerNum.None;
        PlayerScore = new int[4];
        timeLimit = 3;
    }



    public GameObject SweetsPrefab;
    public GameObject BombPrefab;
    //x:-1.9~1.9 y:5.5




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








    //�������牺�e�X�g�p

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
