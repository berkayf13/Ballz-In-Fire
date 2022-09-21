using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out CylinderObstacle obs))
        {
            MoveZ.Instance.isMove = false;
            transform.DOMoveZ(transform.position.z - 20, 1f).SetEase(Ease.OutElastic).OnComplete(()=>MoveZ.Instance.isMove=true);
        }

    }


}
