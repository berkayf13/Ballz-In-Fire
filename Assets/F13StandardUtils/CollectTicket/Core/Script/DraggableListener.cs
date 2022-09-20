using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DraggableListener : MonoBehaviour
{
    [SerializeField, ReadOnly] private Draggable _draggable;
    [SerializeField] private bool isMoveUp;
    [SerializeField] private float offset = .2f;

    public bool IsDraggableIn => _draggable != null;
    public Draggable Draggable => _draggable;
    private void OnTriggerEnter(Collider other)
    {
        if(!DragController.Instance) return;
        if (other.TryGetComponent(out Draggable d) && d.IsDragging)
        {
            _draggable = d;
            if(isMoveUp) transform.position += Vector3.up * offset;
            DragController.Instance.DraggableListenerEnter(d,this);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(!DragController.Instance) return;
        if (_draggable!=null)
        {
            DragController.Instance.DraggableListenerStay(_draggable,this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!DragController.Instance) return;
        if (other.TryGetComponent(out Draggable d)&& d.IsDragging)
        {
            _draggable = null;
            if(isMoveUp) transform.position -= Vector3.up * offset;
            DragController.Instance.DraggableListenerExit(d,this);
        }
    }
}
