using System;
using Assets.F13SDK.Scripts;
// using ElephantSDK;
using F13StandardUtils.CrowdDynamics.Scripts;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable] public class LevelEvent:UnityEvent<int>{}
    public class GameController : Singleton<GameController>
    {
        public static string ENCRYPT = "geypazari";
        public bool IsPlaying => GameManager.Instance.IsGamePlayState;
        public static int VERSION_NUMBER => 105;
        public static int MIN_VERSION_NUMBER = 104;
        [SerializeField, ReadOnly] private int _level;

        public int SavedLevel => PlayerData.Level;
        public int Level => _level;

        public UnityEvent OnGameplayEnter = new UnityEvent();
        public UnityEvent OnGameplayExit = new UnityEvent();
        public UnityEvent OnGameplayUpdate = new UnityEvent();
        public UnityEvent OnGameplayFixedUpdate = new UnityEvent();
        public UnityEvent OnGameplayLateUpdate = new UnityEvent();
        public LevelEvent OnBeforeLevelInit=new LevelEvent();
        public LevelEvent OnLevelInit=new LevelEvent();
        public LevelEvent OnLevelStart=new LevelEvent();
        public LevelEvent OnLevelComplete=new LevelEvent();
        public LevelEvent OnLevelFail=new LevelEvent();
        
        

        public PlayerData PlayerData => PlayerPrefsManager.Instance.PlayerData;
        public RemoteConfigParams RemoteConfig => RemoteConfigParams.Instance;

        private void Awake()
        {
            // RemoteConfig.ApplyConfig();
        }

        private void OnEnable()
        {
            GamePlayState.Instance.Gameplay_OnEntered += OnGamePlayEnter;
            GamePlayState.Instance.Gameplay_OnExited += OnGamePlayExit;
            GamePlayState.Instance.Gameplay_OnExecuted += OnUpdate;
            GamePlayState.Instance.Gameplay_OnFixedExecuted += OnFixedUpdate;
            GamePlayState.Instance.Gameplay_OnLateExecuted += OnLateUpdate;
            OmegaLevelManager.On_BeforeLevelInitialized += OnBeforeLevelInitialized;
            OmegaLevelManager.On_BeforeLevelDestroy += OnBeforeLevelDestroy;
            OmegaLevelManager.On_AfterLevelDestroy += OnAfterLevelDestroy;
            OmegaLevelManager.On_LevelInitialized += OnLevelInitialized;
            OmegaLevelManager.On_LevelStarted += OnLevelStarted;
            OmegaLevelManager.On_LevelCompleted += OnLevelCompleted;
            OmegaLevelManager.On_LevelFailed += OnLevelFailed;
            
        }

        private void OnDisable()
        {
            GamePlayState.Instance.Gameplay_OnEntered -= OnGamePlayEnter;
            GamePlayState.Instance.Gameplay_OnExited -= OnGamePlayExit;
            GamePlayState.Instance.Gameplay_OnExecuted -= OnUpdate;
            GamePlayState.Instance.Gameplay_OnFixedExecuted -= OnFixedUpdate;
            GamePlayState.Instance.Gameplay_OnLateExecuted -= OnLateUpdate;
            OmegaLevelManager.On_BeforeLevelInitialized -= OnBeforeLevelInitialized;
            OmegaLevelManager.On_BeforeLevelDestroy -= OnBeforeLevelDestroy;
            OmegaLevelManager.On_AfterLevelDestroy -= OnAfterLevelDestroy;
            OmegaLevelManager.On_LevelInitialized -= OnLevelInitialized;
            OmegaLevelManager.On_LevelStarted -= OnLevelStarted;
            OmegaLevelManager.On_LevelCompleted -= OnLevelCompleted;
            OmegaLevelManager.On_LevelFailed -= OnLevelFailed;
        }
        
        private void OnUpdate()
        {
            OnGameplayUpdate.Invoke();
            //TODO 
        }
        
        private void OnFixedUpdate()
        {
            OnGameplayFixedUpdate.Invoke();
            //TODO
        }

        private void OnLateUpdate()
        {
            OnGameplayLateUpdate.Invoke();
            //TODO
        }
        
        private void OnGamePlayEnter()
        {
            OnGameplayEnter.Invoke();
            //TODO
        }

        private void OnGamePlayExit()
        {
            OnGameplayExit.Invoke();
            //TODO
        }
        
        private void OnBeforeLevelInitialized()
        {
            _level = OmegaLevelManager.Instance.GetCurrentLevelId();
            OnBeforeLevelInit.Invoke(Level);
            //TODO
        }
        
        private void OnLevelInitialized()
        {
            _level = OmegaLevelManager.Instance.GetCurrentLevelId();
            OnLevelInit.Invoke(Level);
            //TODO
        }
        
        private void OnBeforeLevelDestroy()
        {
            PoolManager.Instance.CleanGhostsInPools();
            //TODO
        }

        private void OnAfterLevelDestroy()
        {
            //TODO
        }
        
        private void OnLevelStarted()
        {
            _level = OmegaLevelManager.Instance.GetCurrentLevelId();
            // SendEvent(GameEvents.gen_levelStartWeapon,Level,Params.New().Set("gunType",(int)GunShopManager.Instance.GetSelectedGun()));
            OnLevelStart.Invoke(Level);
            //TODO

        }

        private void OnLevelCompleted()
        {
            _level = OmegaLevelManager.Instance.GetCurrentLevelId();
            OnLevelComplete.Invoke(Level);
            HeavyHaptic();
            //TODO
        }
        
        private void OnLevelFailed()
        {
            _level = OmegaLevelManager.Instance.GetCurrentLevelId();
            OnLevelFail.Invoke(Level);
            HeavyHaptic();
            //TODO
        }
        
        public void EnterGamePlayState()=> GameManager.Instance.SetState(GamePlayState.Instance);


        [Button]
        public void SetLevel(int level)
        {
            level = Mathf.Clamp(level, 1, int.MaxValue);
            OmegaLevelManager.Instance.SetCurrentLevelId(level);
            OmegaLevelManager.Instance.RestartLevel();
            GameManager.Instance.SetState(MainMenuState.Instance);
        }
        
        [Button]
        public void SuccessLevel()
        {
            OmegaLevelManager.Instance.Success();
        }

        [Button]
        public void FailLevel()
        {
            OmegaLevelManager.Instance.Fail();
        }

        public void ShowAdInterstitial(Action onComplete)
        {
            AdManager.Instance.ShowAdInterstitial(onComplete);
        }

        public void ShowAdRewarded(Action onComplete, Action onFail)
        {
            AdManager.Instance.ShowAdRewarded(onComplete,onFail);
        }
        
        public void LightHaptic()
        {
            OmegaHapticManager.Instance.LightHaptic();
        }
        public void MediumHaptic()
        {
            OmegaHapticManager.Instance.MediumHaptic();
        }
        public void HeavyHaptic()
        {
            OmegaHapticManager.Instance.HeavyHaptic();
        }

        public void SendEvent(GameEvents ev,int level/*,Params parameters=null*/)
        {
            // Elephant.Event(ev.ToString(),level,parameters);
            // Debug.Log("ELEPHANT "+ev+" (level:"+level+")"+(parameters!=null?" "+ParamsLogString(parameters):string.Empty));
        }
        private string ParamsLogString(/*Params parameters*/)
        {
            var str = string.Empty;
            // if (parameters.stringVals.Any()) str += " stringVals: " + JsonConvert.SerializeObject(parameters.stringVals);
            // if (parameters.intVals.Any()) str += " intVals: " + JsonConvert.SerializeObject(parameters.intVals);
            // if (parameters.doubleVals.Any()) str += " doubleVals: " + JsonConvert.SerializeObject(parameters.doubleVals);
            // if (!parameters.customData.IsNullOrWhitespace()) str += " customData: " + parameters.customData;
            return str;
        }
    }

    public enum GameEvents
    {
        gen_manPower,
        rew_manPower,
        gen_income,
        rew_income,
        gen_fireRate,
        rew_fireRate,
        gen_fireDamage,
        rew_fireDamage,
        rew_claim,
        gen_noThanks,
        int_noThanks,
        gen_levelFailed,
        int_levelFailed,
        gen_restart,
        int_restart,
        shp_openShopButton,
        rew_shopPanelWeaponClaim,
        rew_weaponPanelClaim,
        fin_weaponPanelNoThanks,
        gen_levelStartWeapon,
        gen_cheater

    }
}