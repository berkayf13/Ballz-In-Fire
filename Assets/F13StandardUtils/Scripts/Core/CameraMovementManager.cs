using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public enum CameraMovementType
{
    Drag,
    Corner
}

public class CameraMovementManager : Singleton<CameraMovementManager>
{
    [SerializeField] private List<BaseCameraMovementStrategy> _movementStrategies=new List<BaseCameraMovementStrategy>();

    [SerializeField,OnValueChanged(nameof(UpdateCameraMovementType))] private CameraMovementType _current;

    public BaseCameraMovementStrategy MovementStrategy => _movementStrategies[(int)_current];

    private void Awake()
    {
        UpdateCameraMovementType();
    }

    private void UpdateCameraMovementType()
    {
        for (var i = 0; i < _movementStrategies.Count; i++)
        {
            _movementStrategies[i].enabled = i == (int)_current;
        }
    }

    public void SetCameraMovementType(CameraMovementType movementType)
    {
        _current = movementType;
        UpdateCameraMovementType();
    }

    public void SetClamp(float minX, float maxX, float minZ, float maxZ)
    {
        foreach (var movementStrategy in _movementStrategies)
        {
            movementStrategy.minClampX = minX;
            movementStrategy.maxClampX = maxX;
            movementStrategy.minClampZ = minZ;
            movementStrategy.maxClampZ = maxZ;

        }
    }
    
    public void GoTo(float normalizedX, float normalizedZ , float duration=0)
    {
        var movementStrategy = MovementStrategy;
        var pos = Vector3.right *(movementStrategy.minClampX+ (movementStrategy.maxClampX-movementStrategy.minClampX)*normalizedX) + 
                  Vector3.forward *(movementStrategy.minClampZ+ (movementStrategy.maxClampZ-movementStrategy.minClampZ)*normalizedZ) +
                  Vector3.up * transform.position.y;
        if (duration > 0)
        {
            transform.DOMove(pos, duration).OnStart(()=>movementStrategy.enabled=false).OnComplete(()=>movementStrategy.enabled=true);
        }
        else
        {
            transform.position = pos;

        }
    }
    
    public CameraMovementType CurrentMovementType => _current;
}
