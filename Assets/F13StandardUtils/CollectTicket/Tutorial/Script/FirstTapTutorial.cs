using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTapTutorial : TutorialClick
{
    protected override bool TutorialNotStartCondition()
    {
        return base.TutorialNotStartCondition() || !TapToTapController.Instance;
    }
}
