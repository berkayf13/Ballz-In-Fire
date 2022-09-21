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
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CylinderObstacle obs))
        {
                var obsPos = obs.transform.position;
                var bulletPos = transform.position;
                obsPos.y = bulletPos.y;
                var inNormal = (bulletPos - obsPos).normalized;
                _rb.velocity = Vector3.Reflect(_rb.velocity, inNormal);
        }
    }





}
