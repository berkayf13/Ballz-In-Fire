using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : Singleton<PlayerAnimController>
{
    public const string ANIM_IDLE = "isIdle";
    public const string ANIM_ATTACK = "isAttack";
    public const string ANIM_RUN = "isRun";
    public const string ANIM_DEATH = "isDeath";

    public bool isMove = false;
    private bool lastIsMoving = true;
    private bool isDeath = false;

    [SerializeField] protected Animator _animator;
    [SerializeField] protected List<Animator> _secondaryAnimatorList = new List<Animator>();
    [SerializeField, Range(0f, 1f)] protected float attackAnimActionNormalizedTime = 0.5f;

    string curentAnimState = string.Empty;
    public string CurentAnimState => curentAnimState;

    private void LateUpdate()
    {
        if (MoveZ.Instance )
        {
            isMove = MoveZ.Instance.isMove;
        }
        if (lastIsMoving != isMove)
        {
            UpdateAnim();
        }
    }

    private void UpdateAnim()
    {
        lastIsMoving = isMove;
        if (isMove)
            Run();
        else
            Idle();
    }


    [Button]
    public void Idle()
    {
        TriggerAnimation(ANIM_IDLE);
    }

    [Button]
    public void Run()
    {
        TriggerAnimation(ANIM_RUN);

    }

    public void Death()
    {
        TriggerAnimation(ANIM_DEATH);
    }

    [Button]
    public void Attack()
    {
        //_animator.Play("Attack");
        TriggerAnimation(ANIM_ATTACK/*, () => PlayerFire.Instance.CreateBall()*/);

    }

    public virtual void TriggerAnimation(string triggerKey, Action onMidAction = null, Action onEndAction = null)
    {

        if (CurentAnimState.Equals(triggerKey)) return;

        _animator.SetTrigger(triggerKey);
        if (onMidAction != null || onEndAction != null)
            StartCoroutine(ActionCoroutine(attackAnimActionNormalizedTime, onMidAction, onEndAction));
        foreach (var a in _secondaryAnimatorList)
        {
            a.SetTrigger(triggerKey);
        }
        curentAnimState = triggerKey;

    }

    private IEnumerator ActionCoroutine(float midActionNormalizedTime, Action onMidAction = null, Action onEndAction = null)
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= midActionNormalizedTime);
        onMidAction?.Invoke();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        onEndAction?.Invoke();
    }


}
