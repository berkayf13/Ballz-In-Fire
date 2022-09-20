using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class BlendShapeUpdater : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinned;
        [SerializeField] private int _index;
        

        [Button]
        public void SetValue(float value) => _skinned.SetBlendShapeWeight(_index, value);

        public float Value => _skinned.GetBlendShapeWeight(_index);
    }
}