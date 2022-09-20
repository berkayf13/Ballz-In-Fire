using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class LockedSeat : MonoBehaviour
    {
        public static float FILL_DURATION = 3f;
        public static float FILL_THRESH = 1f;

        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private int unlockCost = 100;
        [SerializeField] private int unlockedAmount = 0;
        [SerializeField] int visibilityCount = 0;
        [SerializeField] private bool alreadyUnlocked = false;

        
        public float UnlockedRatio => alreadyUnlocked? 1f :(float)unlockedAmount / unlockCost;
        public bool IsUnlocked =>  UnlockedRatio >= 1;

        public bool IsVisible => SeatManager.Instance.UnlockedSeatsCount >= visibilityCount;

        public int Remaining => alreadyUnlocked ? 0 : unlockCost - unlockedAmount;

        public int SeatIndex => SeatManager.Instance.SeatIndex(this);


        
        private void OnEnable()
        {
            LoadUnlockedRatio();
            UpdateView();
        }
        


        private void LoadUnlockedRatio()
        {
            var seatIndex = SeatIndex;
            var seatRatio = GameController.Instance.PlayerData.GetSeatRatio(seatIndex);
            SetUnlockedRatio(seatRatio);
        }
        
        private void SaveUnlockedRatio()
        {
            var seatIndex = SeatIndex;
            GameController.Instance.PlayerData.SetSeatRatio(seatIndex,UnlockedRatio);
        }
        
        private void SetUnlockedRatio(float ratio)
        {
            ratio = Mathf.Clamp(ratio, 0f, 1f);
            unlockedAmount = (int)(ratio * unlockCost);
        }



        private void UpdateView()
        {
            _fill.fillAmount = UnlockedRatio;
            _tmp.text="$"+Remaining.ToString();
        }
        
        private float saved=0;
        private void OnMouseDrag()
        {
            if (LevelMoneyController.Instance.Money>0)
            {
                var fillRatio = Time.deltaTime / FILL_DURATION;
                var fillAmount = (fillRatio*unlockCost);
                saved += fillAmount;
                if (saved > FILL_THRESH)
                {
                    var intSaved = (int)saved;
                    var actualAmout = Mathf.Clamp(intSaved, 0, LevelMoneyController.Instance.Money);
                    saved -= actualAmout;
                    LevelMoneyController.Instance.SpendMoney(actualAmout);
                    unlockedAmount+=actualAmout;
                    SaveUnlockedRatio();
                    UpdateView();
                    GameController.Instance?.LightHaptic();
                }
            }
        }
    }
}