using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class SeatManager : Singleton<SeatManager>
    {
        [SerializeField] private List<BaseSeatModifierInspector> _inspectors = new List<BaseSeatModifierInspector>();
        [SerializeField] private List<Seat> _seats;
        [SerializeField,ReadOnly] private List<Seat> _unlockedSeats;
        [SerializeField,ReadOnly] private List<Seat> _lockedSeats;
        [SerializeField,ReadOnly] private int _unlockedSeatsCount;
        [SerializeField,ReadOnly] private int _lockedSeatsCount;


        public List<BaseSeatModifierInspector> Inspectors => _inspectors.ToList();

        public List<Seat> Seats => _seats.ToList();

        public List<Seat> UnlockedSeats => _unlockedSeats.ToList();

        public List<Seat> LockedSeats => _lockedSeats.ToList();
        
        public int UnlockedSeatsCount => _unlockedSeatsCount;
        public int LockedSeatsCount => _lockedSeatsCount;
        
        public List<Seat> AvailableSeats => _unlockedSeats.Where(u => u.Unlocked.IsAvailable).ToList();

        public List<Seat> NotAvailableSeats => _unlockedSeats.Where(u => !u.Unlocked.IsAvailable).ToList();


        public bool IsThereUnlockedFitSeat(params BaseSeatModifier[] modifiers) =>
            _seats.Any(s => s.IsUnlocked && s.Unlocked.DoesFits(modifiers));

        public int SeatIndex(Seat seat)
        {
            var index= _seats.FindIndex(s => s.gameObject.name.Equals(seat.gameObject.name));
            return index;
        }

        public int SeatIndex(UnlockedSeat unlocked)
        {
            var index= _seats.FindIndex(s => s.gameObject.name.Equals(unlocked.transform.parent.gameObject.name));
            return index;
        }

        public int SeatIndex(LockedSeat locked)
        {
            var index= _seats.FindIndex(s => s.gameObject.name.Equals(locked.transform.parent.gameObject.name));
            return index;
        }


        private void OnEnable()
        {
            Refresh();
            GameController.Instance.OnLevelComplete.AddListener(OnLevelComplete);
        }

        private void OnDisable()
        {
            GameController.Instance?.OnLevelComplete.RemoveListener(OnLevelComplete);

        }

        private void OnLevelComplete(int l)
        {
            GameController.Instance.PlayerData.ResetSeatRatios();
        }

        public void Refresh()
        {
            FillInspectors();
            RefreshSeatLists();
        }

        private void RefreshSeatLists()
        {
            _unlockedSeats = _seats.Where(s => s.Locked.IsUnlocked).ToList();
            _lockedSeats = _seats.Where(s => !s.Locked.IsUnlocked).ToList();
            _unlockedSeatsCount = _unlockedSeats.Count;
            _lockedSeatsCount = _lockedSeats.Count;
        }


        private void FillInspectors()
        {
            _inspectors.ForEach(i=>i.Refresh());
        }

    }
}