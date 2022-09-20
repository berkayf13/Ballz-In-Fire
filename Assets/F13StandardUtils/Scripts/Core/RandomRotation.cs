using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomRotation : MonoBehaviour
{
    public bool rotateOnAwake = true;
    public bool isRotateX = false;
    [ShowIf(nameof(isRotateX))] public int rotateCountX = 1;
    public bool isRotateY = true;
    [ShowIf(nameof(isRotateY))] public int rotateCountY = 4;
    public bool isRotateZ = false;
    [ShowIf(nameof(isRotateZ))] public int rotateCountZ = 1;

    private void Awake()
    {
        if(rotateOnAwake) Random();
    }

    [Button]
    public void Random()
    {
        var euler = transform.eulerAngles;

        if (isRotateX)
        {
            var randomX = UnityEngine.Random.Range(0, rotateCountX);
            var degreeX = 360f / rotateCountX;
            euler.x = degreeX * randomX;
        }

        if (isRotateY)
        {
            var randomY = UnityEngine.Random.Range(0, rotateCountY);
            var degreeY = 360f / rotateCountY;
            euler.y = degreeY * randomY;
        }

        if (isRotateZ)
        {
            var randomZ = UnityEngine.Random.Range(0, rotateCountZ);
            var degreeZ = 360f / rotateCountZ;
            euler.z = degreeZ * randomZ;
        }
        transform.eulerAngles = euler;
    }
}
