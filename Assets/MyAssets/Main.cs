using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static AdMob;

public class Main : MonoBehaviour
{
    public static Main main;
    DebugSystem debug;
    public SaveData saveData;

    public bool right;
    public bool left;

    public bool isPlayer1Turn = false;
    public bool isPlayer2Turn = false;

    public GameObject Player1;
    public GameObject Player2;
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
        //StartCoroutine("MainSave", 1f);
    }
    void Test()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MatchMaking.matchMake.StartMatchMaking("p1");
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
