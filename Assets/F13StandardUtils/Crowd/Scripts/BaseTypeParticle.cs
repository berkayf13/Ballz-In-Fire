using System;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CrowdDynamics.Scripts
{
    public abstract class BaseTypeParticle<T> : MonoBehaviour where T:Enum
    {
        private static float Delay = 1.5f;
        [SerializeField] private List<ParticleSystem> _particleSystems;

        protected abstract int ToInt(T e);


        [Button]
        public void Play(T type)
        {
            var selectedIndex = ToInt(type);
            for (var i = 0; i < _particleSystems.Count; i++)
            {
                _particleSystems[i].gameObject.SetActive(false);
            }
            var _particle = _particleSystems[selectedIndex];
            _particle.gameObject.SetActive(true);
            _particle.Play(true);
            Invoke(nameof(DestroyWithDelayProcess),Delay);
        }
    
        private void DestroyWithDelayProcess()
        {
            PoolManager.Instance.Destroy(this,this.GetType());
        }
    }
}