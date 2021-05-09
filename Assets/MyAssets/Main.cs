using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static AdMob;

public class Main : MonoBehaviour
{
    public static Main main;
    DebugSystem debug;
    public SaveData saveData;

    private void Awake()
    {
        main = this;
        debug = GetComponent<DebugSystem>();
        DebugSystem.debug = debug;
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
        //StartCoroutine("MainSave", 1f);
    }
    void Test()
    {
    }

    void Update()
    {

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
        DebugSystem.debug.Log("delete");
    }
    void InitializeClass()
    {
        SaveData.SD.playerdata = new PlayerData();
    }

    public void ShowNormalBanner()
    {
        StartCoroutine(adMob.ShowNormalBanner());
    }
    public void ShowLargeBanner()
    {
        StartCoroutine(adMob.ShowLargeBanner());
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
