using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.CollectTicket.Seat.Script.Core;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpecialOrder : MonoBehaviour
{
    public static float DIRTY_RATE = 0.33f;
    [SerializeField] private GameObject _model;
    [SerializeField] private Draggable _draggable;
    [SerializeField] private Tapable _tapable;
    [SerializeField] private Collider _collider;
    [SerializeField] private QuickOutline _outline;
    [SerializeField, ReadOnly] private bool _isUsed;
    [SerializeField,ReadOnly] private UnlockedSeat _seat;
    [SerializeField,ReadOnly] private Customer _customer;

    public bool IsUsed => _isUsed;

    private void OnEnable()
    {
        DragController.Instance?.OnDragStart.AddListener(OnDragStart);
        DragController.Instance?.OnDropToListener.AddListener(OnDropToListener);
        DragController.Instance?.OnDropFailed.AddListener(OnDropFailed);
        DragController.Instance?.OnEnterToListener.AddListener(OnEnterToListener);
        DragController.Instance?.OnExitToListener.AddListener(OnExitToListener);
        TapToTapController.Instance?.OnTapableSelect.AddListener(OnTapableSelect);
        TapToTapController.Instance?.OnTapableCancel.AddListener(OnTapableCancel);
        TapToTapController.Instance?.OnTapListenerSelect.AddListener(OnSeatTapped);
            
    }

    private void OnDisable()
    {
        DragController.Instance?.OnDragStart.RemoveListener(OnDragStart);
        DragController.Instance?.OnDropToListener.RemoveListener(OnDropToListener);
        DragController.Instance?.OnDropFailed.RemoveListener(OnDropFailed);
        DragController.Instance?.OnEnterToListener.RemoveListener(OnEnterToListener);
        DragController.Instance?.OnExitToListener.RemoveListener(OnExitToListener);
        TapToTapController.Instance?.OnTapableSelect.RemoveListener(OnTapableSelect);
        TapToTapController.Instance?.OnTapableCancel.AddListener(OnTapableCancel);
        TapToTapController.Instance?.OnTapListenerSelect.RemoveListener(OnSeatTapped);
    }

    private void OnDragStart(Draggable arg0)
    {
        _isUsed = true;
    }

    private void OnExitToListener(Draggable d, DraggableListener l)
    {
        if(!d.gameObject.Equals(gameObject)) return;

        if (l.TryGetComponent(out UnlockedSeat s))
        {
            if(s.IsFilled && s.Customer.WantSpecialOrder && !s.Customer.haveSpecialOrder)
                s.ShinyObject.IsShine = false;
        }
    }

    private void OnEnterToListener(Draggable d, DraggableListener l)
    {
        if(!d.gameObject.Equals(gameObject)) return;

        if (l.TryGetComponent(out UnlockedSeat s))
        {
            if(s.IsFilled && s.Customer.WantSpecialOrder && !s.Customer.haveSpecialOrder)
                s.ShinyObject.IsShine = true;
        }
    }

    private void OnDropToListener(Draggable d, DraggableListener l)
    {
        if(!d.gameObject.Equals(gameObject)) return;

        if (l.TryGetComponent(out UnlockedSeat s))
        {
            if (s.IsFilled && s.Customer.WantSpecialOrder && !s.Customer.haveSpecialOrder)
            {
                _seat = s;
                GiveSpecialOrder();
            }
            else
            {
                DropFailed();
            }
        }
    }

    private void GiveSpecialOrder()
    {
        _isUsed = true;
        SetDraggableOrTapableActive(false);
        _seat.ShinyObject.IsShine = false;
        _customer = _seat.Customer;
        _customer.haveSpecialOrder = true;
        _customer.OnCustomerStandUp.AddListener(OnSitComplete);
        var parent = _seat.Customer.CustomerGroup.Holder;
        transform.SetParent(parent);
        transform.localPosition=Vector3.zero;
        transform.localRotation=Quaternion.identity;

    }

    private void OnSitComplete()
    {
        _customer.OnCustomerStandUp.RemoveListener(OnSitComplete);
        var willDirty = Utils.RandomBool(DIRTY_RATE);
        if(willDirty) _seat.SetDirty();
        _seat = null;
        _customer = null;
        Destroy(gameObject);
    }

    private void OnDropFailed(Draggable d)
    {
        if(!d.gameObject.Equals(gameObject)) return;
        DropFailed();
    }

    private void DropFailed()
    {
        Destroy(gameObject);
    }

    private void OnTapableSelect(Tapable t)
    {
        if (_tapable.Equals(t))
        {
            _outline.enabled = true;
        }
    }
    
    private void OnTapableCancel(Tapable t)
    {
        if (_tapable.Equals(t))
        {
            _outline.enabled = false;
        }
    }
    
    private void OnSeatTapped(Tapable t, TapableListener l)
    {
        if(!t.gameObject.Equals(gameObject)) return;

        if (l.TryGetComponent(out UnlockedSeat s))
        {
            if (s.IsFilled && s.Customer.WantSpecialOrder && !s.Customer.haveSpecialOrder)
            {
                _seat = s;
                GiveSpecialOrder();
            }
            else
            {
                DropFailed();
            }
        }
    }
    
    private void SetDraggableOrTapableActive(bool enabled)
    {
        if (_draggable) _draggable.enabled = enabled;
        if (_tapable) _tapable.enabled = enabled;
    }

    public void SpawnAnimation()
    {
        _model.transform.DOScale(0,0);
        _model.transform.DOScale(1,0.2f);
    }
}
