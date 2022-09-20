using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapableListener : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(!TapToTapController.Instance) return;
        if(!TapToTapController.Instance.SelectedTapable) return;
        TapToTapController.Instance.TapableListener(this);
    }
}
