using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using static AdMob;

public class Main : MonoBehaviour
{
    public static Main main;
    public SaveData saveData;

    private void Awake()
    {
        main = this;
        //まだセーブファイルが作成されてないとき初期化
        if (SaveSystem.Load() == null)
        {
            Debug.Log("first");
            InitializeClass();
        }
        else
        {
            MainLoad();
        }
    }
    void Start()
    {
        StartCoroutine("MainSave", 1f);
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
    public void MainLoad()
    {
        Debug.Log("load");
        saveData = SaveSystem.Load();
        Debug.Log(saveData.playerdata.level);
    }

    void InitializeClass()
    {
        SaveData.SD.playerdata = new PlayerData();
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
