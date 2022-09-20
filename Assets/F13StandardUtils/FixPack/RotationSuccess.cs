using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


public class RotationSuccess : BaseObjectUpdater<bool>
{
    [SerializeField] private Vector3 targetEuler=Vector3.zero;
    [SerializeField] private float thresh=10f;

    [ShowInInspector] public float Angle => Quaternion.Angle(transform.rotation, Quaternion.Euler(targetEuler));
    [ShowInInspector] public float SuccessRatio
    {
        get
        {
            var angle = Angle;
            return angle <= thresh ? (1f-angle/thresh) : 0f;
        }
    }


    public UnityEvent OnSuccess=new UnityEvent();
    public UnityEvent OnFail=new UnityEvent();


    public bool IsSuccess => Angle <= thresh;
    
    protected override bool Value => IsSuccess;

    protected override void OnValueUpdate()
    {
        var value = Value;
        if (value)
        {
            Debug.Log("Rotation Success");
            OnSuccess.Invoke();
        }
        else
        {
            Debug.Log("Rotation Fail");
            OnFail.Invoke();
        }
    }
}
