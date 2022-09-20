using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class WheelRotateWithDrag : MonoBehaviour
{
    [SerializeField,ReadOnly] private bool _isRotating;
    [SerializeField] private float _speed = 100;
    [SerializeField] private float _clampDistance = 1.5f;
    private Vector3 _lastWorldPoint;

    public bool IsRotating => _isRotating;

    public UnityEvent OnRotateStart = new UnityEvent();
    public UnityEvent OnRotateEnd = new UnityEvent();
    public UnityEvent OnRotating = new UnityEvent();

    private void Reset()
    {
        CalculateClampDistance();
    }

    protected void Update()
    {
        if (_isRotating)
        {
            var worldPlanePoint = WorldPoint();
            if (Vector3.Distance(worldPlanePoint, _lastWorldPoint) > float.Epsilon)
            {
                var diffVector = ( worldPlanePoint- _lastWorldPoint);
                var fromCenter = worldPlanePoint-transform.position;
                var cross = Vector3.Cross(fromCenter,diffVector);
                transform.Rotate(cross*_speed);
                _lastWorldPoint = worldPlanePoint;
                OnRotating.Invoke();
            }
        }
    }
    
    protected void OnMouseDown()
    {
        _isRotating = true;
        _lastWorldPoint = WorldPoint();
        OnRotateStart.Invoke();
    }
    

    protected void OnMouseUp()
    {
        _isRotating = false;
        OnRotateEnd.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (_isRotating)
        {
            Gizmos.DrawSphere(_lastWorldPoint,0.1f);
        }
    }

    private Vector3 WorldPoint()
    {
        Camera.main.ScreenToWorldPlanePoint(Input.mousePosition, out Vector3 worldPlanePoint, planeY: transform.position.y);
        var diff = worldPlanePoint-transform.position;
        diff = diff.normalized * Mathf.Clamp(diff.magnitude, 0f, _clampDistance);
        worldPlanePoint = transform.position + diff;
        return worldPlanePoint;
    }
    
    [Button]
    private void CalculateClampDistance()
    {
        if (TryGetComponent(out Collider collider))
        {
            _clampDistance = collider.bounds.size.magnitude * 0.33f;
        }
    }
    
    
}
