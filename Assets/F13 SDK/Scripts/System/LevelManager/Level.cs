using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    [System.Serializable]
    public class Level
    {
        public LevelType levelType;
        public GameObject levelPrefab;
        public int levelId;
        public bool IsLevelCompleted = false;
        public bool IsLevelFailed = false;

    }
}
