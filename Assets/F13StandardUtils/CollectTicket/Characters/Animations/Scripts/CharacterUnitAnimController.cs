using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class CharacterUnitAnimController : MonoBehaviour
    {
        public const string ANIM_IDLE = "isIdle";
        public const string ANIM_RUN = "isRun";
        public const string ANIM_GETUPSET = "isGetUpset";
        public const string ANIM_SITTING_GET_UPSET = "isSittingGetUpset";
        public const string ANIM_SITTING = "isSitting";
        public const string ANIM_SITTING_VICTORY = "isSittingVictory";
        public const string ANIM_LIE_DOWN = "isLieDown";
        public const string ANIM_CATWALK = "isCatWalk";
        public const string ANIM_SITTING_HAND_RAISING = "isSittingHandRaising";
        public const string ANIM_CARRYING_BAG_WALK = "isRun";
        public const string ANIM_VICTORY = "isVictory";
        public const string ANIM_HAND_RAISING = "isHandRaising";


        [SerializeField] protected Animator _animator;
        [SerializeField] protected List<Animator> _secondaryAnimatorList = new List<Animator>();
        [SerializeField, Range(0f, 1f)] protected float attackAnimActionNormalizedTime = 0.5f;

        protected string curentAnimState = string.Empty;

        public string CurentAnimState => curentAnimState;


        public void SetMirror(bool isMirror)
        {
            var scale = transform.localScale;
            scale.x *= isMirror ? -1 : 1;
            transform.localScale = scale;
        }

        [Button]
        public virtual void Idle()
        {
            TriggerAnimation(ANIM_IDLE);
        }

        [Button]
        public virtual void Run()
        {
            TriggerAnimation(ANIM_RUN);

        }
        [Button]
        public virtual void GetUpset()
        {
            TriggerAnimation(ANIM_GETUPSET);

        }
        [Button]
        public virtual void SittingGetUpset()
        {
            TriggerAnimation(ANIM_SITTING_GET_UPSET);

        }
        [Button]
        public virtual void Sitting()
        {
            TriggerAnimation(ANIM_SITTING);

        }
        [Button]
        public virtual void SittingVictory()
        {
            TriggerAnimation(ANIM_SITTING_VICTORY);

        }
        [Button]
        public virtual void LieDown()
        {
            TriggerAnimation(ANIM_LIE_DOWN);

        }
        [Button]
        public virtual void CatWalk()
        {
            TriggerAnimation(ANIM_CATWALK);

        }
        [Button]
        public virtual void SittingHandRaising()
        {
            TriggerAnimation(ANIM_SITTING_HAND_RAISING);

        }
        [Button]
        public virtual void HoldingBagWalk()
        {
            TriggerAnimation(ANIM_CARRYING_BAG_WALK);

        }
        [Button]
        public virtual void Victory()
        {
            TriggerAnimation(ANIM_VICTORY);
        }
        [Button]
        public virtual void HandRaising()
        {
            TriggerAnimation(ANIM_HAND_RAISING);
        }
        public virtual void PauseAnimator()
        {
            SetSpeed(0);
        }

        public virtual void ResumeAnimator()
        {
            SetSpeed(1);
        }

        public virtual void SetSpeed(float x)
        {
            _animator.speed = x;
            foreach (var a in _secondaryAnimatorList)
            {
                a.speed = x;
            }
        }

        private IEnumerator ActionCoroutine(float midActionNormalizedTime, Action onMidAction = null, Action onEndAction = null)
        {
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= midActionNormalizedTime);
            onMidAction?.Invoke();
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            onEndAction?.Invoke();
        }

        [Button]
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

        [Button]
        public virtual void PlayAnimationWithName(string animName, float normalizedTime, bool pauseAnimator = false)
        {
            _animator.Play(animName, 0, normalizedTime);
            foreach (var a in _secondaryAnimatorList)
            {
                a.Play(animName, 0, normalizedTime);
            }
            curentAnimState = animName;
            SetSpeed(pauseAnimator ? 0 : 1);
        }
    }
}