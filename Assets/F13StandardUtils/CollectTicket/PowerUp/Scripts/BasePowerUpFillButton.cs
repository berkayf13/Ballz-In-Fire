using System;
using F13StandardUtils.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace F13StandardUtils.CollectTicket.PowerUp.Scripts
{
    public abstract class BasePowerUpFillButton : BasePowerUp
    {
        public static float FILL_DURATION = 3f;
        public static float FILL_THRESH = 1f;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _fillObject;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Image _fillImage;
        [SerializeField] protected int visibleLevel = 0;
        private int unlockedAmount = 0;

        protected bool IsVisible=> !IsLevelMax() && visibleLevel == (int) Level;
        protected float FillAmount => Level - (int) Level;


        private void OnEnable()
        {
            if (IsVisible)
            {
                GetUnlockedRatio();
            }
        }

        protected override void OnLevelChanged()
        {
            UpdateView();
        }

        private float saved=0;

        private void OnMouseDrag()
        {
            
            if (IsVisible && LevelMoneyController.Instance.Money>0)
            {
                var fillRatio = Time.deltaTime / FILL_DURATION;
                var fillAmount = (fillRatio*LevelUpCost());
                saved += fillAmount;
                if (saved > FILL_THRESH)
                {
                    var intSaved = (int)saved;
                    var actualAmonut = Mathf.Clamp(intSaved, 0, LevelMoneyController.Instance.Money);
                    saved -= actualAmonut;
                    LevelMoneyController.Instance.SpendMoney(actualAmonut);
                    unlockedAmount+=actualAmonut;
                    SaveUnlockedRatio();
                    GameController.Instance?.LightHaptic();
                }
            }
            
        }
        
        private void GetUnlockedRatio()
        {
            unlockedAmount = (int) (FillAmount * LevelUpCost());


        }
        
        private void SaveUnlockedRatio()
        {
            Level = (int)Level+ (float)unlockedAmount / LevelUpCost();
        }

        protected virtual void UpdateView()
        {
            var isVisible = IsVisible;
            if (_collider.enabled!= isVisible)
            {
                _collider.enabled = isVisible;
                _fillObject.gameObject.SetActive(isVisible);
            }

            if (!IsLevelMax())
            {
                var fillAmount = FillAmount;
                _fillImage.fillAmount = fillAmount;
                _moneyText.text = "$"+(LevelUpCost()-unlockedAmount);
            }
        }
    }
}