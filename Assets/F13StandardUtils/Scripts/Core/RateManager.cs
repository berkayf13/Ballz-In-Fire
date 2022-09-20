using System;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class RateManager : MonoBehaviour
{
    public static int RATE_US_LEVEL = 1;
    
    private void OnEnable()
    {
        GameController.Instance.OnLevelComplete.AddListener(OnLevelSuccess);
    }

    private void OnDisable()
    {
        GameController.Instance?.OnLevelComplete.RemoveListener(OnLevelSuccess);
    }

    private void OnLevelSuccess(int level)
    {
        if (RATE_US_LEVEL == level)
        {
            OpenRatePanel();
        }
    }
    
    private void OpenRatePanel()
    {
        Debug.LogWarning("RateUs");
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}
