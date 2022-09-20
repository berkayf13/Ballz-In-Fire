using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public enum DebugType{
        UI,
        Input,
        State,
        GameManager,
        SingletonManager,
        AudioManager,
        HapticManager
    }

    public class OmegaDebugManager : OmegaSingletonManager<OmegaDebugManager>
    {
        [Tooltip("These field close and open the debugs.")]
        public bool DeactivateDebugs= false;
        public bool UIDebug =false;
        public bool InputDebug = false;
        public bool StateDebug = false;

        [Space]
        public bool SingletonManager = false;
        public bool GameManager = false;
        public bool AudioManager = false;
        
        public bool HapticManager = false;

        public void PrintDebug(string debugMessage, DebugType type){
            if(DeactivateDebugs) return;
            if(UIDebug && (type == DebugType.UI)){
                Debug.Log("<b>F13 SDK:</b>\n <color=#008080ff>UI Log: </color> " + debugMessage);
            }
            else if(InputDebug && (type == DebugType.Input))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#ff00ffff>Input Log: </color> " + debugMessage);
            }
            else if(StateDebug && (type == DebugType.State))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#800000ff>Game State Log: </color> " + debugMessage);
            }
            else if (GameManager && (type == DebugType.GameManager))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#00ff00ff>Game Manager Log: </color> " + debugMessage);
            }
            else if(SingletonManager && (type == DebugType.SingletonManager))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#00ffffff>Singleton Manager Log: </color> " + debugMessage);
            }
            else if (AudioManager && (type == DebugType.AudioManager))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#808000ff>AudioManager Manager Log: </color> " + debugMessage);
            }
            else if(HapticManager && (type == DebugType.HapticManager))
            {
                Debug.Log("<b>F13 SDK:</b>\n <color=#ffa500ff>HapticManager Manager Log: </color> " + debugMessage);
            }
        }
        
    }
}