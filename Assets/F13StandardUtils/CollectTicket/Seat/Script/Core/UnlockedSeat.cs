using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class UnlockedSeat : MonoBehaviour
    {
        public static float DIRTY_CLEAN_DURATION = 2;
        [SerializeField] private List<BaseSeatModifier> _seatModifiers;
        [SerializeField] private Transform _customerHolder;
        [SerializeField] private ShinyObject _shinyObject;
        [SerializeField] private ChronometerGroup _chronometer;
        [SerializeField, ReadOnly] private Customer _customer;
        


        public  Transform CustomerHolder =>_customerHolder;
        public List<BaseSeatModifier> SeatModifiers => _seatModifiers.ToList();
        
        public bool IsFilled => _customer != null;
        
        public bool IsDirty => _chronometer.gameObject.activeInHierarchy;

        public bool IsAvailable => !IsFilled && !IsDirty;

        public Customer Customer => _customer;

        public ShinyObject ShinyObject => _shinyObject;
        
        public bool HasSeat<T>()
        {
            return _seatModifiers.Find(m => typeof(T) == m.GetType())!=null;
        }
        

        public bool DoesFits(params BaseSeatModifier[] modifiers)
        {
            return IsAvailable && modifiers.All(m=>_seatModifiers.Contains(m));
        }

        private void OnEnable()
        {
            DragController.Instance?.OnDropToListener.AddListener(OnDropToListener);
            DragController.Instance?.OnStayToListener.AddListener(OnStayToListener);
            DragController.Instance?.OnExitToListener.AddListener(OnExitToListener);
            TapToTapController.Instance?.OnTapListenerSelect.AddListener(OnSeatTapped);
            _chronometer.CurrentObject.OnComplete.AddListener(OnDirtySeatCleaned);
            
        }




        private void OnDisable()
        {
            DragController.Instance?.OnDropToListener.RemoveListener(OnDropToListener);
            DragController.Instance?.OnStayToListener.RemoveListener(OnStayToListener);
            DragController.Instance?.OnExitToListener.RemoveListener(OnExitToListener);
            TapToTapController.Instance?.OnTapListenerSelect.RemoveListener(OnSeatTapped);
            _chronometer.CurrentObject.OnComplete.RemoveListener(OnDirtySeatCleaned);
        }



        private void OnStayToListener(Draggable d, DraggableListener l)
        {
            if(!l.gameObject.Equals(gameObject)) return;
            if(!DragController.Instance.IsDragging) return;

            if (d.TryGetComponent(out Customer c))
            {
                if (IsAvailable)
                {
                    var doesFits = DoesFits(c.Targets.ToArray());
                    _shinyObject.SetColor(doesFits);
                    _shinyObject.IsShine = DragController.Instance.IsOnListener && DragController.Instance.Listener == l;
                }
            }
        }
        
        private void OnExitToListener(Draggable d, DraggableListener l)
        {
            if(!l.gameObject.Equals(gameObject)) return;

            if (d.TryGetComponent(out Customer c))
            {
                if (_shinyObject.IsShine)
                {
                    _shinyObject.IsShine = false;
                }
            }
        }

        private void OnDropToListener(Draggable d, DraggableListener l)
        {
            if(!l.gameObject.Equals(gameObject)) return;
            if (d.TryGetComponent(out Customer c))
            {
                _shinyObject.IsShine = false;
                if(IsAvailable && DoesFits(c.Targets.ToArray()))
                {
                    _customer = c;
                    CustomerQueueManager.Instance.DequeueCustomer(_customer);
                    _customer.SetDraggableOrTapableActive(false);
                    _customer.TargetInfoSetAppear(false);
                    _customer.ChronometerGroup.CurrentObject.ResetTime();
                    _customer.OnCustomerStandUp.AddListener(OnSitComplete);
                    _customer.StartSitDialog(_customerHolder);
                }
                else
                {
                    c.ResetPosition();
                }
            }

        }

        private void OnSitComplete()
        {
            _customer.ChronometerGroup.SetCurrent(ChronometerType.Progress);
            _customer.ChronometerGroup.CurrentObject.ResetTime();
            _customer.OnCustomerStandUp.RemoveListener(OnSitComplete);
            _customer = null;
        }
        


        private void OnSeatTapped(Tapable t, TapableListener l)
        {
            if(!l.gameObject.Equals(gameObject)) return;

            if (t.TryGetComponent(out Customer c))
            {
                if (IsAvailable && DoesFits(c.Targets.ToArray()))
                {
                   _shinyObject.SetColor(true);
                    _customer = c;
                    CustomerQueueManager.Instance.DequeueCustomer(c);
                    c.SetDraggableOrTapableActive(false);
                    c.TargetInfoSetAppear(false);
                    c.ChronometerGroup.CurrentObject.ResetTime();
                    c.Move(transform.position,12f,() =>
                    {
                        c.OnCustomerStandUp.AddListener(OnSitComplete);
                        c.StartSitDialog(_customerHolder);
                    });
                }
                else
                {
                    _shinyObject.SetColor(false);
                }
                
                _shinyObject.IsShine = true;
                this.StartWaitForSecondCoroutine(0.1f, () =>
                {
                    _shinyObject.IsShine = false;
                });
                
            }
        }
        
        private void OnDirtySeatCleaned(Chronometer c)
        {
            _chronometer.gameObject.SetActive(false);
        }
        
        public void SetDirty()
        {
            _chronometer.gameObject.SetActive(true);
            _chronometer.SetCurrent(ChronometerType.Clean);
            _chronometer.CurrentObject.duration = DIRTY_CLEAN_DURATION;
            _chronometer.CurrentObject.ResetTime();
        }

        
    }
}
