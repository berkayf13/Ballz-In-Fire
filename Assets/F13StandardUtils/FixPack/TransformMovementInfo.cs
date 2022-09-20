using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TransformMovementInfo : MonoBehaviour
{
    [SerializeField,ReadOnly] private Vector3 _lastPosition;
    [SerializeField,ReadOnly] private Vector3 _deltaMove;
    [SerializeField,ReadOnly] private Vector3 _velocity;
    [SerializeField,ReadOnly] private float _speed;

    public Vector3 Velocity => _velocity;
    
    public Vector3 DeltaMove => _deltaMove;

    public float Speed => _speed;


    private void Update()
    {
        var position = transform.position;
        _deltaMove = position-_lastPosition;
        _velocity = _deltaMove / Time.deltaTime;
        _speed = _velocity.magnitude;
        _lastPosition = position;
    }

    private Rigidbody _rigidbody;
    public void SpeedToRigidbody(float multiplier=1f)
    {
        if (_rigidbody || TryGetComponent(out _rigidbody))
        {
            _rigidbody.velocity = _velocity * multiplier;
        }
        else
        {
            Debug.LogError("There is no rigidbody attached this object",gameObject);
        }
    }
}
