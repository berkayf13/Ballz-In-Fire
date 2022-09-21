using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float duration;
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform start,inGame,special,actionCam,finishCam;
    [SerializeField] private List<Transform> _pointList=new List<Transform>();
    [SerializeField] private Transform player;
    public bool chasePlayer=false;
    [SerializeField,ReadOnly] private Transform lastCam;
    public bool IsActionCam => actionCam.Equals(lastCam);

    public Vector3 ActionCameraOfsetForCount =>
        (Vector3.back * 0.03f + Vector3.up * 0.01f) /* Player.Instance.CrowdManager.Count*/;


    private void Update()
    {
        ChaseCam();
    }

    private void ChaseCam()
    {
        if (chasePlayer && player)
        {
            var camPos = transform.localPosition;
            var posX = Mathf.Lerp(camPos.x, player.transform.localPosition.x, Time.deltaTime * 10);
            var posZ = Mathf.Lerp(camPos.z, player.transform.localPosition.z, Time.deltaTime * 10);
            camPos.x = posX;
            camPos.z = posZ;
            transform.localPosition = camPos;
        }

    }
    
    private void CameraToTransform(Transform t, float durationMultiplier=1f,Action endAction=null)
    {
        DOTween.Kill(mainCam);
        lastCam = t;
        mainCam.DOLocalRotate(lastCam.localRotation.eulerAngles, duration*durationMultiplier);
        mainCam.DOLocalMove(lastCam.localPosition, duration*durationMultiplier).OnComplete(() =>
        {
            endAction?.Invoke();
        });
    }
    
    [Button]
    public void CameraToAction(Vector3 ofset,Action endAction=null)
    {
        DOTween.Kill(mainCam);
        lastCam = actionCam;
        mainCam.DOLocalMove(actionCam.localPosition+ofset, duration);
        mainCam.DOLocalRotate(actionCam.localRotation.eulerAngles, duration).OnComplete(() =>
        {
            endAction?.Invoke();
        });
    }
    
    public void CameraToAction(Action endAction=null)
    {
        CameraToAction(ActionCameraOfsetForCount,endAction);
    }
    
    [Button]
    public void CameraToListPoint(int pointIndex,Action endAction=null)
    {
        CameraToTransform(_pointList[pointIndex],endAction:endAction);
    }

    [Button]
    public void CameraToStart(Action endAction=null)
    {
        CameraToTransform(start,endAction:endAction);
    }

    [Button] 
    public void CameraToSpecial(Action endAction=null)
    {
        CameraToTransform(special,endAction:endAction);


    }

    [Button]
    public void CameraToInGame(Action endAction=null)
    {
        CameraToTransform(inGame,endAction:endAction);
    }
    [Button]
    public void CameraToFinish(Action endAction=null)
    {
        CameraToTransform(finishCam,2f,endAction:endAction);

    }
    
    [Button]
    public void ZoomOut(float zoom)
    {
        DOTween.Kill(mainCam);
        Vector3 targetPos = mainCam.localPosition - mainCam.forward * zoom;
        mainCam.DOLocalMove(targetPos, duration);
    }
    [Button]
    public void ZoomIn(float zoom)
    {
        DOTween.Kill(mainCam);
        Vector3 targetPos = mainCam.localPosition + mainCam.forward * zoom;
        mainCam.DOLocalMove(targetPos, duration);
    }
    [Button]
    public void LookChange(Vector3 angle)
    {
        DOTween.Kill(mainCam);
        Quaternion targetRot = mainCam.localRotation * Quaternion.Euler(angle);
        mainCam.DOLocalRotateQuaternion(targetRot, duration);
    }
    
}
