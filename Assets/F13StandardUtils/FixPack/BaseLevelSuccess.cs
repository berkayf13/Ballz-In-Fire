using F13StandardUtils.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLevelSuccess : BaseObjectUpdater<bool>
{
    [SerializeField,Range(0f,1f)] protected float successRatio = 1f;
    [SerializeField] protected  int maxStar = 3;
    public UnityEvent OnSuccess=new UnityEvent();

    public float SuccessRatio
    {
        get => successRatio;
        set => successRatio = value;
    }

    protected override void OnValueUpdate()
    {
        if (Value)
        {
            Success();
        }
    }
    
    public void Success()
    {
        StarUnlock.SCORE = (int)(successRatio*maxStar+0.5f);
        StarUnlock.MAXSCORE = maxStar;
        OnSuccess.Invoke();
        this.StartWaitForSecondCoroutine(1f, GameController.Instance.SuccessLevel);

    }
}