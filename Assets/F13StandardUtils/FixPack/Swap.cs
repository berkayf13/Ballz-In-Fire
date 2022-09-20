using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Swap : MonoBehaviour
{
    [SerializeField] private BaseMove _move;
    [SerializeField] private float swapDistance = 0.1f;
    [SerializeField,ReadOnly] private Swap _connected;
    [SerializeField,ReadOnly] private bool _isSwapping;
    [SerializeField,ReadOnly] private float _distance;
    public SerializedEvent<GameObject> OnSwap=new SerializedEvent<GameObject>();

    public bool IsSwapping
    {
        get => _isSwapping;
        set
        {
            _isSwapping = value;
        }
    }
    public BaseMove Move => _move;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Swap swapping))
        {
            _connected = swapping;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Swap swapping))
        {
            if(swapping.Equals(_connected)) _connected = null;

        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_connected && other.gameObject.Equals(_connected.gameObject))
        {
            if(_isSwapping || _connected._isSwapping) return;

            var diff = _connected.Move.CurrentPosition - Move.CurrentPosition;
            _distance = diff.magnitude;
            var closeEnough = _distance  < swapDistance;
            if(closeEnough) DoSwap(_connected);
        }
    }

    public void DoSwap(Swap swapping)
    {
        IsSwapping = true;
        swapping.IsSwapping = true;
        var pos = _move.Destination;
        var swapPos = swapping.Move.Destination;
        _move.SetDestination(swapPos);
        swapping.Move.SetDestination(pos);
        OnSwap.Invoke(swapping.gameObject);
        swapping.OnSwap.Invoke(gameObject);
        this.StartWaitUntilCoroutine(()=> !_move.IsMoving, () =>
        {
            IsSwapping = false;
        });
        this.StartWaitUntilCoroutine(()=> !swapping.Move.IsMoving, () =>
        {
            swapping.IsSwapping = false;
        });
    }
}
