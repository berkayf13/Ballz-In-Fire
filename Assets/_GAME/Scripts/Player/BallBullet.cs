using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBullet : MonoBehaviour
{
    [SerializeField] public Rigidbody _rb;
    public float range=2;
    public float bounce;

    private void Awake()
    {
        StartCoroutine(Range(range));
    }
    public void SetVelocity(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CylinderObstacle obs))
        {
            if (bounce>0)
            {
                var obsPos = obs.transform.position;
                var bulletPos = transform.position;
                obsPos.y = bulletPos.y;
                var inNormal = (bulletPos - obsPos).normalized;
                _rb.velocity = Vector3.Reflect(_rb.velocity, inNormal);
                bounce--;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    private IEnumerator Range(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }



}
