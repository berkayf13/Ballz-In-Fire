using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.F13SDK.Scripts
{
    public enum UI_TYPE
    {
        Runner
    }
    public enum UI_INPUTTYPE
    {
        DragToMove,
        TapAndHold,
        Swipe,
        Swerve,
        TapToStart,
        Sling

    }
    public class OmegaUIManager: OmegaSingletonManager<OmegaUIManager>
    {
        private OmegaUIManager() { }
        public Canvas OmegaUICanvas;
        public UI_TYPE Type;
        public UI_INPUTTYPE _input;
        public List<RunnerPanel> _runnerPanels;
        Dictionary<Type, IOmegaPanel> _omegaPanelMap = new Dictionary<Type, IOmegaPanel>();

        public void AwakeOmegaUIManager()
        {
            UISwitch();
        }

        public void UpdateOmegaUIManager()
        {
           foreach(KeyValuePair<Type, IOmegaPanel> omegaPanel in _omegaPanelMap)
           {
                if(omegaPanel.Value != null)
                    omegaPanel.Value.OmageOnUpdate();
           }
        }

        private void UISwitch()
        {
            switch (Type)
            {
                case UI_TYPE.Runner:
                    foreach (var _omagePanel in _runnerPanels)
                    {
                        if (_omagePanel != null)
                        {
                            var panel = Instantiate(_omagePanel, OmegaUICanvas.transform, false);
                            panel.OmegaPanelAwake();
                            _omegaPanelMap.Add(panel.GetType(), panel);
                            panel.HidePanel();
                        }
                    }
                    break;
            }
        }

    }

}
