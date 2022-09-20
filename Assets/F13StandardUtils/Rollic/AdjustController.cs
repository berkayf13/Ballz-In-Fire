
// using com.adjust.sdk;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class AdjustController : MonoBehaviour
{
    private string lvl10 = "2jumn6";
    private string lvl20 = "22v6pr";
    private string lvl30="ub65lu";
    private string lvl40="l18ha8";
    private string lvl50="fkb9km";


    
    // Start is called before the first frame update
    void Awake()
    {
        GameController.Instance.OnLevelComplete.AddListener(OnLevelSuccess);
        
    }

    private void OnDestroy()
    {
        GameController.Instance?.OnLevelComplete.AddListener(OnLevelSuccess);
    }

    private void OnLevelSuccess(int l)
    {
        var level = l + 1;
        string eventKey = "";
        
        if (level == 10)
        {
            eventKey = lvl10;
        }
        else if(level ==20)
        {
            eventKey = lvl20;
        }
        else if (level == 30)
        {
            eventKey = lvl30;
        }
        else if (level == 40)
        {
            eventKey = lvl40;
        }
        else if (level == 50)
        {
            eventKey = lvl50;
        }

        if (eventKey != "")
        {
            // AdjustEvent adjustEvent = new AdjustEvent(eventKey);
            // Adjust.trackEvent(adjustEvent);
        }
    }
}
