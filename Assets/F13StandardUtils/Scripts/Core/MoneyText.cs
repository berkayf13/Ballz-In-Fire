using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Core;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MoneyText : BaseObjectUpdater<int>
{
    public static MoneyText Instance;
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private Vector3 incrementTextOfset=new Vector3(5,0,0);
    public bool updateUI = true;
    public bool incrementText = true;

    public TextMeshProUGUI TMP => _tmp;

    protected override int Value => updateUI? MoneyManager.Instance.MoneyCount: lastValue;
    protected override void OnValueUpdate()
    {
        _tmp.text = MoneyManager.MONEY_ICON + " " + value.ToString();
        if(incrementText) CreateIncrementText(value - lastValue);
    }

    [Button]
    public void CreateIncrementText(int increment)
    {
        var duration = 1f;
        var incrementText = Instantiate(_tmp,transform).GetComponent<TextMeshProUGUI>();
        incrementText.text = string.Empty;
        incrementText.transform.localPosition = incrementTextOfset;
        incrementText.transform.DOLocalMoveY(10, duration).OnComplete(() => Destroy(incrementText.gameObject));
        incrementText.DOFade(0, duration);
        if (increment < 0)
        {
            incrementText.color = Color.red;
            incrementText.text += increment;
        }
        else
        {
            incrementText.text += "+" + increment;
            incrementText.color = Color.green;
        }

        incrementText.text += " " + MoneyManager.MONEY_ICON;
    }

    protected void Awake()
    {
        Instance = this;
    }
    
}
