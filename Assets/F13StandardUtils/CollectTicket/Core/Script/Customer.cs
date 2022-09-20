using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.CollectTicket.Seat.Script.Core;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public static float SPECIAL_ORDER_RATE = 0f;
    public static int CUSTOMER_MIN_MONEY = 33;
    public static int CUSTOMER_MAX_MONEY = 66;
    public static int CUSTOMER_SPECIAL_ORDER_MONEY = 5;
    public static float CUSTOMER_WAIT_DURATION = 15;
    public static float CUSTOMER_WAIT_SPECIAL_ORDER_DURATION = 20;
    public static float CUSTOMER_SIT_DURATION = 20;


    [SerializeField,ReadOnly] private List<BaseSeatModifier> _targets=new List<BaseSeatModifier>();
    [SerializeField,ReadOnly]private bool _wantSpecialOrder;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Draggable _draggable;
    [SerializeField] private Tapable _tapable;
    [SerializeField] private ChronometerGroup _chronometerGroup;
    [SerializeField] private TargetInfoGroup _targetInfoGroup;
    [SerializeField] private CustomerGroup _customerGroup;
    [SerializeField] private GameObject _cashPrefab;

    private Vector3 lastPosition;
    public bool haveSpecialOrder;
    public UnityEvent OnCustomerStandUp=new UnityEvent();
    private int _randomMoney;

    public int EarnedMoney
    {
        get
        {
            var money = _randomMoney;
            if (_wantSpecialOrder) money += CUSTOMER_SPECIAL_ORDER_MONEY;
            return money;
        }
    }

    public bool WantSpecialOrder => _wantSpecialOrder;
    
    public List<BaseSeatModifier> Targets => _targets.ToList();
    public ChronometerGroup ChronometerGroup => _chronometerGroup;

    public Draggable Draggable => _draggable;
    public Tapable Tapable => _tapable;

    public CustomerGroup CustomerGroup => _customerGroup;


    public bool DraggableOrTapableActive => _draggable && _draggable.enabled || _tapable && _tapable.enabled;
    public bool IsDragging => _draggable && _draggable.IsDragging;
    public bool IsSelected => _tapable && _tapable.IsSelected;

    public bool IsDraggingOrSelected => IsDragging || IsSelected;
    
    public bool IsLastDragged => DragController.Instance && DragController.Instance.LastDraggable && 
                                 _draggable.Equals(DragController.Instance.LastDraggable);

    public bool IsLastSelected => TapToTapController.Instance && TapToTapController.Instance.LastTapable &&
                                  _tapable.Equals(TapToTapController.Instance.LastTapable);

    private void OnEnable()
    {
        DragController.Instance?.OnDragStart?.AddListener(OnDragStart);
        DragController.Instance?.OnDragEnd?.AddListener(OnDragEnd);
        DragController.Instance?.OnDropFailed?.AddListener(OnDropFailed);
        TapToTapController.Instance?.OnTapableSelect.AddListener(OnTapableSelect);
        TapToTapController.Instance?.OnTapableCancel.AddListener(OnTapableCancel);
        _customerGroup.Random();
        FillTargets();
    }



    private void OnDisable()
    {
        DragController.Instance?.OnDragStart?.RemoveListener(OnDragStart);
        DragController.Instance?.OnDragEnd?.RemoveListener(OnDragEnd);
        DragController.Instance?.OnDropFailed.RemoveListener(OnDropFailed);
        TapToTapController.Instance?.OnTapableSelect.RemoveListener(OnTapableSelect);
        TapToTapController.Instance?.OnTapableCancel.RemoveListener(OnTapableCancel);
    }

    private void FillTargets()
    {
        _targets.Clear();
        _targets = RandomSeatRequestManager.Instance.Random();
        _wantSpecialOrder = Utils.RandomBool(SPECIAL_ORDER_RATE);
        _randomMoney = Random.Range(CUSTOMER_MIN_MONEY, CUSTOMER_MAX_MONEY);
        _targetInfoGroup.SetAppear(_targets,_wantSpecialOrder,EarnedMoney);

    }
    
    public void SetDraggableOrTapableActive(bool state)
    {
        if (_draggable) _draggable.enabled = state;
        if (_tapable) _tapable.enabled = state;
    }
    
    public void ResetPosition()
    {
        transform.position = lastPosition;
    }

    public void Move(Vector3 dest,float speed,Action onFinhish=null)
    {
        lastPosition = dest;
        if(IsDragging) return;
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(lastPosition);
        _customerGroup.CurrentObject.Run();
        this.StartWaitUntilCoroutine(() => (_navMeshAgent.transform.position-lastPosition).magnitude<1f && _navMeshAgent.velocity==Vector3.zero, () =>
        {
            _navMeshAgent.isStopped = true;
            _customerGroup.CurrentObject.Idle();
            onFinhish?.Invoke();
        });
    }
    
    public void StartWaitProgress()
    {
        _chronometerGroup.SetCurrent(ChronometerType.Progress);
        _chronometerGroup.CurrentObject.duration = CUSTOMER_WAIT_DURATION;
        _chronometerGroup.CurrentObject.ResetAndPlay();
        _chronometerGroup.CurrentObject.OnUpdate.AddListener(OnWaitProgressUpdate);
        _chronometerGroup.CurrentObject.OnPauseUpdate.AddListener(OnWaitProgressPauseUpdate);
        _chronometerGroup.CurrentObject.OnComplete.AddListener(OnWaitProgressComplete);
    }

    public void StartSitDialog(Transform holder) => StartSitDialog(holder.position, holder.rotation);
    public void StartSitDialog(Vector3 pos, Quaternion rot)
    {
        _navMeshAgent.enabled = false;
        lastPosition = pos;
        transform.position = pos;
        transform.rotation = rot;
        OnSit();
    }

    public void TargetInfoSetAppear(bool state)
    {
        _targetInfoGroup.gameObject.SetActive(state);
    }
    

    private void OnSit()
    {
        if (_wantSpecialOrder)
        {
            OnWaitSpecialOrder();
        }
        else
        {
            OnSitDialog();
        }
    }

    private void OnWaitSpecialOrder()
    {
        _customerGroup.CurrentObject.SittingHandRaising();
        _chronometerGroup.SetCurrent(ChronometerType.Order);
        _chronometerGroup.CurrentObject.duration = CUSTOMER_WAIT_SPECIAL_ORDER_DURATION;
        _chronometerGroup.CurrentObject.ResetAndPlay();
        _chronometerGroup.CurrentObject.OnUpdate.AddListener(OnWaitSpecialOrderUpdate);
        _chronometerGroup.CurrentObject.OnComplete.AddListener(OnWaitSpecialOrderComplete);
    }

    private void OnWaitSpecialOrderComplete(Chronometer c)
    {
        OnCustomerGoAway();
    }

    private void OnWaitSpecialOrderUpdate(Chronometer c)
    {
        if (haveSpecialOrder)
        {
            _chronometerGroup.CurrentObject.OnUpdate.RemoveListener(OnWaitSpecialOrderUpdate);
            _chronometerGroup.CurrentObject.Pause();
            OnSitDialog();
        }
        else
        {
            if(_chronometerGroup.CurrentObject.CompleteRatio>0.5f) _customerGroup.CurrentObject.SittingGetUpset();
        }
    }

    private void OnCustomerGoAway()
    {
        _navMeshAgent.enabled = true;
        transform.rotation = Quaternion.identity;
        OnCustomerStandUp?.Invoke();
        CustomerQueueManager.Instance.GoAway(this,true, (c) =>
        {
            SetDraggableOrTapableActive(true);
        });
    }
    

    private void OnSitDialog()
    {
        _customerGroup.CurrentObject.Sitting();
        _chronometerGroup.SetCurrent(ChronometerType.Dialog);
        _chronometerGroup.CurrentObject.duration = CUSTOMER_SIT_DURATION;
        _chronometerGroup.CurrentObject.ResetAndPlay();
        _chronometerGroup.CurrentObject.OnComplete.AddListener(OnSitDialogComplete);
        _chronometerGroup.CurrentObject.OnUpdate.AddListener(OnSitDialogUpdate);
    }

    private void OnSitDialogUpdate(Chronometer chronometer)
    {
        if(_chronometerGroup.CurrentObject.CompleteRatio>0.5f ) _customerGroup.CurrentObject.SittingVictory();
    }


    private void OnSitDialogComplete(Chronometer chronometer)
    {        
        var cash=Instantiate(_cashPrefab, transform.position+Vector3.up*1.5f, Quaternion.identity,transform.parent)
            .GetComponent<CashObject>();
        cash.SetMoney(EarnedMoney);
        _chronometerGroup.CurrentObject.OnComplete.RemoveListener(OnSitDialogComplete);
        OnCustomerGoAway();

    }

    private void OnWaitProgressUpdate(Chronometer chronometer)
    {
        if(!DraggableOrTapableActive) return;
        if(IsDraggingOrSelected) _chronometerGroup.CurrentObject.Pause();
        if(_chronometerGroup.CurrentObject.CompleteRatio>0.55f) _customerGroup.CurrentObject.GetUpset();
        else if(_chronometerGroup.CurrentObject.CompleteRatio>0.33f) _customerGroup.CurrentObject.HandRaising();

    }
    private void OnWaitProgressPauseUpdate(Chronometer chronometer)
    {
        if(!DraggableOrTapableActive) return;
        if((IsLastDragged && !IsDragging) || (IsLastSelected && !IsSelected)) _chronometerGroup.CurrentObject.Resume();
    }
    private void OnWaitProgressComplete(Chronometer chronometer)
    {
        SetDraggableOrTapableActive(false);
        if(TapToTapController.Instance) TapToTapController.Instance.CancelSelect();
        _chronometerGroup.CurrentObject.OnUpdate.RemoveListener(OnWaitProgressUpdate);
        _chronometerGroup.CurrentObject.OnPauseUpdate.RemoveListener(OnWaitProgressPauseUpdate);
        _chronometerGroup.CurrentObject.OnComplete.RemoveListener(OnWaitProgressComplete);
        CustomerQueueManager.Instance.DequeueAndGoAway(this,true, (c) => { SetDraggableOrTapableActive(true); });
    }
    

    private void OnDragEnd(Draggable d)
    {
        if (_draggable.Equals(d))
        {
            _navMeshAgent.enabled = true;
        }
    }

    private void OnDragStart(Draggable d)
    {
        if (_draggable.Equals(d))
            _navMeshAgent.enabled = false;
    }
    
    private void OnDropFailed(Draggable d)
    {
        if(_draggable.Equals(d))
            ResetPosition();
    }
    
    private void OnTapableCancel(Tapable t)
    {
        if (_tapable.Equals(t))
        {
            _customerGroup.Outline.enabled = false;
        }
    }

    private void OnTapableSelect(Tapable t)
    {
        if (_tapable.Equals(t))
        {
            _customerGroup.Outline.enabled = true;
        }
    }


}
