using System;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public abstract class BaseObjectDisabler : BaseObjectUpdater<bool>
    {
        [SerializeField] private GameObject _disabledObject;
        protected override void OnValueUpdate()
        {
            _disabledObject.SetActive(Value);
        }
    }
}