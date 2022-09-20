using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
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
        _animator.Play("Attack");

    }

    public virtual void TriggerAnimation(string triggerKey)
    {

        _animator.SetTrigger(triggerKey);

    }

}
