using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class UnlockedSeatCountObjectDisabler:BaseObjectDisabler
    {
        [SerializeField] private int visibleObjectCount = 0;
        protected override bool Value => visibleObjectCount <= SeatManager.Instance.UnlockedSeatsCount;
    }
}