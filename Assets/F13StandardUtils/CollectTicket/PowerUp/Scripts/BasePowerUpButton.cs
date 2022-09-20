using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace F13StandardUtils.CollectTicket.PowerUp.Scripts
{
    public abstract class BasePowerUpButton :BasePowerUp
    {
        [SerializeField] private TextMeshProUGUI _lvlText;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Button _button;
        
        public bool Interactable => !IsLevelMax() && IsMoneyEnough;

        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        protected override void OnLevelChanged()
        {
            _lvlText.text = LevelString();
            _moneyText.text = IsLevelMax()?"MAX": LevelUpCost().ToString();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            var interactable = Interactable;
            if(_button.interactable != interactable)
                _button.interactable = interactable;
        }
        
        private void OnClick()
        {
            LevelUp();
        }
        
        private void LevelUp()
        {
            if (IsMoneyEnough)
            {
                LevelMoneyController.Instance.SpendMoney(LevelUpCost());
                Level++;
            }
            else
            {
                throw new Exception("Money is not enough");
            } 
        }
    }
}