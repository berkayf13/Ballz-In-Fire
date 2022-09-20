using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Core.Script
{
    public class SendCustomerService: Singleton<SendCustomerService>
    {
        public static float LEVEL_END_INTERVAL = 2f;
        public static float LEVEL_START_INTERVAL = 5f;
        
        public bool isSend = true;

        public float MinIntervalMultiplier => 0.25f + 0.40f * (1f - LevelMoneyController.Instance.LevelCompleteRatio);
        public float Interval => Mathf.Lerp(LEVEL_END_INTERVAL, LEVEL_START_INTERVAL, 1f - LevelMoneyController.Instance.LevelCompleteRatio);
        private void Awake()
        {
            StartCoroutine(SendCustomerLoop());
        }
        
        private void OnEnable()
        {
            GameController.Instance.OnGameplayEnter.AddListener(OnGameplayEnter);
            GameController.Instance.OnGameplayExit.AddListener(OnGameplayExit);

        }

        private void OnDisable()
        {
            GameController.Instance?.OnGameplayEnter.RemoveListener(OnGameplayEnter);
            GameController.Instance?.OnGameplayExit.RemoveListener(OnGameplayExit);
        }

        private void OnGameplayEnter()
        {
            isSend = true;
        }
        
        private void OnGameplayExit()
        {
            isSend = false;
        }


        
        private IEnumerator SendCustomerLoop()
        {
            while (enabled)
            {
                if (isSend)
                {
                    var mult = UnityEngine.Random.Range(MinIntervalMultiplier,1f);
                    var interval = mult * Interval;
                    QueueCustomer();
                    yield return new WaitForSeconds(interval);
                }
                else
                {
                    yield return null;
                }
            }
            
        }

        public void QueueCustomer()
        {
            CustomerQueueManager.Instance.QueueCustomer((customer) => { customer.StartWaitProgress(); });
        }

        public void DequeueAndGoAway()
        {
            CustomerQueueManager.Instance.DequeueAndGoAway();
        }
    }
}