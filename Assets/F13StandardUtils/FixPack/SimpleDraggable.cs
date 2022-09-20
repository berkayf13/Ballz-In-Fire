using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SimpleDraggable : MonoBehaviour
{
    [SerializeField,OnValueChanged(nameof(UpdateIsActive))] private bool _isActive = true;
    [SerializeField,ReadOnly] private bool _isDragging = false;
    [SerializeField] private bool _isHandleMode = true;
    [SerializeField,ShowIf(nameof(_isHandleMode))] private float xOffset = 0f;
    [SerializeField,ShowIf(nameof(_isHandleMode))] private float yOffset = 0f;
    [SerializeField,ShowIf(nameof(_isHandleMode))] private float zOffset = 0.5f;
    [SerializeField] private bool isConstraintX = true;
    [SerializeField,ShowIf(nameof(isConstraintX))] private MoveConstraint constraintAxisX = new MoveConstraint(){minPoint = -1f , maxPoint = 1f};
    [SerializeField] private bool isConstraintZ = true;
    [SerializeField,ShowIf(nameof(isConstraintZ))] public MoveConstraint constraintAxisZ = new MoveConstraint(){minPoint = -2.5f , maxPoint = 2.5f};
    public UnityEvent OnDragStart=new UnityEvent();
    public UnityEvent OnDragEnd=new UnityEvent();
    public UnityEvent OnDragging=new UnityEvent();
    
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            UpdateIsActive();
        }
    }

    public bool IsDragging => _isDragging;


    private Vector3 _lastWorldPlanePoint;
    private void OnMouseDown()
    {
        if (!_isActive) return;
        if(!_isHandleMode) Camera.main.ScreenToWorldPlanePoint(Input.mousePosition, out _lastWorldPlanePoint, planeY: transform.position.y);
        _isDragging = true;
        OnDragStart.Invoke();
        

    }

    private void OnMouseUp()
    {
        if (!_isActive) return;
        _isDragging = false;
        OnDragEnd.Invoke();
    }

    private void OnMouseDrag()
    {
        if (!_isActive) return;
        if(!_isDragging) return;

        Vector3 worldPlanePoint;
        if (_isHandleMode)
        {
            Camera.main.ScreenToWorldPlanePoint(Input.mousePosition, out worldPlanePoint, planeY: yOffset);
            worldPlanePoint += Vector3.forward * zOffset + Vector3.right * xOffset;
        }
        else
        {
            Camera.main.ScreenToWorldPlanePoint(Input.mousePosition, out worldPlanePoint, planeY: transform.position.y);
            var diff = worldPlanePoint - _lastWorldPlanePoint;
            _lastWorldPlanePoint = worldPlanePoint;
            worldPlanePoint = transform.position + diff;
        }

        if(isConstraintX) worldPlanePoint.x = Mathf.Clamp(worldPlanePoint.x, constraintAxisX.minPoint, constraintAxisX.maxPoint);
        if(isConstraintZ) worldPlanePoint.z = Mathf.Clamp(worldPlanePoint.z, constraintAxisZ.minPoint, constraintAxisZ.maxPoint);
        if(Vector3.Distance(transform.position,worldPlanePoint)<=float.Epsilon) return;
        transform.position = worldPlanePoint;
        OnDragging.Invoke();

    }
    
    private void UpdateIsActive()
    {
        if(_isDragging) _isDragging = false;
    }

    [System.Serializable]
    public class MoveConstraint
    {
        public float minPoint;
        public float maxPoint;
    }
    public float XOffset => xOffset;
    public void SetXOffset(Single x)
    {
        xOffset = x;
    }
    public void SetXOffset(int x)
    {
        xOffset = x;
    }

    public float YOffset => yOffset;
    public void SetYOffset(Single y)
    {
        yOffset = y;
    }
    public void SetYOffset(int y)
    {
        yOffset = y;
    }
    
    public float ZOffset => zOffset;
    public void SetZOffset(Single z)
    {
        zOffset = z;
    }
    public void SetZOffset(int z)
    {
        zOffset = z;
    }
}