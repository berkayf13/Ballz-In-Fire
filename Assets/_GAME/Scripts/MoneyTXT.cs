using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTXT : Singleton<MoneyTXT>
{
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Vector3 incrementTextOfset = new Vector3(5, 0, 0);


    private void Update()
    {
        _moneyText.text = MoneyManager.MONEY_ICON + " " + GameController.Instance.PlayerData.Money;
        
    }



    [Button]
    public void CreateIncrementText(int increment)
    {
        var duration = 1f;
        var incrementText = Instantiate(_tmp, transform).GetComponent<TextMeshProUGUI>();
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
}
