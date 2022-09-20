using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tapable : MonoBehaviour
{
    [SerializeField] private bool _isSelected;

    public bool IsSelected => _isSelected;
    private void OnMouseDown()
    {
        if(!TapToTapController.Instance) return;
        if(!enabled) return;
        TapToTapController.Instance.SelectObject(this);
    }
    
}
