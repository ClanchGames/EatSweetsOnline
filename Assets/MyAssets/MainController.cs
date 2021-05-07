using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using static AdMob;

public class MainController : MonoBehaviour
{
    SaveController SC;
    private void Awake()
    {
        SC = GetComponent<SaveController>();
        SaveController.SC = SC;
        if (SaveData.SD.isFirst)
        {
            SaveData.SD.isFirst = false;
            InitializeClass();
        }
        else
        {
            SaveController.SC.Load();
        }
    }
    void Start()
    {

    }
    void Test()
    {
    }

    void Update()
    {

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
