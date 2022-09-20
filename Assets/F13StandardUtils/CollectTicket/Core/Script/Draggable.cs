using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
    [SerializeField,ReadOnly] private bool _isDragging;
    [SerializeField] private float offsetY, offsetZ;
    private Vector3 takenPos;

    public bool IsDragging => _isDragging;
    private Vector3 CalculatedOffset => Vector3.up * offsetY + Vector3.forward * offsetZ;

    protected Vector3 lastDragPoint;
    private void OnMouseDown()
    {
        Pick();
    }



    private void OnMouseDrag()
    {
        Drag();
    }
    


    private void OnMouseUp()
    {
        Drop();
    }



    protected virtual void Pick()
    {
        if (!DragController.Instance) return;
        if (!enabled) return;
        takenPos = transform.position;
        _isDragging = true;
        UpdatePosition(Vector3.zero, () => { DragController.Instance.DragStart(this); });
    }
    
    protected virtual void Drag()
    {
        if (!DragController.Instance) return;
        if (!enabled) return;
        if(!_isDragging) return;
        UpdatePosition(CalculatedOffset, () => { DragController.Instance.DragUpdate(this); });
    }
    
    protected virtual void Drop()
    {
        if (!DragController.Instance) return;
        if (!enabled) return;
        _isDragging = false;
        UpdatePosition(Vector3.zero, () => { DragController.Instance.DragEnd(this); });
    }
    
    protected virtual void UpdatePosition(Vector3 offset ,Action onRayHit = null)
    {
        var plane = new Plane(Vector3.up, new Vector3(0, takenPos.y, 0));
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            lastDragPoint = ray.GetPoint(distance) + offset;
            transform.position = lastDragPoint;
            onRayHit?.Invoke();
        }
    }

}