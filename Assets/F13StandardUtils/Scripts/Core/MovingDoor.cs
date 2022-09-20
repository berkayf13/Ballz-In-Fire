using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    [SerializeField] private bool _isMove;
    [SerializeField] private float _rangeOfMove;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _direction = 1f;
    private Vector3 _defaultPosition;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
    }

    private void Update()
    {
        DoorMovement();
    }

    private void DoorMovement()
    {
        if ( _isMove)
        {
            var diff = (transform.localPosition - _defaultPosition).magnitude;
            var remaining = _rangeOfMove - diff;
            if (remaining <= 0.01f)
            {
                _direction *= -1;
            }

            var translateAmound = _moveSpeed * Time.deltaTime;
            translateAmound = remaining > 0.01f && remaining <= translateAmound ? remaining : translateAmound;
            var translate = Vector3.right * translateAmound * _direction;
            transform.Translate(translate);
        }
    }
}
