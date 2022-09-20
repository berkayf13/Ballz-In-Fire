using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.CollectTicket.Tutorial.Script;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialClick : BaseTutorial
{
    [SerializeField] private Image _mask;
    [SerializeField] private Image _hand;
    [SerializeField] private Collider _disablerCollider;
    [SerializeField] private bool isMask = true;
    [SerializeField] private bool isScaleAnimation;
    [SerializeField] private bool isMoveAnimation;
    [SerializeField,ShowIf(nameof(isMoveAnimation))] private float moveDuration=2f;
    [SerializeField,ShowIf(nameof(isMoveAnimation))] private float moveWaitDuration=0.25f;
    [SerializeField] private Transform startObject;
    [SerializeField,ShowIf(nameof(isMoveAnimation))] private Transform endObject;
    [SerializeField,ReadOnly] private bool finishTutorial;

    private bool isActivated = false;
    private Vector3 StartPos => !startObject.TryGetComponent(out RectTransform rt) && Camera.main
        ? Camera.main.WorldToScreenPoint(startObject.transform.position)
        : startObject.transform.position;

    private Vector3 EndPos => !endObject.TryGetComponent(out RectTransform rt) && Camera.main
        ? Camera.main.WorldToScreenPoint(endObject.transform.position)
        : endObject.transform.position;
    
    
    protected override bool TutorialNotStartCondition()
    {
        return false;
    }

    protected override bool TutorialStartCondition()
    {
        return GameController.Instance.IsPlaying;
    }

    protected override bool TutorialEndCondition()
    {
        return finishTutorial;
    }

    protected override void OnTutorialStart()
    {
        if(!_hand.gameObject.activeInHierarchy) _hand.gameObject.SetActive(true);
        _mask.gameObject.SetActive(isMask);
        _disablerCollider.gameObject.SetActive(true);

        if (isScaleAnimation)
        {
            ScaleAnimation();
        }
        
        if (isMoveAnimation) 
            StartCoroutine(MoveAnimation(ActivateTutorial));
        else
        {
            var startPos = StartPos;
            _hand.transform.position = startPos;
            _mask.transform.position = startPos;
            ActivateTutorial();
        }
    }

    private void ActivateTutorial()
    {
        isActivated = true;
        _disablerCollider.gameObject.SetActive(false);
    }
    
    protected override void OnTutorialEnd()
    {
        
    }


    protected virtual void Awake()
    {
        _mask.gameObject.SetActive(false);
        _hand.gameObject.SetActive(false);
        _disablerCollider.gameObject.SetActive(false);
        var startPos = StartPos;
        _mask.transform.position = startPos;
        _hand.transform.position = startPos;
    }

    public void Update()
    {
        if(!isActivated) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            finishTutorial = true;
        }
    }
    
    private IEnumerator MoveAnimation(Action onMoveEndPosition=null)
    {
        var startPos = StartPos;
        var endPos = EndPos;
        _hand.transform.position = startPos;
        _mask.transform.position = startPos;
        yield return new WaitForSecondsRealtime(moveWaitDuration);
        var startDif = endPos - startPos;

        while (!finishTutorial)
        {
            var currentDifF = endPos - _hand.transform.position;
            if (currentDifF.magnitude < 0.1f)
            {
                onMoveEndPosition?.Invoke();
                yield return new WaitForSecondsRealtime(moveWaitDuration);
                _hand.transform.position = startPos;
                _mask.transform.position = startPos;
                yield return new WaitForSecondsRealtime(moveWaitDuration);
            }
            var translateAmount = startDif / moveDuration * Time.unscaledDeltaTime;
            if (currentDifF.magnitude < translateAmount.magnitude)
                translateAmount = currentDifF;
            _hand.transform.Translate(translateAmount);
            _mask.transform.Translate(translateAmount);
            yield return null;
        }
        
    }
    
    private void ScaleAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Append(_hand.transform.DOScale(1.5f, 0.5f));
        seq.Append(_hand.transform.DOScale(1f, 0.5f));
        seq.SetLoops(-1);
        seq.SetUpdate(true);
    }
}
