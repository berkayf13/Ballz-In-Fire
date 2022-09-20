using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public abstract class BaseSeatModifierInspector: MonoBehaviour
    {
        [SerializeField,ReadOnly] private List<BaseSeatModifier> _allModifiers=new List<BaseSeatModifier>();

        public abstract bool InGroup(BaseSeatModifier m);

        public List<BaseSeatModifier> AllModifiers => _allModifiers.ToList();

        public void Refresh()
        {
            FillExistingModifiers();
        }
        
        private void FillExistingModifiers()
        {
            _allModifiers.Clear();
            var seats = SeatManager.Instance.UnlockedSeats;
            foreach (var seat in seats)
            {
                var modifiers = seat.Unlocked.SeatModifiers;
                foreach (var m in modifiers)
                {
                    if(InGroup(m) && !_allModifiers.Contains(m)) _allModifiers.Add(m);
                }
            }
        }
    }
}