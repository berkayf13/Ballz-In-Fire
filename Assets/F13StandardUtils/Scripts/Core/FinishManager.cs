using System;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

public enum ABType
{
    A,
    B
}

public class FinishManager : Singleton<FinishManager>
{
    public static ABType FINISH_TYPE = ABType.B;

    public static float TOTAL_DURATION=4.5f;
    public static int ANIM_MAX_BALE_COUNT=100;
    public static float ANIM_BALE_MULTIPLIER=1f;
    public static float UI_ANIM_MAX_AREA_WIDTH=200f;
    public static float UI_ANIM_MAX_PARTICLE_ROTATE=15f;
    public static int UI_ANIM_MIN_RANDOM=10;
    public static int UI_ANIM_MAX_RANDOM=20;
    public static float UI_ANIM_COUNT_MULTIPLIER=0.2f;
    
    public static float MINI_DURATION => TOTAL_DURATION /6;
    [SerializeField] private List<BaseFinish> _finishList = new List<BaseFinish>();

    public BaseFinish CurrentFinish => _finishList[(int) FINISH_TYPE];
    public Transform Platform => CurrentFinish.Platform;
    public int FinishMoney => CurrentFinish.FinishMoney;
    public bool IsFinishStart => CurrentFinish.IsFinishStart;
    public bool IsFinishCompleted => CurrentFinish.IsFinishCompleted;
    public UnityEvent OnFinishStart=new UnityEvent();
    public UnityEvent OnFinishComplete=new UnityEvent();
    public UnityEvent OnMoneyEarn=new UnityEvent();

    private void Start()
    {
        UpdateFinish();
    }

    private void OnEnable()
    {
        GameController.Instance?.OnGameplayEnter.AddListener(OnGameplayEnter);
    }

    private void OnDisable()
    {
        GameController.Instance?.OnGameplayEnter.RemoveListener(OnGameplayEnter);
        
    }
    
    private void OnGameplayEnter()
    {
        StartSwerveActive();
    }
    
    private void StartSwerveActive()
    {
        SwerveController.Instance.canSwerve = true;
    }

    private void UpdateFinish()
    {
        for (var i = 0; i < _finishList.Count; i++)
        {
            var finish = _finishList[i];
            finish.Platform.gameObject.SetActive( i == (int)FINISH_TYPE);
            finish.OnFinishStart.AddListener(OnFinishStarted);
            finish.OnFinishCompleted.AddListener(OnFinishCompleted);

        }
    }
    
    private void OnFinishStarted()
    {
        OnFinishStart.Invoke();
    }
    
    private void OnFinishCompleted()
    {
        OnFinishComplete.Invoke();
    }

    public void CreateUICollectAnimation(Vector3 pos)
    {
        var earnedMoney = FinishMoney;
        var text = MoneyText.Instance.TMP;
        var particleCount = UnityEngine.Random.Range(UI_ANIM_MIN_RANDOM, UI_ANIM_MAX_RANDOM) + (int) (earnedMoney * UI_ANIM_COUNT_MULTIPLIER);
        var from = Camera.main.WorldToScreenPoint(pos + Vector3.one / 2f);
        var currentMoney = MoneyManager.Instance.MoneyCount;

        AnimationManager.Instance.UICollectAnimation(AnimationManager.Instance.uiPrefabs[0],particleCount,from,text.transform.position,text.transform.parent,
            UI_ANIM_MAX_AREA_WIDTH,UI_ANIM_MAX_PARTICLE_ROTATE, () =>
            {
                text.color=Color.yellow;
                text.transform.DOScale(1.3f, 0.1f).OnComplete(() =>
                {
                    text.color=Color.white;
                    text.transform.DOScale(1, 0.1f);
                });
                MoneyManager.Instance.IncrementMoney(earnedMoney / particleCount,false);
            }, () =>
            {
                MoneyManager.Instance.SetMoneyCount(currentMoney + earnedMoney);
                OnMoneyEarn.Invoke();
                GameController.Instance.SuccessLevel();
            });
            
        }



}
