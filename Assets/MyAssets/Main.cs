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

    public bool isMaster { get; set; }

    public enum PlayerNum
    {
        Player1 = 1,
        Player2 = 2,
    }
    public int TurnPlayer { get; set; } = 0;




    public GameObject Player1 { get; set; }
    public GameObject Player2 { get; set; }

    public GameObject HomeScreen;
    public GameObject ConnectionScreen;
    public GameObject BattleScreen;

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
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["TurnPlayer"] = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MatchMaking.matchMake.StartMatchMaking("aaa");
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
        MatchMaking.matchMake.StartMatchMaking("aaa");
        ChangeActive(HomeScreen, ConnectionScreen);
        ChangeTurn(PlayerNum.Player1);
        photonView.RPC(nameof(ChangeTurn), RpcTarget.AllBuffered, PlayerNum.Player1);

    }


    [PunRPC]
    private void ChangeTurn(PlayerNum playerNum)
    {
        TurnPlayer = (int)playerNum;
        Debug.Log(TurnPlayer);
    }
    [PunRPC]
    private void ChangeTurn()
    {
        if (TurnPlayer == (int)PlayerNum.Player1)
        {
            TurnPlayer = (int)PlayerNum.Player2;
        }
        else if (TurnPlayer == (int)PlayerNum.Player2)
        {
            TurnPlayer = (int)PlayerNum.Player1;
        }

    }

    /// <summary>
    /// スクリーンの切り替えとか
    /// </summary>
    /// <param name="falseObj"></param>
    /// <param name="trueObj"></param>
    public void ChangeActive(GameObject falseObj, GameObject trueObj)
    {
        if (falseObj.activeSelf)
            falseObj.SetActive(false);
        if (!trueObj.activeSelf)
            trueObj.SetActive(true);
    }

    public void CheckPlayerIsMove()
    {
        StartCoroutine(CheckPlayerIsMoveCoroutine());
    }
    IEnumerator CheckPlayerIsMoveCoroutine()
    {
        int a = 0;
        while (a < 1000000)
        {
            yield return new WaitForSeconds(0.5f);
            a++;
            bool IsAllPlayerStop = false;
            foreach (var player in AllPlayers)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb.velocity == Vector3.zero)
                {
                    IsAllPlayerStop = true;
                }
                else
                {
                    IsAllPlayerStop = false;
                }
            }

            //全員が止まってたらOK
            if (IsAllPlayerStop)
            {
                ChangeTurn();
                photonView.RPC(nameof(ChangeTurn), RpcTarget.AllBuffered);
                yield break;
            }

        }
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
