using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class UpdateCheck : MonoBehaviour
{
    [SerializeField] private GameObject updatePanel;

    private void Update()
    {
        Check();
    }
    
    private void Check()
    {
        var check = CheckVersionNumber();
        if(updatePanel.activeInHierarchy!=check)
            updatePanel.SetActive(check);
    }

    private bool CheckVersionNumber()
    {
        return GameController.VERSION_NUMBER < GameController.MIN_VERSION_NUMBER;
    }

    public void OpenMarket()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.DogukanKurekci.ShootNDoor");
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1605775321");
#endif
    }
}
