using System.Collections;
using System.Collections.Generic;
using Assets.F13SDK.Scripts;
using F13StandardUtils.CollectTicket.PowerUp.Scripts;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class QueueLengthButton : BasePowerUpButton
{
    
    private PlayerData PlayerData => GameController.Instance.PlayerData;

    protected override float Level
    {
        get => PlayerData.QueueLenght;
        set => PlayerData.QueueLenght = value;
    }
    protected override List<int> LevelCosts()
    {
        return CustomerQueueManager.CUSTOMER_QUEUE_LEVEL_COSTS;
    }
    
    protected override string LevelString()
    {
        return "count: " + CustomerQueueManager.Instance.QueueCapacity;
    }
    
}
