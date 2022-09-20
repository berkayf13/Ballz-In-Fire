using System;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.Core
{
    public abstract class BaseUpdateableText<T> : MonoBehaviour where T:IEquatable<T>,IFormattable
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        protected T lastValue;
        protected abstract string ValueToString();
        protected abstract T Value();

        public TextMeshProUGUI TMP => _tmp;

        protected virtual void Awake()
        {
            UpdateValue();
        }
        
        protected virtual void LateUpdate()
        {
            if (!lastValue.Equals(Value()))
            {
                UpdateValue();
            }
        }

        private void UpdateValue()
        {
            _tmp.text = ValueToString();
            lastValue = Value();
        }
    }
}