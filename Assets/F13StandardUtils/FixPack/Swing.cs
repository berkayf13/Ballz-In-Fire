using System;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private Vector3 targetEuler=Vector3.zero;
    [SerializeField] private float maxAngle=20;
    [SerializeField] private Vector3 power = new Vector3(0,50f,0);
    [SerializeField] private float returnPower=0.1f;


    private Vector3 lastPosition;

    private void OnEnable()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        var position = transform.position;
        var diff = position - lastPosition;
        if (!Mathf.Approximately(diff.magnitude, 0))
        {
            var dir = Quaternion.Euler(0, 0, -90) * diff;
            var rotateAmount = Vector3.Scale(dir, power);
            transform.Rotate(rotateAmount);
            var angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetEuler));
            if (angle > maxAngle)
            {
                transform.rotation=Quaternion.Lerp(Quaternion.Euler(targetEuler), transform.rotation, maxAngle / angle);
            }
        }
        else
        {
            transform.rotation=Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetEuler), returnPower);;
        }

        

        
        lastPosition = position;
    }
}
