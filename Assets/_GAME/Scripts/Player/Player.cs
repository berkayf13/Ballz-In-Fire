using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out CylinderObstacle obs))
        {
            MoveZ.Instance.isMove = false;
            transform.DOMoveZ(transform.position.z - 20, 1f).OnComplete(()=>MoveZ.Instance.isMove=true);
        }

    }


}
