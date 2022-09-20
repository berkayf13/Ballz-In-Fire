using System;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private UnlockedSeat _unlocked;
        [SerializeField] private LockedSeat _locked;

        public UnlockedSeat Unlocked=> _unlocked;
        public LockedSeat Locked=> _locked;
        
        public bool IsUnlocked=>_locked.IsUnlocked;
        
        public bool IsVisible=>_locked.IsVisible;


        private void Update()
        {
            var isUnlocked = IsUnlocked;
            var isChanged = false;
            if (_unlocked.gameObject.activeInHierarchy != isUnlocked)
            {
                _unlocked.gameObject.SetActive(!_unlocked.gameObject.activeInHierarchy);
                isChanged = true;
            }

            var isVisible = IsVisible;
            if (_locked.gameObject.activeInHierarchy != (isVisible && !isUnlocked))
            {
                _locked.gameObject.SetActive(!_locked.gameObject.activeInHierarchy);
                isChanged = true;
            }
            if(isChanged) 
                SeatManager.Instance.Refresh();

        }
    }
}