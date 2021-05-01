using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using static AdMob;

public class MainManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // AdMob.adMob.RequestBanner();
    }
    void Test()
    {
    }
    // Update is called once per frame
    void Update()
    {

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
        adMob.RequestInterstitial();
    }
    public void Reward()
    {
        adMob.ShowRewardAd();
    }
}
