using DG.Tweening;
using F13StandardUtils.Crowd.Scripts;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFinish : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected Transform groupTarget;

    protected Sequence finishSeq;
    protected bool isFinishStart=false;
    protected bool isFinishCompleted = false;
    protected UnityEvent onFinishStart = new UnityEvent();
    protected UnityEvent onFinishCompleted=new UnityEvent();
    
    public Transform Platform => _platform;
    public bool IsFinishStart => isFinishStart;
    public bool IsFinishCompleted => isFinishCompleted;
    public UnityEvent OnFinishStart => onFinishStart;
    public UnityEvent OnFinishCompleted => onFinishCompleted;
    
    public abstract int FinishMoney { get; }
    protected abstract void FinishProcess();
    
    protected virtual void OnDestroy()
    {
        finishSeq?.Kill();
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerType.Player.ToString()))
        {
            FinishProcess();
        }
    }
}