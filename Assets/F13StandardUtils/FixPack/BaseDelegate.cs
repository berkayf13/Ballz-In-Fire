using System;
using ThirdParty.Tools.Serializablecallback.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable]
    public class SerializedCallback<T>: SerializableCallback<T>{}
    [System.Serializable]
    public class SerializedEvent<T>: UnityEvent<T>{}

    public abstract class BaseDelegate<T> : BaseObjectUpdater<T> where T: IEquatable<T>
    {
        [SerializeField] private SerializedCallback<T> _callback;
        
        [SerializeField] private SerializedEvent<T> _onValueChanged;

        protected override T Value => _callback != null ? _callback.Invoke() : default(T);

        protected override void OnValueUpdate()
        {
            _onValueChanged.Invoke(Value);
        }
    }
}