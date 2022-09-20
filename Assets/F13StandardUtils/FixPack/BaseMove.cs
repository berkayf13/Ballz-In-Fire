using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseMove : MonoBehaviour
{
    [SerializeField] protected bool _instantMove = false;
    [SerializeField,HideIf(nameof(_instantMove))] private float _speed = 10f;
    
    [SerializeField] private Vector3 _destination;

    [SerializeField,ReadOnly] protected Vector3 _startPos;
    
    public abstract Vector3 CurrentPosition { get; set; }

    public Vector3 Destination => _destination;

    public float DistanceToDestination => Vector3.Distance(_destination, CurrentPosition);

    public bool IsMoving => enabled && !Mathf.Approximately(DistanceToDestination ,0);

    public bool InstantMove
    {
        get => _instantMove;
        set => _instantMove = value;
    }

    private void Awake()
    {
        _startPos = CurrentPosition;
        _destination = _startPos;

    }

    private void Update()
    {
        var diff = _destination - CurrentPosition;
        var diffMagnitude = diff.magnitude;
        if (!Mathf.Approximately(diffMagnitude, 0))
        {
            var diffAbs = Mathf.Abs(diffMagnitude);
            var translate = _speed * Time.deltaTime;
            translate = !_instantMove && diffAbs > translate ? translate : diffAbs;
            CurrentPosition+= diff.normalized * translate;
        }

    }


    public void SetDestination(Vector3 position)
    {
        _destination = position;
        if(_instantMove) Update();

    }

    public void ResetDestination()
    {
        _destination = _startPos;
        if(_instantMove) Update();
    }
    
    [Button]
    public void SetDestinationX(Single x)
    {
        var pos = _destination;
        pos.x = x;
        SetDestination(pos);
    }
    
    [Button]
    public void SetDestinationY(Single y)
    {
        var pos = _destination;
        pos.y = y;
        SetDestination(pos);
    }
    
    [Button]
    public void SetDestinationZ(Single z)
    {
        var pos = _destination;
        pos.z = z;
        SetDestination(pos);
    }
    

    public void SetDestinationX(int x) => SetDestinationX((Single) x);
    public void SetDestinationY(int y) => SetDestinationY((Single) y);
    public void SetDestinationZ(int z) => SetDestinationZ((Single) z);



}
