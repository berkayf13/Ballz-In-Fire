using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Core
{
    public enum InputState
    {
        Down,
        Up,
        Stay,
        None
    }
    
    [System.Serializable]
    public struct ReplayInputRecord
    {
        public float time;
        public Vector3 mousePosition;
        public InputState inputState;
    }
    
    [System.Serializable]
    public struct Replay
    {
        public string id;
        public List<ReplayInputRecord> record;

        [Button]
        public void Play()
        {
            if (Application.isPlaying)
            {
                ReplayManager.Instance?.SetReplay(id);
                ReplayManager.Instance?.Play();
            }
            else
            {
                Debug.LogWarning("Replay cannot play because app is not running");
            }
        }
        
        [Button]
        public void SetPlayOnAwake()
        {
            if (!Application.isPlaying)
            {
                var replayManager = Object.FindObjectOfType<ReplayManager>();
                replayManager.SetPlayOnAwake(id);
            }
            else
            {
                Debug.LogWarning("Replay cannot set play on awake because app is already running");
            }
        }

        public Replay Clone()
        {
            var r=new Replay();
            r.id = id;
            r.record=new List<ReplayInputRecord>(record);
            return r;
        }
    }
    
    
    [CreateAssetMenu(fileName = "ReplayData", menuName = "Replay/ReplayData", order = 1)]
    public class ReplayData:ScriptableObject
    {
        public List<Replay> replays=new List<Replay>();
    }
}