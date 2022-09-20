using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BasicAnimatorStateChanger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField,ValueDropdown(nameof(Parameters))] private string _parameter;
    [SerializeField, Range(0f, 1f)] protected float midAnimNormalizedTime = 0.5f;

    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnMid = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();

    public AnimationClip PlayingAnimation
    {
        get
        {
            var clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
            if(clipInfos.Any()) return clipInfos[0].clip;
            return null;
        }
    }

    private void Reset()
    {
        TryGetComponent(out _animator);
    }

    private string[] Parameters() => _animator?_animator.parameters.Select(p => p.name).ToArray():new string[0];

    [SerializeField,ReadOnly] private AnimationClip _lastAnimation;
    private IEnumerator ActionCoroutine()
    {
        yield return new WaitUntil(()=>
        {
            if (_lastAnimation == null && PlayingAnimation != null) return true;
            if (_lastAnimation != null && !_lastAnimation.Equals(PlayingAnimation)) return true;
            return false;
        });
        OnStart.Invoke();
        Debug.Log("BasicAnimatorStateChanger.OnStart",gameObject);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= midAnimNormalizedTime);
        OnMid.Invoke();
        Debug.Log("BasicAnimatorStateChanger.OnMid",gameObject);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        OnEnd.Invoke();
        Debug.Log("BasicAnimatorStateChanger.OnEnd",gameObject);

    }


    [Button]
    public void SetTrigger()
    {
        _lastAnimation = PlayingAnimation;
        _animator.SetTrigger(_parameter);
        StartCoroutine(ActionCoroutine());
    }

    [Button]
    public void SetFloat(float value)
    {
        _lastAnimation = PlayingAnimation;
        _animator.SetFloat(_parameter, value);
        StartCoroutine(ActionCoroutine());
    }

    [Button]
    public void SetInteger(int value)
    {
        _lastAnimation = PlayingAnimation;
        _animator.SetInteger(_parameter, value);
        StartCoroutine(ActionCoroutine());
    }

    [Button]
    public void SetBool(bool value)
    {
        _lastAnimation = PlayingAnimation;
        _animator.SetBool(_parameter, value);
        StartCoroutine(ActionCoroutine());
    }

    [Button]
    public void SetSpeed(float speed)
    {
        _animator.speed = speed;
    }
}
