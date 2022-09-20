using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    [SerializeField,ReadOnly] protected Vector3 _startPos;
    
    public SerializedEvent<Vector3> OnPositionChanged=new SerializedEvent<Vector3>();

    [SerializeField] private bool _rememberChangesWhenEnabledAgain = false;
    
    public Vector3 Position => transform.position;
    public float PositionX => Position.x;
    public float PositionY => Position.y;
    public float PositionZ => Position.z;

    private void Awake()
    {
        _startPos = Position;
    }

    private void OnEnable()
    {
        if(_rememberChangesWhenEnabledAgain) 
            SetPosition(_startPos);
    }

    private void OnDisable()
    {
        if (_rememberChangesWhenEnabledAgain)
            _startPos = Position;
    }

    [Button]
    public void SetPosition(Vector3 position)
    {
        if (enabled)
        {
            transform.position = position;
            OnPositionChanged.Invoke(position);
        }
        else
        {
            _startPos = position;
        }


    }
    [Button]
    public void ResetPosition()
    {
        SetPosition(_startPos);
    }
    
    [Button]
    public void SetPositionX(Single x)
    {
        var pos = Position;
        pos.x = x;
        SetPosition(pos);
    }
    
    [Button]
    public void SetPositionY(Single y)
    {
        var pos = Position;
        pos.y = y;
        SetPosition(pos);
    }
    
    [Button]
    public void SetPositionZ(Single z)
    {
        var pos = Position;
        pos.z = z;
        SetPosition(pos);
    }
    

    public void SetPositionX(int x) => SetPositionX((Single) x);
    public void SetPositionY(int y) => SetPositionY((Single) y);
    public void SetPositionZ(int z) => SetPositionZ((Single) z);


}
