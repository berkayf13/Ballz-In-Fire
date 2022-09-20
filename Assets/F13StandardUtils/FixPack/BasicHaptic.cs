using System;
using UnityEngine;

public class BasicHaptic : MonoBehaviour
{
    public static float HAPTIC_INTERVAL = 0.35f;
    
    public bool isActive = true;
    
    private float _lastHapticTime;

    public void SuccessHaptic()
    {
        if (isActive)
        {
            _lastHapticTime = Time.time;
            try
            {
                Taptic.Success();
                Debug.Log("BasicHaptic Success");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    public void FailedHaptic()
    {
        if (isActive)
        {
            if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
            {
                _lastHapticTime = Time.time;
                try
                {
                    Taptic.Failure();
                    Debug.Log("BasicHaptic Failure");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
    public void LightHaptic()
    {
        if (isActive)
        {
            if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
            {
                _lastHapticTime = Time.time;
                try
                {
                    Taptic.Light();
                    Debug.Log("BasicHaptic Light");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
    public void MediumHaptic()
    {
        if (isActive)
        {
            if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
            {
                _lastHapticTime = Time.time;
                try
                {
                    Taptic.Medium();
                    Debug.Log("BasicHaptic Medium");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
    public void HeavyHaptic()
    {
        if (isActive)
        {
            if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
            {
                _lastHapticTime = Time.time;
                try
                {
                    Taptic.Heavy();
                    Debug.Log("BasicHaptic Heavy");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

    }
    public void SetActive()
    {
        isActive = true;
        Debug.Log("BasicHaptic isActive=true");


    }
    public void SetDeactive()
    {
        isActive = false;
        Debug.Log("BasicHaptic isActive=false");
    }
}
