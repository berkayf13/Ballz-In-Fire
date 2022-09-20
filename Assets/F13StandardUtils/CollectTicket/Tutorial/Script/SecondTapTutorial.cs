using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTapTutorial : TutorialClick
{
    protected override bool TutorialNotStartCondition()
    {
        return base.TutorialNotStartCondition() || !TapToTapController.Instance;
    }
    
    protected override bool TutorialStartCondition()
    {
        return base.TutorialStartCondition() && TapToTapController.Instance && TapToTapController.Instance.IsSelected;
    }
}
