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
    private RewardedAd rewardedAd;
    private bool isRewarded = false;
    // Use this for initialization
    void Start()
    {
        adMob = this;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        RequestReward();

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
        bannerView = new BannerView(bannerId, AdSize.IABBanner, AdPosition.Bottom);
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
        if (bannerView == null)
            RequestBanner();
        else
            bannerView.Show();
    }
    public void RequestInterstitial()
    {
        InterstitialAd interstitial;
        interstitial = new InterstitialAd(interstitialId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("interstitial not loaded");
        }

    }

    public void RequestReward()
    {
        rewardedAd = new RewardedAd(rewardId);

        // Load�������Ɏ��s����֐��̓o�^
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Load���s���Ɏ��s����֐��̓o�^
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // �\�����Ɏ��s����֐��̓o�^
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // �\�����s���Ɏ��s����֐��̓o�^
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // ��V�󂯎�莞�Ɏ��s����֐��̓o�^
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // �L������鎞�Ɏ��s����֐��̓o�^
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
