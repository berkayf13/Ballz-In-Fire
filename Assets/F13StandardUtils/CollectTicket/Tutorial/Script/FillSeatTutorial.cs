using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillSeatTutorial : TutorialClick
{
    [SerializeField] private int money = 100;
    protected override bool TutorialStartCondition()
    {
        return base.TutorialStartCondition() && LevelMoneyController.Instance &&
               LevelMoneyController.Instance.Money >= money;
    }
}
