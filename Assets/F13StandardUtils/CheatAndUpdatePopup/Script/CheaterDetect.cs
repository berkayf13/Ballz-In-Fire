using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class CheaterDetect : Singleton<CheaterDetect>
{
    [SerializeField] private GameObject _panel;
    
    private void Update()
    {
        if (GameController.Instance && GameController.Instance.PlayerData != null &&
            GameController.Instance.PlayerData.CheatingStatus && !_panel.activeInHierarchy)
        {
            ShowPopup();
            GameController.Instance.PlayerData.CheatingStatus = false;
        }
    }

    public void ShowPopup()
    {
        _panel.gameObject.SetActive(true);
    }

}
