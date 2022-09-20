using System;
using _GAME.Scripts.level5.NewScripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BaseRotate))]
public class ShakeRotation : MonoBehaviour
{
    [SerializeField] private BaseRotate _rotate;
    [SerializeField] private Vector3 _shake;
    [SerializeField] private bool _isShaking = false;
    [SerializeField] private float _shakeDirection = 1;
    [SerializeField, Range(-1f, 1f)] private float _startAt = 0;
    [SerializeField, ReadOnly, ShowIf(nameof(_isShaking))] private Vector3 currentEuler;

    public bool IsShaking
    {
        get => _isShaking;
        set => _isShaking = value;
    }

    private void Reset()
    {
        TryGetComponent(out _rotate);
    }

    private bool _lastIsShaking = false;
    protected void Update()
    {
        if (_isShaking != _lastIsShaking)
        {
            _lastIsShaking = _isShaking;
            if (_isShaking)
            {
                currentEuler = _rotate.Current.eulerAngles;
                _rotate.SetCurrent(currentEuler+_startAt*_shake);
                _rotate.SetDestination(currentEuler+_shakeDirection*_shake);
            }
            else
            {
                _rotate.SetDestination(currentEuler);
            }
        }
        else if(_isShaking)
        {
            var angle = _rotate.Angle(currentEuler+_shakeDirection*_shake);
            if (angle<=float.Epsilon)
            {
                _shakeDirection *= -1;
                _rotate.SetDestination(currentEuler+_shakeDirection*_shake);
            }
        }

    }
}
