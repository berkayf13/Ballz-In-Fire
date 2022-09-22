using Assets.F13SDK.Scripts;
using F13StandardUtils.CollectTicket.PowerUp.Scripts;
using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateButton : BasePowerUpButton
{
    private PlayerData PlayerData => GameController.Instance.PlayerData;
    protected override float Level 
    {
        get => PlayerData.FireRate;
        set => PlayerData.FireRate = (int)value;
    }

    protected override List<int> LevelCosts()
    {
        return UpgradeCost.FIRERATE_POWER_UP_COSTS;
    }

    protected override string LevelString()
    {
        return "level: " + (PlayerData.FireRate + 1);
    }


}
