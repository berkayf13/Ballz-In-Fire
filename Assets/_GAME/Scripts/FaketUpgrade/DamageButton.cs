using Assets.F13SDK.Scripts;
using F13StandardUtils.CollectTicket.PowerUp.Scripts;
using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageButton : BasePowerUpButton
{
    private PlayerData PlayerData => GameController.Instance.PlayerData;
    protected override float Level 
    {
        get => PlayerData.Damage;
        set => PlayerData.Damage = (int)value;
    }

    protected override List<int> LevelCosts()
    {
        return UpgradeCost.DAMAGE_POWER_UP_COSTS;
    }

    protected override string LevelString()
    {
        return "level: " + (PlayerData.Damage + 1);
    }

}
