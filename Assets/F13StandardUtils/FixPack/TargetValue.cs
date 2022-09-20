using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class TargetValue : BaseObjectUpdater<float>
    {
        [SerializeField] private float _value;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;



        public SerializedEvent<float> OnMaxValue = new SerializedEvent<float>();
        public SerializedEvent<float> OnDecreaseFromMaxValue = new SerializedEvent<float>();
        public SerializedEvent<float> OnMinValue = new SerializedEvent<float>();
        public SerializedEvent<float> OnIncreaseFromMinValue = new SerializedEvent<float>();
        public SerializedEvent<float> OnValue = new SerializedEvent<float>();


        protected override float Value => _value;

        public float GetValue() => _value;
        public void SetValue(float value) => _value = value;

        protected override void OnValueUpdate()
        {
            _value = Mathf.Clamp(_value,_minValue, _maxValue);
            if (Mathf.Approximately(_value, _minValue))
            {
                OnMinValue.Invoke(_value);
                Debug.Log("OnMinValue"+_value);
            }
            else if (Mathf.Approximately(_value, _minValue))
            {
                OnMaxValue.Invoke(_value);
                Debug.Log("OnMaxValue"+_value);
            }
            else
            {
                OnValue.Invoke(_value);
                if (Mathf.Approximately(lastValue, _minValue))
                {
                    OnIncreaseFromMinValue.Invoke(_value);
                    Debug.Log("OnIncreaseFromMinValue"+_value);
                }
                else if (Mathf.Approximately(lastValue, _minValue))
                {
                    OnDecreaseFromMaxValue.Invoke(_value);
                    Debug.Log("OnDecreaseFromMaxValue"+_value);
                }
            }
        }
    }
}