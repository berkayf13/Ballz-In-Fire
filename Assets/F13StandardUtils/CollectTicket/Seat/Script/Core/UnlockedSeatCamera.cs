using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class UnlockedSeatCamera : MonoBehaviour
    {
        private int lastCount;

        private int UnlockedSeatCount => SeatManager.Instance.UnlockedSeatsCount;

        private void Update()
        {
            var unlocked = UnlockedSeatCount;
            if (unlocked != lastCount)
            {
                CameraController.Instance.CameraToListPoint(unlocked);
                lastCount = unlocked;
            }
        }
    }
}