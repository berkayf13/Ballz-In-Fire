using System;
// using RollicGames.Advertisements;
using UnityEngine;

public class RLAIntegrator : MonoBehaviour
{
    
#if UNITY_IOS
    private readonly string[] _bannerAdUnits = { "f1f19fd4c1070dcd" };
    private readonly string[] _interstitialAdUnits = { "dd0395de52fe08ba" };
    private readonly string[] _rewardedVideoAdUnits = { "ef327d96a5866ecb" };

#elif UNITY_ANDROID || UNITY_EDITOR
    private readonly string[] _bannerAdUnits = { "7fb94d4956933331" };
    private readonly string[] _interstitialAdUnits = { "c46b8be269980114" };
    private readonly string[] _rewardedVideoAdUnits = { "dca15f6680338209" };
#endif

    // private string _appKey = "p1IwTLy2_wURoJRVgSE_V4zLeis_BuXM6RLw2XVXJ5O0-2btoJzPI99nTGVQQdTBqaI3QRaQ20uzb-hTSurhGF";

    public bool IsInterstitialReady => IsInit /*&& RLAdvertisementManager.Instance && RLAdvertisementManager.Instance.isInterstitialReady()*/;
    public bool IsRewardedReady => IsInit /*&& RLAdvertisementManager.Instance && RLAdvertisementManager.Instance.isRewardedVideoAvailable()*/;

    private bool _isInit = false;

    public bool IsInit => _isInit;

    // private void Awake() {
    //     RLAdvertisementManager.Instance.init(_appKey, _rewardedVideoAdUnits, _bannerAdUnits, _interstitialAdUnits);
    //     RLAdvertisementManager.OnRollicAdsSdkInitializedEvent += OnRollicAdsSdkInitialized;
    //     RLAdvertisementManager.OnRollicAdsAdLoadedEvent += OnRollicAdsAdLoaded;
    // }

    // private void OnDestroy()
    // {
    //     RLAdvertisementManager.OnRollicAdsSdkInitializedEvent -= OnRollicAdsSdkInitialized;
    //     RLAdvertisementManager.OnRollicAdsAdLoadedEvent -= OnRollicAdsAdLoaded;
    // }

    private void OnRollicAdsSdkInitialized(string adUnitId)
    {
        _isInit = true;
        LoadBanner();
    }
    private void OnRollicAdsAdLoaded(string adUnitId)
    {
        ShowBanner();
    }

    public void ShowAdInterstitial(Action onComplete=null)
    {
        // if(RLAdvertisementManager.Instance.isInterstitialReady()) 
        // {
        //     RLAdvertisementManager.Instance.showInterstitial();
        //     if(onComplete!=null) RLAdvertisementManager.Instance.onInterstitialAdClosedEvent = onComplete;
        // }
        // else
        // {
            onComplete?.Invoke();
        // }
    }

    public void ShowAdRewarded(Action onComplete=null, Action onFail=null)
    {
        
        // if(RLAdvertisementManager.Instance.isRewardedVideoAvailable())
        // {
        //     RLAdvertisementManager.Instance.showRewardedVideo();
        //     RLAdvertisementManager.Instance.rewardedAdResultCallback = (result) =>
        //     {
        //         switch (result)
        //         {
        //             case RLRewardedAdResult.Failed:
        //                 onFail?.Invoke();
        //                 break;
        //             case RLRewardedAdResult.Skipped:
        //                 onFail?.Invoke();
        //                 break;
        //             case RLRewardedAdResult.Finished:
        //                 onComplete?.Invoke();
        //                 break;
        //             default:
        //                 throw new ArgumentOutOfRangeException(nameof(result), result, null);
        //         }
        //     };
        //
        // }
        // else
        // {
            onFail?.Invoke();
        // }
    }

    public void LoadBanner()
    {
        // RLAdvertisementManager.Instance.loadBanner();
    }

    public void ShowBanner()
    {
        // RLAdvertisementManager.Instance.showBanner();
    }
        
    public void HideBanner()
    {
        // RLAdvertisementManager.Instance.hideBanner();

    }
}
