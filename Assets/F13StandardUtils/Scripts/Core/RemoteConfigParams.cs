using System;
using System.Collections.Generic;
using Assets.F13SDK.Scripts;
// using ElephantSDK;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable]
    public class RemoteConfigParams : Singleton<RemoteConfigParams>
    {
        //General
        public int genMinVersionNumber;
        public int gen_levelSetAB;
        public float gen_moveZSpeed;
        public float gen_moveZRotateDuration;
        public int gen_rateUsLevel;
        public float gen_hapticInterval;
        public int gen_poolCleanSize;


        //Incrementals
        public List<int> inc_incomeCosts=new List<int>();
        public List<int> inc_manpowerCosts=new List<int>();
        public List<int> inc_fireRateCosts=new List<int>();
        public List<int> inc_fireDamageCosts=new List<int>();
        public int inc_incomeStart;
        public int inc_manPowerStart;
        public int inc_incomeMultiplier;
        public int inc_manPowerMultiplier;
        public float inc_minFireRateMultiplier;
        public float inc_maxFireDamageMultiplier;
        //Crowd
        public float cro_pullPower;
        public float cro_pullDelay;
        public float cro_circularInterval;
        public float cro_circularRandomization;
        public float cro_memberScale;

        //Finish
        public int fin_finishAB;
        public float fin_duration;
        public float fin_noThanksButtonDelay;

        //Ad
        public float ads_interstitialInterval;
        public int ads_interstitialMinLevel;



        // private RemoteConfig remoteConfig;
        

        public void ApplyConfig()
        {
            LoadConfig();
            SetConfig();
        }

        private void LoadConfig()
        {
            // remoteConfig = RemoteConfig.GetInstance();
            // //General
            // genMinVersionNumber = remoteConfig.GetInt(nameof(genMinVersionNumber), GameController.MIN_VERSION_NUMBER);
            // gen_levelSetAB = remoteConfig.GetInt(nameof(gen_levelSetAB), (int)OmegaLevelManager.LEVEL_SET);
            // gen_moveZSpeed = remoteConfig.GetFloat(nameof(gen_moveZSpeed), MoveZ.VELOCITY);
            // gen_moveZRotateDuration = remoteConfig.GetFloat(nameof(gen_moveZRotateDuration), MoveZ.ROTATE_DURATION);
            // gen_rateUsLevel = remoteConfig.GetInt(nameof(gen_rateUsLevel), RateManager.RATE_US_LEVEL);
            // gen_hapticInterval = remoteConfig.GetFloat(nameof(gen_hapticInterval), OmegaHapticManager.HAPTIC_INTERVAL);
            // gen_poolCleanSize = remoteConfig.GetInt(nameof(gen_poolCleanSize), PoolManager.POOL_CLEAN_SIZE);
            //
            // //Incrementals
            // inc_manpowerCosts = RemoteConfigGetList(nameof(inc_manpowerCosts), MoneyManager.MAN_POWER_UPGRADE_COSTS);
            // inc_incomeCosts = RemoteConfigGetList(nameof(inc_incomeCosts), MoneyManager.INCOME_UPGRADE_COSTS);
            // inc_fireRateCosts = RemoteConfigGetList(nameof(inc_fireRateCosts), MoneyManager.FIRE_RATE_UPGRADE_COSTS);
            // inc_fireDamageCosts = RemoteConfigGetList(nameof(inc_fireDamageCosts), MoneyManager.FIRE_DAMAGE_UPGRADE_COSTS);
            // inc_manPowerStart = remoteConfig.GetInt(nameof(inc_manPowerStart), MoneyManager.MANPOWER_START);
            // inc_incomeStart = remoteConfig.GetInt(nameof(inc_incomeStart), MoneyManager.INCOME_START);
            // inc_manPowerMultiplier = remoteConfig.GetInt(nameof(inc_manPowerMultiplier), MoneyManager.MANPOWER_MULTIPLIER);
            // inc_incomeMultiplier = remoteConfig.GetInt(nameof(inc_incomeMultiplier), MoneyManager.INCOME_MULTIPLIER);
            // inc_minFireRateMultiplier = remoteConfig.GetFloat(nameof(inc_minFireRateMultiplier), MoneyManager.MIN_FIRE_RATE_MULTIPLIER);
            // inc_maxFireDamageMultiplier = remoteConfig.GetFloat(nameof(inc_maxFireDamageMultiplier), MoneyManager.MAX_FIRE_DAMAGE_MULTIPLIER);
            // //Finish
            // fin_finishAB = remoteConfig.GetInt(nameof(fin_finishAB), (int)FinishManager.FINISH_TYPE);
            // fin_duration = remoteConfig.GetFloat(nameof(fin_duration), FinishManager.TOTAL_DURATION);
            // fin_noThanksButtonDelay = remoteConfig.GetFloat(nameof(fin_noThanksButtonDelay), Claim.NO_THANKS_BUTTON_DELAY);
            // //Crowd
            // cro_circularInterval = remoteConfig.GetFloat(nameof(cro_circularInterval), CrowdManager.CIRCULAR_INTERVAL);
            // cro_circularRandomization = remoteConfig.GetFloat(nameof(cro_circularRandomization), CrowdManager.CIRCULAR_RANDOMIZATION);
            // cro_pullPower = remoteConfig.GetFloat(nameof(cro_pullPower), CrowdManager.PULL_POWER);
            // cro_pullDelay = remoteConfig.GetFloat(nameof(cro_pullDelay), CrowdManager.DEFAULT_PULL_DELAY);
            // cro_memberScale = remoteConfig.GetFloat(nameof(cro_memberScale), CrowdMember.DEFAULT_SCALE);
            // //Ad
            // ads_interstitialInterval = remoteConfig.GetFloat(nameof(ads_interstitialInterval), AdManager.INTERSTITIAL_INTERVAL);
            // ads_interstitialMinLevel = remoteConfig.GetInt(nameof(ads_interstitialMinLevel), AdManager.INTERSTITIAL_MIN_LEVEL);


        }
        private void SetConfig()
        {
            // //General
            // GameController.MIN_VERSION_NUMBER = Mathf.Clamp(genMinVersionNumber, 1, int.MaxValue);
            // OmegaLevelManager.LEVEL_SET = (ABType)Mathf.Clamp(gen_levelSetAB,0,Enum.GetValues(typeof(ABType)).Length-1);
            // MoveZ.VELOCITY = Mathf.Clamp(gen_moveZSpeed,-50f,0f);
            // MoveZ.ROTATE_DURATION = Mathf.Clamp(gen_moveZRotateDuration,0.1f,5f);
            // RateManager.RATE_US_LEVEL = Mathf.Clamp(gen_rateUsLevel,1,int.MaxValue);
            // OmegaHapticManager.HAPTIC_INTERVAL = Mathf.Clamp(gen_hapticInterval,0.01f,2f);
            // PoolManager.POOL_CLEAN_SIZE = Mathf.Clamp(gen_poolCleanSize,100,500);
            //
            // //Incrementals
            // MoneyManager.MAN_POWER_UPGRADE_COSTS = inc_manpowerCosts.Clamp(1,int.MaxValue);;
            // MoneyManager.INCOME_UPGRADE_COSTS = inc_incomeCosts.Clamp(1,int.MaxValue);
            // MoneyManager.FIRE_RATE_UPGRADE_COSTS = inc_fireRateCosts.Clamp(1,int.MaxValue);
            // MoneyManager.FIRE_DAMAGE_UPGRADE_COSTS = inc_fireDamageCosts.Clamp(1,int.MaxValue);
            // MoneyManager.MANPOWER_START = Mathf.Clamp(inc_manPowerStart,1,100);
            // MoneyManager.INCOME_START = Mathf.Clamp(inc_incomeStart,1,999999);
            // MoneyManager.MANPOWER_MULTIPLIER = Mathf.Clamp(inc_manPowerMultiplier,1,10);
            // MoneyManager.INCOME_MULTIPLIER = Mathf.Clamp(inc_incomeMultiplier,1,99999);
            // MoneyManager.MIN_FIRE_RATE_MULTIPLIER =  Mathf.Clamp(inc_minFireRateMultiplier,0.5f,1f);
            // MoneyManager.MAX_FIRE_DAMAGE_MULTIPLIER = Mathf.Clamp(inc_maxFireDamageMultiplier,1f,2f);
            // //Finish
            // FinishManager.FINISH_TYPE = (ABType)Mathf.Clamp(fin_finishAB,0,Enum.GetValues(typeof(ABType)).Length-1);
            // FinishManager.TOTAL_DURATION = Mathf.Clamp(fin_duration,1f,10f);
            // Claim.NO_THANKS_BUTTON_DELAY = Mathf.Clamp(fin_noThanksButtonDelay,0f,10f);
            // //Crowd
            // CrowdManager.CIRCULAR_INTERVAL = Mathf.Clamp(cro_circularInterval,0.25f,5f);
            // CrowdManager.CIRCULAR_RANDOMIZATION = Mathf.Clamp(cro_circularRandomization,0f,CrowdManager.CIRCULAR_INTERVAL*0.5f);
            // CrowdManager.RECTANGULAR_INTERVAL = FinishManager.FINISH_TYPE == ABType.A ? 4 : 0;
            // CrowdManager.PULL_POWER = Mathf.Clamp(cro_pullPower,0.01f,30f);
            // CrowdManager.DEFAULT_PULL_DELAY = Mathf.Clamp(cro_pullDelay,0.01f,2f);
            // CrowdMember.DEFAULT_SCALE = Mathf.Clamp(cro_memberScale,0.5f,2.5f);
            //
            // //Ad
            // AdManager.INTERSTITIAL_INTERVAL = Mathf.Clamp(ads_interstitialInterval,0f,float.MaxValue);
            // AdManager.INTERSTITIAL_MIN_LEVEL = Mathf.Clamp(ads_interstitialMinLevel,1,int.MaxValue);

        }
        

        // private List<T> RemoteConfigGetList<T>(string key,List<T> defaultList)
        // {
        //     var json = remoteConfig.Get(key,Utils.ToJson(key,defaultList));
        //     return Utils.FromJson<T>(key, json);
        // }
        

        

    }
    
}