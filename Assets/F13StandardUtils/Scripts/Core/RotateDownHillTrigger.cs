using F13StandardUtils.Crowd.Scripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class RotateDownHillTrigger : MonoBehaviour
{
    [SerializeField] private Collider _enterCollider;
    [SerializeField] private Collider _exitCollider;
    [SerializeField] private Transform _rotatePivotEnter;
    [SerializeField] private Transform _rotatePivotExit;
    [SerializeField] private bool isDownHill = true;
    [SerializeField] private float rotate = 45;
    [SerializeField] private PlayerType _trigger;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(_trigger.ToString()))
        {
            if (_enterCollider.enabled)
            {
                DownHill();
            }
            else
            {
                UpHill();
            }
        }
    }

    [Button]
    private void DownHill()
    {
        MoveZ.Instance.isDownHill = true;
        MoveZ.Instance.SetMultiplier(MoveZ.DOWNHILL_VELOCITY_MULTIPLIER);
        MoveZ.Instance.RotateAround(isDownHill? -Vector3.right: Vector3.right, _rotatePivotEnter,rotate,false, () =>
        {
            CameraController.Instance.CameraToAction();
        },onComplete: () =>
        {
            CameraController.Instance.chasePlayer = true;
        });
        _enterCollider.enabled = false;
    }
    
    [Button]
    private void UpHill()
    {
        MoveZ.Instance.isDownHill = false;
        MoveZ.Instance.RotateAround(isDownHill? Vector3.right: -Vector3.right, _rotatePivotExit,rotate,false, () =>
        {
            CameraController.Instance.chasePlayer = false;
            CameraController.Instance.CameraToInGame();
        },onComplete: () =>
        {
            MoveZ.Instance.SetMultiplier(1);
        });
        _exitCollider.enabled = false;
    }
}