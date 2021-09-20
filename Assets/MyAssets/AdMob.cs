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
    bool BannerIsDisplay = false;
    bool normalBannerIsLoaded = false;
    bool largeBannerIsLoaded = false;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private bool isRewarded = false;

    public Text logtext;
    private void Awake()
    {
        if (adMob == null)
        {
            adMob = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        RequestNormalBanner();
        RequestLargeBanner();
        RequestBanner();
    }
    // Use this for initialization
    void Start()
    {



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
        Debug.Log("in");
        if (largeBannerIsDisplay)
        {
            Debug.Log("isdisplaylarge");
            return;
        }
        Debug.Log("show large");
        largeBanner.Show();
        largeBannerIsDisplay = true;

    }
    public void ShowBanner()
    {
        if (BannerIsDisplay) return;
        Debug.Log("show banner");
        bannerView.Show();
        BannerIsDisplay = true;


    }
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

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            BannerIsDisplay = false;
        }
    }

    public void HandleOnNormalBannerLoaded(object sender, EventArgs args)
    {
        //  print("HandleAdLoaded event received");
        normalBannerIsLoaded = true;
        normalBanner.Hide();
    }
    public void HandleOnLargeBannerLoaded(object sender, EventArgs args)
    {
        // print("HandleAdLoaded event received");
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

        // Loadê¨å˜éûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Loadé∏îséûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // ï\é¶éûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // ï\é¶é∏îséûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // ïÒèVéÛÇØéÊÇËéûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // çLçêÇï¬Ç∂ÇÈéûÇ…é¿çsÇ∑ÇÈä÷êîÇÃìoò^
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public void ShowRewardAd()
    {
        //ç≈èâÇÕÇ±Ç±Ç…ÇÕÇ¢ÇÈ
        if (rewardedAd == null)
        {
            RequestReward();
            logtext.text = "show";
        }
        else
        {
            //Ç±Ç±Ç…ÇÕì¸ÇÁÇ»Ç¢ÇÕÇ∏
            if (rewardedAd.IsLoaded())
            {
                logtext.text = "show";
                rewardedAd.Show();
            }
            elseÅ@//äÓñ{Ç±Ç¡ÇøÇ…ì¸ÇÈ
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
        //ì«Ç›çûÇ›Ç™èIÇÌÇ¡ÇƒÇ©ÇÁShowÇ∑ÇÈ
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
