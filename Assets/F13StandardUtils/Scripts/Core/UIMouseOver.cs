using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UIMouseOver : BaseObjectUpdater<bool>
{
    public UnityEvent OnEnterUI=new UnityEvent();
    public UnityEvent OnExitUI=new UnityEvent();
    
    
    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsThereAnyUIObject(Input.mousePosition);
    }

    protected override bool Value => IsPointerOverUIObject();
    protected override void OnValueUpdate()
    {
        if (value)
        {
            OnEnterUI.Invoke();
            Debug.Log("OnEnterUI");
        }
        else
        {
            OnExitUI.Invoke();
            Debug.Log("OnExitUI");

        }
    }
}
