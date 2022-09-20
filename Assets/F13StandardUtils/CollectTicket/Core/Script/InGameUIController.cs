using System;
using F13StandardUtils.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : Singleton<InGameUIController>
{
    [SerializeField] private Image _levelProgress, _cashImage;
    [SerializeField] private TextMeshProUGUI _level, _money;
    private LevelMoneyController _levelMoneyController;

    public Image CashImage => _cashImage;
    private void OnEnable()
    {
        _levelMoneyController = LevelMoneyController.Instance;
        _levelMoneyController.OnMoneyAdd.AddListener(Init);
        _levelMoneyController.OnMoneySpend.AddListener(Init);
        GameController.Instance.OnLevelInit.AddListener(Init);
        Init(0);
    }

    private void OnDisable()
    {
        _levelMoneyController = LevelMoneyController.Instance;
        _levelMoneyController?.OnMoneyAdd.RemoveListener(Init);
        _levelMoneyController?.OnMoneySpend.RemoveListener(Init);
        GameController.Instance?.OnLevelInit.RemoveListener(Init);
    }

    private void Init(int m)
    {
        _money.text = _levelMoneyController.Money.ToString();
        _level.text = "Level " + (GameController.Instance.PlayerData.Level);
        _levelProgress.fillAmount = _levelMoneyController.LevelCompleteRatio;
    }

}
