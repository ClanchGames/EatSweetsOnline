using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.UI;


public class AdMob : MonoBehaviour
{
    public static AdMob adMob;
    public string appId;
    public string bannerId;
    public string normalBannerId;
    public string largeBannerId;
    public string interstitialId;
    public string rewardId;

    private BannerView bannerView;
    private BannerView normalBanner;
    private BannerView largeBanner;
    bool normalBannerIsDisplay = false;
    bool largeBannerIsDisplay = false;
    bool normalBannerIsLoaded = false;
    bool largeBannerIsLoaded = false;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private bool isRewarded = false;

    public Text logtext;
    // Use this for initialization
    void Start()
    {
        adMob = this;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        RequestNormalBanner();
        RequestLargeBanner();


    }
    private void Update()
    {
        if (isRewarded)
        {
            isRewarded = false;
            GetReward();
        }
    }
    public void Test()
    {
        Debug.Log("test");
    }
    void RequestNormalBanner()
    {
        if (normalBanner != null)
        {
            normalBanner.Destroy();
        }
        normalBanner = new BannerView(normalBannerId, AdSize.Banner, AdPosition.Bottom);

        normalBanner.OnAdLoaded += HandleOnNormalBannerLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        normalBanner.LoadAd(request);
    }
    void RequestLargeBanner()
    {
        if (largeBanner != null)
        {
            largeBanner.Destroy();
        }

        largeBanner = new BannerView(largeBannerId, AdSize.MediumRectangle, AdPosition.Center);
        largeBanner.OnAdLoaded += HandleOnLargeBannerLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        largeBanner.LoadAd(request);
    }

    public void RequestBanner()
    {
        if (bannerView != null)
        {
            return;
        }
        AdSize adaptiveSize =
              AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(bannerId, adaptiveSize, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
    public void ShowNormalBanner()
    {
        if (normalBannerIsDisplay) return;
        Debug.Log("show normal");
        normalBanner.Show();
        normalBannerIsDisplay = true;
    }
    public void ShowLargeBanner()
    {
        if (largeBannerIsDisplay) return;
        Debug.Log("show large");
        largeBanner.Show();
        largeBannerIsDisplay = true;

    }
    /* public IEnumerator ShowNormalBanner()
     {
         if (normalBannerIsDisplay)
         {
             yield break;
         }
         int timeout = 1000;
         float trytime = Time.realtimeSinceStartup;
         if (normalBanner == null)
         {
             Debug.Log("normalbanner null so request and show");
             RequestNormalBanner();
             yield return new WaitUntil(() => normalBannerIsLoaded || Time.realtimeSinceStartup - trytime > timeout);
             if (normalBannerIsLoaded)
             {
                 normalBanner.Show();
                 normalBannerIsDisplay = true;
             }
             else if (Time.realtimeSinceStartup - trytime > timeout)
             {
                 Debug.Log("normal banner fail to load");
             }

         }
         else
         {

             Debug.Log("show normal");
             normalBanner.Show();
             normalBannerIsDisplay = true;
         }
     }
     public IEnumerator ShowLargeBanner()
     {
         if (largeBannerIsDisplay)
         {
             yield break;
         }
         int timeout = 1000;
         float trytime = Time.realtimeSinceStartup;
         if (largeBanner == null)
         {
             Debug.Log("largebanner null so request and show");
             RequestLargeBanner();
             yield return new WaitUntil(() => largeBannerIsLoaded || Time.realtimeSinceStartup - trytime > timeout);
             if (largeBannerIsLoaded)
             {
                 largeBanner.Show();
                 largeBannerIsDisplay = true;
             }
             else if (Time.realtimeSinceStartup - trytime > timeout)
             {
                 Debug.Log("large banner fail to load");
             }

         }
         else
         {
             Debug.Log("show large");
             largeBanner.Show();
             largeBannerIsDisplay = true;
         }
     }*/
    public void HideNormalBanner()
    {
        if (normalBanner != null)
        {
            normalBanner.Hide();
            normalBannerIsDisplay = false;
        }
    }

    public void HideLargeBanner()
    {
        if (largeBanner != null)
        {
            largeBanner.Hide();
            largeBannerIsDisplay = false;
        }
    }
    public void ShowBanner()
    {
        /* if (bannerView != null)
         {
             return;
         }
         else
         {
             RequestBanner();
         }*/


    }
    public void HideBanner()
    {
        /* if (bannerView == null) return;
         bannerView.Hide();
         bannerView.Destroy();
         bannerView = null;*/
    }

    public void HandleOnNormalBannerLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received");
        normalBannerIsLoaded = true;
        normalBanner.Hide();
    }
    public void HandleOnLargeBannerLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received");
        largeBannerIsLoaded = true;
        largeBanner.Hide();
    }

    public void RequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(interstitialId);

        interstitial.OnAdLoaded += HandleInterstitialAdLoaded;
        interstitial.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialAdOpening;
        interstitial.OnAdClosed += HandleInterstitialAdClosed;
        interstitial.OnAdLeavingApplication += HandleInterstitialAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    public void ShowInterstitial()
    {
        RequestInterstitial();
    }
    public void HandleInterstitialAdLoaded(object sender, EventArgs args)
    {
        interstitial.Show();
    }
    public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DebugSystem.debug.Log("fail to load interstitial");
    }
    public void HandleInterstitialAdOpening(object sender, EventArgs args)
    {

    }
    public void HandleInterstitialAdClosed(object sender, EventArgs args)
    {
        Debug.Log("interstitial close");
    }
    public void HandleInterstitialAdLeavingApplication(object sender, EventArgs args)
    {

    }

    public void RequestReward()
    {
        rewardedAd = new RewardedAd(rewardId);

        // Load¬Œ÷‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Load¸”s‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // •\¦‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // •\¦¸”s‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // •ñVó‚¯æ‚è‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // L‚ğ•Â‚¶‚é‚ÉÀs‚·‚éŠÖ”‚Ì“o˜^
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public void ShowRewardAd()
    {
        //Å‰‚Í‚±‚±‚É‚Í‚¢‚é
        if (rewardedAd == null)
        {
            RequestReward();
            logtext.text = "show";
        }
        else
        {
            //‚±‚±‚É‚Í“ü‚ç‚È‚¢‚Í‚¸
            if (rewardedAd.IsLoaded())
            {
                logtext.text = "show";
                rewardedAd.Show();
            }
            else@//Šî–{‚±‚Á‚¿‚É“ü‚é
            {
                logtext.text = "fail to load so request";
                RequestReward();
            }
        }

    }


    public void GetReward()
    {
        Debug.Log("Show Reward");
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
        logtext.text = "reward loaded";
        //“Ç‚İ‚İ‚ªI‚í‚Á‚Ä‚©‚çShow‚·‚é
        rewardedAd.Show();

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
        logtext.text = "failed to load:" + args.Message;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
        logtext.text = "reward open";
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
        logtext.text = "failed to show";
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        logtext.text = "close reward ";
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        isRewarded = true;
        logtext.text = "earned reward";
    }
}
