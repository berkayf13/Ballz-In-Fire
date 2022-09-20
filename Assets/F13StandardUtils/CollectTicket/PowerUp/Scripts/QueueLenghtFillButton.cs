using System.Collections.Generic;
using Assets.F13SDK.Scripts;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.PowerUp.Scripts
{
    public class QueueLenghtFillButton:BasePowerUpFillButton
    {
        [SerializeField] private GameObject ground;
        private PlayerData PlayerData => GameController.Instance.PlayerData;
        private PlayerData Disable => GameController.Instance.PlayerData;

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

        protected override void UpdateView()
        {
            base.UpdateView();
            ground.SetActive(visibleLevel < (int)Level);
        }
    }
}