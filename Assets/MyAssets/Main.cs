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
    All = -1,
    None = 0,
    Player1 = 1,
    Player2 = 2,
    Player3 = 3,
    Player4 = 4,
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


    public bool isMaster { get; set; }
    public int Player1Life { get; set; }
    public int Player2Life { get; set; }
    int playerStartLife = 1;

    public PlayerNum Winner { get; set; } = PlayerNum.None;

    public string PlayerName { get; set; }


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

    //スタート地点
    Vector3 Player1StartPos = new Vector3(-0.6f, -3.5f, 0);
    Vector3 Player2StartPos = new Vector3(0.6f, -3.5f, 0);
    Vector3 Player3StartPos = new Vector3(5, -11, -4);
    Vector3 Player4StartPos = new Vector3(-5, 11, -4);

    //この座標の周りは穴をあけない　壁も作らない
    List<Vector3> SafeZone = new List<Vector3>();



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
        if (SafeZone.Count == 0)
        {
            SafeZone.Add(Player1StartPos);
            SafeZone.Add(Player2StartPos);
            SafeZone.Add(Player3StartPos);
            SafeZone.Add(Player4StartPos);
        }
    }


    void Update()
    {
        // Debug.Log("turnplayer  " + TurnPlayer);

        if (IsGameStart)
        {
            if (Player1Life <= 0 || Player2Life <= 0 || IsGameEnd)
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
    /// スタートボタンを押す処理
    /// </summary>
    public void Play()
    {
        MaxPlayer = 2;
        IsPressPlay = true;
        MatchMaking.matchMake.StartMatchMaking("Game");
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

    public void StartGenerateStage()
    {
        photonView.RPC(nameof(GenerateStage), RpcTarget.AllBuffered);
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
    }

    /* private void ChangeTurn()
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
     }*/
    /*[PunRPC]
    void SetTurnPlayer(int num)
    {
        TurnPlayer = (PlayerNum)Enum.ToObject(typeof(PlayerNum), num);
        // Debug.Log("set turn player");
        //ターンプレイヤーになった人はプレイヤー生成
        if (TurnPlayer == playerNum)
        {
            // Debug.Log("you are turnplayer");
            GeneratePlayer();
            countDown = null;
            countDown = CountDown();
            StartCoroutine(countDown);
        }
    }

    public IEnumerator CountDown()
    {
        timeLimit = 20;
        while (timeLimit > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLimit--;
            //  Debug.Log("limit" + timeLimit);
        }
        if (IsGameEnd) yield break;
        ChangeTurn();
    }
    */

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

    /*
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
                        //ここよくない
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
                    foreach (var p2 in P2Objects)
                    {
                        if (p2 == null)
                        {
                            continue;
                        }

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

    }
    */

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

    [PunRPC]
    public void PlayerDead(int playernum)
    {
        if (playernum == (int)PlayerNum.Player1)
        {
            Player1Life--;
        }
        else if (playernum == (int)PlayerNum.Player2)
        {
            Player2Life--;
        }
    }



    public void GameSet()
    {
        IsGameStart = false;
        IsPressPlay = false;
        IsGameEnd = false;
        ChangeActive(BattleScreen, ResultScreen);
        if (Player1Life <= 0)
        {
            Winner = PlayerNum.Player2;
        }
        if (Player2Life <= 0)
        {
            Winner = PlayerNum.Player1;
        }

        if (Player1Life <= 0 && Player2Life <= 0)
        {
            Winner = PlayerNum.All;
        }
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

        ResetGame();
    }
    public void Retry()
    {
        PhotonNetwork.LeaveRoom();
        ChangeActive(ResultScreen, ConnectionScreen);
        Play();
    }

    void InitGame()
    {
        TurnPlayer = PlayerNum.None;
        Winner = PlayerNum.None;
        Player1Life = playerStartLife;
        Player2Life = playerStartLife;
    }
    void ResetGame()
    {
        /*  GameObject[] floorObjects = MyMethod.FindObject("FloorArea").GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
          foreach (var floorObject in floorObjects)
          {
              if (floorObject.name != "FloorArea")
                  Destroy(floorObject);
          }*/
        InitGame();
    }
    public void DestroyAll()
    {

        /* if (P1Objects.Count > 0)
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
         P2Objects = new List<GameObject>();*/
    }

    [SerializeField]
    GameObject Floor1Prefab;
    private Vector3 StartPos = new Vector3(-7, -13, 0);
    private Vector3 EndPos = new Vector3(7, 13, 0);

    enum TileType
    {
        Empty,
        Floor,
        Wall
    }

    int width;
    int height;
    int mapSize;
    int holeNum = 10;


    //Masterがステージを生成
    [PunRPC]
    public void GenerateStage()
    {
        width = (int)(EndPos.x - StartPos.x) + 1;
        height = (int)(EndPos.y - StartPos.y) + 1;
        mapSize = width * height;
        if (!isMaster) return;

        //ここに追加された座標からランダムに穴をあける座標を選ぶ
        var FloorList = new List<(int i, int j)>();

        int[,] MapData = new int[height, width];
        int tileNum = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 position = new Vector3(StartPos.x + j, StartPos.y + i, StartPos.z);

                //とりあえず全座標をMapDataに入れる
                MapData[i, j] = (int)TileType.Floor;

                bool IsSafe = true;

                //スタート地点の周りは安全にする
                foreach (var pos in SafeZone)
                {

                    //スタート地点と離れているか確認
                    if ((pos - position).magnitude < 5)
                    {
                        //一か所でもスタート地点と近かったらfalseして抜け出す
                        IsSafe = false;
                        break;
                    }
                }

                if (!IsSafe)
                {
                    //SafeZoneじゃないなら追加する
                    FloorList.Add((i, j));
                }

                tileNum++;
            }
        }


        //ランダムに穴をあける
        for (int n = 0; n < holeNum; n++)
        {
            int random = UnityEngine.Random.Range(0, FloorList.Count);
            MapData[FloorList[random].i, FloorList[random].j] = (int)TileType.Empty;


            FloorList.RemoveAt(random);
        }


        //MapDataに入っている情報通りに生成
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 position = new Vector3(StartPos.x + j, StartPos.y + i, StartPos.z);

                if (MapData[i, j] == (int)TileType.Floor)
                {
                    GameObject floor = Instantiate(Floor1Prefab, position, Quaternion.identity);
                    GameObject floorArea = MyMethod.FindObject("FloorArea");
                    floor.transform.SetParent(floorArea.transform, true);
                }

            }
        }

        var MapData1D = MyMethod.ToOneDimensional(MapData);

        //Master以外がステージデータを受け取り生成
        photonView.RPC(nameof(ReceiveStageData), RpcTarget.OthersBuffered, MapData1D);
    }


    //Master以外がステージデータを受け取り生成
    [PunRPC]
    public void ReceiveStageData(int[] mapData1D)
    {
        var mapData2D = MyMethod.ToTowDimensional(mapData1D, width, height);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 position = new Vector3(StartPos.x + j, StartPos.y + i, 0);
                if (mapData2D[i, j] == (int)TileType.Floor)
                {
                    GameObject floor = Instantiate(Floor1Prefab, position, Quaternion.identity);
                    GameObject floorArea = MyMethod.FindObject("FloorArea");
                    floor.transform.SetParent(floorArea.transform, true);
                }
            }
        }

        // photonView.RPC(nameof(SetTurnPlayer), RpcTarget.AllBuffered, (int)PlayerNum.Player1);

        //終わったらゲーム開始
        photonView.RPC(nameof(GameStart), RpcTarget.AllBuffered);
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
