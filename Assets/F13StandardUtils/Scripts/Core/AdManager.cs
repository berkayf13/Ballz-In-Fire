using System;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class AdManager : Singleton<AdManager>
    {
        public static float INTERSTITIAL_INTERVAL = 30f;
        public static int INTERSTITIAL_MIN_LEVEL = 2;

        [SerializeField] private RLAIntegrator _rlaIntegrator;

        private float _lastInterstitialTime;
        public bool IsInsterstitialConditionOK => Application.isPlaying &&
                                                  GameController.Instance.Level >= INTERSTITIAL_MIN_LEVEL &&
                                                  (Time.time - _lastInterstitialTime) > INTERSTITIAL_INTERVAL;

        public bool IsInterstitialReady => _rlaIntegrator.IsInterstitialReady;
        public bool IsRewardedReady => _rlaIntegrator.IsRewardedReady;
        public bool IsInit => _rlaIntegrator.IsInit;

    

        public void ShowAdInterstitial(Action onComplete=null)
        {
            if (IsInsterstitialConditionOK)
            {
                _rlaIntegrator.ShowAdInterstitial(()=>
                {
                    _lastInterstitialTime = Time.time;
                    onComplete?.Invoke();
                });
            }
            else
            {
                onComplete.Invoke();
            }
        }

        public void ShowAdRewarded(Action onComplete=null, Action onFail=null)
        {
            _lastInterstitialTime = Time.time;
            _rlaIntegrator.ShowAdRewarded(onComplete,onFail);
        }
    
    }
}