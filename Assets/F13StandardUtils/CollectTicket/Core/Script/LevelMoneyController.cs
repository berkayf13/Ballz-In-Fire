using System;
using System.Collections.Generic;
using Assets.F13SDK.Scripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LevelMoneyEvent : UnityEvent<int>
{
}

public class LevelMoneyController : Singleton<LevelMoneyController>
{
    public const string MONEY_ICON = "$";
    private PlayerData PlayerData => GameController.Instance.PlayerData;

    public static List<int> LEVEL_COMPLETE_MONEYS = new List<int>
    {
        12000, 13000, 16000, 20000, 25000, 100000
    };

    public LevelMoneyEvent OnLevelMoneyCompleted = new LevelMoneyEvent();
    public LevelMoneyEvent OnMoneyAdd = new LevelMoneyEvent();
    public LevelMoneyEvent OnMoneySpend = new LevelMoneyEvent();
    public LevelMoneyEvent OnOutOfMoney = new LevelMoneyEvent();
    public LevelMoneyEvent OnMoneyChange = new LevelMoneyEvent();
    public LevelMoneyEvent OnLevelMoneyChange = new LevelMoneyEvent();

    [SerializeField, ReadOnly] private int _money, _levelMoney, _lastIncrease, _lastSpend;

    public int Money => _money;
    public int LevelMoney => _levelMoney;
    public int LevelCompleteMoney => LEVEL_COMPLETE_MONEYS[PlayerData.Level-1];

    public float LevelCompleteRatio => (float) _levelMoney / LevelCompleteMoney;

    public int LastIncrease => _lastIncrease;
    public int LastSpend => _lastSpend;

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        _money = PlayerData.Money;
        _levelMoney = PlayerData.LevelMoney;
    }

    [Button]
    public void AddMoney(int add)
    {
        _lastIncrease = add;
        _money += _lastIncrease;
        PlayerData.Money = _money;
        _levelMoney += _lastIncrease;
        PlayerData.LevelMoney = _levelMoney;
        OnMoneyAdd.Invoke(_lastIncrease);
        OnLevelMoneyChange.Invoke(_levelMoney);
        OnMoneyChange.Invoke(_money);
        if (_levelMoney >= LevelCompleteMoney)
        {
            OnLevelMoneyCompleted.Invoke(_levelMoney);
            GameController.Instance.SuccessLevel();
            PlayerData.Money = 0;
            _money = 0;
            PlayerData.LevelMoney = 0;
            _levelMoney = 0;
        }
    }

    [Button]
    public void SpendMoney(int spend)
    {
        if (_money - spend < 0)
        {
            OnOutOfMoney.Invoke(spend);
            return;
        }

        _lastSpend = spend;
        _money -= _lastSpend;
        PlayerData.Money = _money;
        OnMoneySpend.Invoke(_lastSpend);
        OnMoneyChange.Invoke(_money);
    }
}