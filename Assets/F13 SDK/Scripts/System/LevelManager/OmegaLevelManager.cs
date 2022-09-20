using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Assets.F13SDK.Scripts
{
    public enum ABType
    {
        A,
        B
    }
    
    public enum LevelType
    {
        Normal,
        Bonus,
        Tutorial
    }

    [System.Serializable]
    public class LevelSet
    {
        public List<Level> levels=new List<Level>();
    }

    public class OmegaLevelManager : OmegaSingletonManager<OmegaLevelManager>
    {
        public static ABType LEVEL_SET=ABType.A;
        private OmegaLevelManager()
        {
        }
        public List<LevelSet> levelSets=new List<LevelSet>();
        public List<Level> Levels => levelSets[(int) LEVEL_SET].levels;

        [BoxGroup("Current Level Info")] public Level currentLevel;

        [BoxGroup("Current Level Info")] [PreviewField]
        public GameObject currentLevelObject;

        [InfoBox(
            "Levels are contains of your all levels. Add their levelIds and prefabs to work with them.\n Levels have 3 different options. Choose the level type. ")]

        public static OmegaEventManager.GameLevelHandler On_BeforeLevelInitialized;
        public static OmegaEventManager.GameLevelHandler On_BeforeLevelDestroy;
        public static OmegaEventManager.GameLevelHandler On_AfterLevelDestroy;
        public static OmegaEventManager.GameLevelHandler On_LevelInitialized;
        public static OmegaEventManager.GameLevelHandler On_LevelCompleted;
        public static OmegaEventManager.GameLevelHandler On_LevelStarted;
        public static OmegaEventManager.GameLevelHandler On_LevelFailed;

        public void AwakeOmegaLevelManager()
        {
            GamePlayState.Instance.Gameplay_OnEntered += OnGamePlayEnter;
            UIInputManager.On_NextLevelButton += RestartLevel;
            UIInputManager.On_PreLevelButton += SetPreLevel;
            UIInputManager.On_ResetLevelButton += ResetLevels;
            UIInputManager.On_RestartLevelButton += RestartLevel;

            if (GetCurrentLevelId() == 0) SetCurrentLevelId(1);
            var levelIndex = GetLevelIndex(GetCurrentLevelId());
            InitiliazeLevel(levelIndex);
        }

        [Button]
        private void SetPreLevel()
        {
            DestroyCurrentLevelObject();
            var preLevelId = GetCurrentLevelId() - 1;
            var preLevelIndex = GetLevelIndex(preLevelId);
            InitiliazeLevel(currentLevel.levelId);
        }

        [Button]
        public void RestartLevel()
        {
            DestroyCurrentLevelObject();
            var levelId = GetCurrentLevelId();
            var levelIndex = GetLevelIndex(levelId);
            InitiliazeLevel(levelIndex);
        }

        [Button]
        public void SetNextLevel()
        {
            DestroyCurrentLevelObject();
            var nextLevelId = GetCurrentLevelId() + 1;
            var nextLevelIndex = GetLevelIndex(nextLevelId);
            InitiliazeLevel(nextLevelIndex);
        }


        [Button]
        public void ResetLevels()
        {
            DestroyCurrentLevelObject();
            InitiliazeLevel(0);
        }

        public bool isLevelStart = false;

        private void OnGamePlayEnter()
        {
            if (isLevelStart) return;
            isLevelStart = true;
            StartLevel();
        }

        private void StartLevel()
        {
            var levelId = GetCurrentLevelId();
            // Elephant.LevelStarted(levelId);
            Debug.Log("ELEPHANT Level started :" + levelId);
            On_LevelStarted.Invoke();
        }

        [Button]
        public void Fail()
        {
            if (!currentLevel.IsLevelFailed)
            {
                currentLevel.IsLevelFailed = true;
                On_LevelFailed.Invoke();
                var levelId = GetCurrentLevelId();
                // Elephant.LevelFailed(levelId);
                Debug.Log("ELEPHANT Level failed :" + levelId);
            }
        }

        [Button]
        public void Success()
        {
            if (!currentLevel.IsLevelCompleted)
            {
                currentLevel.IsLevelCompleted = true;
                On_LevelCompleted.Invoke();
                var levelId = GetCurrentLevelId();
                SetCurrentLevelId(levelId + 1);
                // Elephant.LevelCompleted(levelId);
                Debug.Log("ELEPHANT Level completed :" + levelId);
            }
        }

        public void SetCurrentLevelId(int id)
        {
            id = Mathf.Clamp(id, 1, int.MaxValue);
            PlayerPrefsManager.Instance.PlayerData.Level = id;
        }

        public int GetCurrentLevelId()
        {
            return PlayerPrefsManager.Instance.PlayerData.Level;
        }


        [Button]
        private int GetLevelIndex(int levelId)
        {
            if (levelId <= 0) levelId = 1;
            var index = levelId - 1;
            var levelCount = Levels.Count;
            var turn = index / levelCount;
            var mod = index % levelCount;
            var inc = turn > 0 ? Math.PI * levelId : 0;
            return (mod + (int) inc) % levelCount;
        }

        [Button]
        private int[] GetLevelIndexTest(int maxLevel = 100)
        {
            int[] array = new int[20];

            for (var i = 1; i < maxLevel; i++)
            {
                Debug.Log("levelId: " + i + " levelIndex: " + GetLevelIndex(i));
                array[GetLevelIndex(i)]++;
            }

            return array;
        }

        public void DestroyCurrentLevelObject()
        {
            if (currentLevelObject)
            {
                On_BeforeLevelDestroy.Invoke();
                Destroy(currentLevelObject);
                On_AfterLevelDestroy.Invoke();
            }
        }

        private void InitiliazeLevel(int levelIndex)
        {
            StartCoroutine(InitiliazeLevelCoroutine(levelIndex));
        }

        private IEnumerator InitiliazeLevelCoroutine(int levelIndex)
        {
            yield return new WaitForEndOfFrame();
            currentLevel = Levels[levelIndex];
            isLevelStart = false;
            currentLevel.IsLevelCompleted = false;
            currentLevel.IsLevelFailed = false;
            On_BeforeLevelInitialized.Invoke();
            currentLevelObject = Instantiate(currentLevel.levelPrefab);
            On_LevelInitialized.Invoke();
            currentLevelObject.SetActive(true);
        }
    }
}