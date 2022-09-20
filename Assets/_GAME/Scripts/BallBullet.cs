using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBullet : MonoBehaviour
{
    [SerializeField] public Rigidbody _rb;
    public void SetVelocity(Vector3 velocity)
    {
        _rb.velocity = velocity;

    }
}
