using F13StandardUtils.Crowd.Scripts;
using F13StandardUtils.CrowdDynamics.Scripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class RotateAroundTrigger : MonoBehaviour
{
    [SerializeField] private Collider _triggerCollider;
    [SerializeField] private Transform _rotatePivot;
    [SerializeField] private bool isLeft = false;
    [SerializeField] private float rotate = 90;
    [SerializeField] private PlayerType _trigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(_trigger.ToString()))
        {
            var crowdMember = other.GetComponent<CrowdMember>();
            if (crowdMember)
            {
                RotateAround();
            }
        }
    }

    [Button]
    private void RotateAround()
    {
        MoveZ.Instance.RotateAround(isLeft?Vector3.up:-Vector3.up, _rotatePivot,rotate,onStart: () =>
        {
            
        }, onComplete: () =>
        {

        });
        _triggerCollider.enabled = false;
    }
}