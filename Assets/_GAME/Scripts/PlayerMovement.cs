using F13StandardUtils.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _lastSwerve;
    private bool _onlyOnce;
    private void OnEnable()
    {
        SwerveController.Instance.OnSwerveChanged.AddListener(OnSwerveChanged);

    }


    private void OnDisable()
    {
        SwerveController.Instance?.OnSwerveChanged.RemoveListener(OnSwerveChanged);

    }

    private void LateUpdate()
    {
        if (GameController.Instance.IsPlaying && !_onlyOnce) MoveZ.Instance.isMove = true;
    }


    private void OnSwerveChanged(float arg0)
    {
        var pos = transform.position;
        var transformRight = transform.right * (arg0 - _lastSwerve);
        transform.position = pos + transformRight;
        _lastSwerve = arg0;
    }


}
