using F13StandardUtils.Scripts.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace F13StandardUtils.CollectTicket.PowerUp.Scripts
{
    public class PowerUpPanel:Singleton<PowerUpPanel>
    {

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);

        }
    }
}