using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRotate : MonoBehaviour
{
    [SerializeField] private Vector3 destinationEuler=Vector3.zero;
    [SerializeField] private float speed=45f;

    public abstract Quaternion Current { get; set;}

    protected virtual void Awake()
    {
        destinationEuler = Current.eulerAngles;
    }

    protected virtual  void Update()
    {
        var dest = Quaternion.Euler(destinationEuler);
        var angle = Quaternion.Angle(Current, dest);
        if (angle > float.Epsilon)
        {
            Current=Quaternion.RotateTowards(Current, dest, speed*Time.deltaTime);
        }
    }

    public float Angle(Vector3 euler)
    {
        var dest = Quaternion.Euler(euler);
        var angle = Quaternion.Angle(Current, dest);
        return angle;
    }

    public void SetCurrent(Vector3 euler) => Current = Quaternion.Euler(euler);

    public void SetDestination(Vector3 euler) => destinationEuler = euler;
    public void SetDestinationX(float angleX) => destinationEuler.x = angleX;
    public void SetDestinationY(float angleY) => destinationEuler.y = angleY;
    public void SetDestinationZ(float angleZ) => destinationEuler.z = angleZ;
    public float DestinationX => destinationEuler.x;
    public float DestinationY => destinationEuler.y;
    public float DestinationZ => destinationEuler.z;
    public float CurrentX => Current.eulerAngles.x;
    public float CurrentY => Current.eulerAngles.y;
    public float CurrentZ => Current.eulerAngles.z;

}
