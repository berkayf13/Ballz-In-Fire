using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTutorial : TutorialClick
{
    protected override bool TutorialNotStartCondition()
    {
        return base.TutorialNotStartCondition() || !DragController.Instance;
    }
}
