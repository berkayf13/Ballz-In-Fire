using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.CollectTicket.Seat.Script.Core;
using UnityEngine;

public class FillQueueAreaTutorial : TutorialClick
{
    [SerializeField] private int unlockedSeatCount = 4;
    protected override bool TutorialStartCondition()
    {
        return base.TutorialStartCondition() && SeatManager.Instance &&
               SeatManager.Instance.UnlockedSeatsCount >= unlockedSeatCount;
    }
}
