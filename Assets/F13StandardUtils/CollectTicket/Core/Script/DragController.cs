using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class DragListenerEvent : UnityEvent<Draggable,DraggableListener> {}
[System.Serializable] public class DragEvent : UnityEvent<Draggable> {}
public class DragController : Singleton<DragController>
{
    public DragEvent OnDragStart = new DragEvent();
    public DragEvent OnDragging = new DragEvent();
    public DragEvent OnDragEnd = new DragEvent();
    public DragEvent OnDropFailed = new DragEvent();
    public DragListenerEvent OnDropToListener = new DragListenerEvent();
    public DragListenerEvent OnEnterToListener = new DragListenerEvent();
    public DragListenerEvent OnStayToListener = new DragListenerEvent();
    public DragListenerEvent OnExitToListener = new DragListenerEvent();


    [SerializeField, ReadOnly] private Draggable _draggable;
    [SerializeField, ReadOnly] private Draggable _lastDraggable;
    [SerializeField, ReadOnly] private List<DraggableListener> _listeners=new List<DraggableListener>();
    [SerializeField, ReadOnly] private bool _isDragging;

    public Draggable Draggable => _draggable;
    public Draggable LastDraggable => _lastDraggable;

    public DraggableListener Listener
    {
        get
        {
            var a=_listeners.MinItem(l =>
            {
                if(_draggable==null)
                    throw new Exception("Draggable is null");
                if(l==null)
                    throw new Exception("Listener is null");
                var distance =(_draggable.transform.position - l.transform.position).magnitude;
                return distance;
            });
            return a;
        }
    }


    public bool IsOnListener => _listeners.Any();
    public bool IsDragging => _isDragging;
    public void DragStart(Draggable d)
    {
        _draggable = d;
        _isDragging = true;
        OnDragStart.Invoke(d);
    }

    public void DragUpdate(Draggable d)
    {
        OnDragging.Invoke(d);
    }

    public void DragEnd(Draggable d)
    {

        if (IsOnListener)
        {
            OnDropToListener.Invoke(d,Listener);
            _listeners.Clear();
        }
        else
        {
            OnDropFailed.Invoke(d);
        }
        _draggable = null;
        _lastDraggable = d;
        _isDragging = false;
        OnDragEnd.Invoke(d);

        
    }

    public void DraggableListenerEnter(Draggable d, DraggableListener l)
    {
        if(!_listeners.Contains(l))_listeners.Add(l);
        OnEnterToListener.Invoke(d,l);
    }
    public void DraggableListenerStay(Draggable d, DraggableListener l)
    {
        if(_isDragging)
            OnStayToListener.Invoke(d,l);
    }
    
    public void DraggableListenerExit(Draggable d, DraggableListener l)
    {
        if(_listeners.Contains(l))_listeners.Remove(l);
        OnExitToListener.Invoke(d,l);
    }
}
