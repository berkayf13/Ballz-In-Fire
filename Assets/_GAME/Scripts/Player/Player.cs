using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            BackBouncy();
        }
        if (other.TryGetComponent(out CylinderObstacle cylinder))
        {
            if (cylinder.final)
            {
                GameController.Instance.SuccessLevel();
                MoveZ.Instance.isMove = false;
                SwerveController.Instance.canSwerve = false;
                SwerveController.Instance.Reset();
            }
            else
            {
                BackBouncy();
            }
                
        }
        if (other.CompareTag("Finish"))
        {
            
                GameController.Instance.SuccessLevel();
                MoveZ.Instance.isMove = false;
                SwerveController.Instance.canSwerve = false;
                SwerveController.Instance.Reset();

        }
    }

    private void BackBouncy()
    {
        MoveZ.Instance.isMove = false;
        transform.DOMoveZ(transform.position.z - 20, 1f).OnComplete(() => MoveZ.Instance.isMove = true);
    }
}
