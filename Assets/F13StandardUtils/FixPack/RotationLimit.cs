using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLimit : MonoBehaviour
{
    [SerializeField] private Vector3 targetEuler=Vector3.zero;
    [SerializeField] private float maxAngle=20;

    private void LateUpdate()
    {
        var angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetEuler));
        if (angle > maxAngle)
        {
            transform.rotation=Quaternion.Lerp(Quaternion.Euler(targetEuler), transform.rotation, maxAngle / angle);
        }
    }
}
