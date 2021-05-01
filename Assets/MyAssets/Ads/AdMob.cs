using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;


public class AdMob : MonoBehaviour
{
    public static AdMob adMob;
    public string appId;
    public string bannerId;
    public string interstitialId;
    public string rewardId;

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private bool isRewarded = false;
    // Use this for initialization
    void Start()
    {
        adMob = this;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        RequestReward();
        RequestInterstitial();

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
    public void HideBanner()
    {
        if (bannerView == null) return;
        bannerView.Hide();
        bannerView.Destroy();
        bannerView = null;
    }
    public void ShowBanner()
    {
        if (bannerView != null)
        {
            return;
        }
        else
        {
            RequestBanner();
        }
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
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
    public void HandleInterstitialAdLoaded(object sender, EventArgs args)
    {

    }
    public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }
    public void HandleInterstitialAdOpening(object sender, EventArgs args)
    {

    }
    public void HandleInterstitialAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
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
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }
    public void GetReward()
    {
        Debug.Log("Show Reward");
    }

    public void CreateAndLoadRewardedAd()
    {
        rewardedAd = new RewardedAd(rewardId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);

    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        CreateAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        isRewarded = true;
    }
}
