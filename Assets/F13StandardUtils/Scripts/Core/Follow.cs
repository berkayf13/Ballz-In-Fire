using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _following;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool localOfset = false;

    [Button]
    private void LoadOfset()
    {
        if(_following)
            offset = transform.position - _following.transform.position;
    }
    
    [Button]
    private void UpdateOffset()
    {
        var followPos = _following.transform.position;
        followPos += localOfset ? _following.TransformDirection(offset) : offset;
        transform.position = followPos;
    }

    private void LateUpdate()
    {
        UpdateOffset();
    }
}
