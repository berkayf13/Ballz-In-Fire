using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class TargetValueRange : BaseObjectUpdater<float>
    {
        [SerializeField] private float _value;
        [SerializeField] protected float _targetValue;
        [SerializeField] protected float _thresh=0.5f;
        [SerializeField] protected bool _isClamp = false;
        [SerializeField,ShowIf(nameof(_isClamp))] private float minClamp = -1;
        [SerializeField,ShowIf(nameof(_isClamp))] private float maxClamp = 1;
        
        public SerializedEvent<float> OnSuccess = new SerializedEvent<float>();
        public SerializedEvent<float> OnFail = new SerializedEvent<float>();

        [SerializeField, ReadOnly] private bool _isSuccess = false;

        protected override float Value => _value;

        public float GetValue() => _value;
        public void SetValue(float value) => _value = value;

        protected override void OnValueUpdate()
        {
            if (_isClamp)
            {
                _value = Mathf.Clamp(_value, minClamp, maxClamp);
            }
            var diff = Math.Abs(_targetValue - _value);
            var success = diff < _thresh;
            if (_isSuccess != success)
            {
                _isSuccess = success;
                if(_isSuccess)
                    OnSuccess.Invoke(_value);
                else
                    OnFail.Invoke(_value);
            }
        }
    }
}