using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class CashObject : MonoBehaviour
{
    private int MinMoney=>Customer.CUSTOMER_MIN_MONEY;
    private int MaxMoney=>Customer.CUSTOMER_MAX_MONEY;
    [SerializeField] private float _waitTime=1;
    [SerializeField] private List<GameObject> _moneyObjects=new List<GameObject>();
    [SerializeField,OnValueChanged(nameof(UpdateMoney))] private int _money=17;

    
    private Coroutine currentCoroutine;

    private void OnEnable()
    {
        currentCoroutine = StartCoroutine(MoveWait());
    }
    
    public void SetMoney(int money)
    {
        _money = money;
        UpdateMoney();
    }
    private void UpdateMoney()
    {
        var clampedMoney = Mathf.Clamp(_money, MinMoney, MaxMoney);
        var ratio = Mathf.InverseLerp(MinMoney, MaxMoney, clampedMoney);
        var index = ratio * _moneyObjects.Count;
        index = Mathf.Clamp(index, 0, _moneyObjects.Count - 1);
        for (var i = 0; i < _moneyObjects.Count; i++)
        {
            _moneyObjects[i].SetActive(i<=index);
        }
    }

    private void OnMouseDown()
    {
        StopCoroutine(currentCoroutine);
        MoveToUI();
    }

    IEnumerator MoveWait()
    {
        yield return new WaitForSeconds(_waitTime);
        MoveToUI();
    }

    private void MoveToUI()
    {
        Vector3 targetPosUI = InGameUIController.Instance.CashImage.transform.position + Vector3.forward*10;
        Vector3 targetPosWorld = Camera.main.ScreenToWorldPoint(targetPosUI);
        transform.DOMove(targetPosWorld, 1).SetEase(Ease.InOutCirc).OnComplete((() =>
        {
            transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutExpo);
            LevelMoneyController.Instance.AddMoney(_money);
        }));
    }
}
