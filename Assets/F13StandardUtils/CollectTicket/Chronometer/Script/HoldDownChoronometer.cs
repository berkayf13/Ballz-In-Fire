using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HoldDownChoronometer : MonoBehaviour
{
    [SerializeField] private Chronometer _chronometer;
    

    private void OnMouseDown()
    {
        _chronometer.Resume();
    }

    private void OnMouseUp()
    {
        _chronometer.Pause();
    }
}
