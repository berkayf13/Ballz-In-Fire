using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    [System.Serializable]
    public class RequestItem
    {
        public List<BaseSeatModifier> modifiers;
    }
    
    [System.Serializable]
    public class LevelRatioPair
    {
        public float levelCompleteRatio;
        [Range(1, 8)] public int alreadyFilledRepeatCount = 4;
        public float alreadyFilledRatio = 0.5f;
        [Range(0f, 1f)] public List<float> ratios=new List<float>(){0.5f,0.5f,0f};
    }
    public class RandomSeatRequestManager:Singleton<RandomSeatRequestManager>
    {

        [SerializeField] private bool isSequence = true;
        [SerializeField] private List<RequestItem> sequence = new List<RequestItem>();
        
        [SerializeField] private List<LevelRatioPair> _LevelRatioPairs=new List<LevelRatioPair>();

        public float LevelCompleteRatio => LevelMoneyController.Instance.LevelCompleteRatio;

        public LevelRatioPair CurrentRatios
        {
            get
            {
                var pair = new LevelRatioPair()
                {
                    levelCompleteRatio=0f,
                    ratios = new List<float>(){0f, 0f, 0f}
                };
                if (!Application.isPlaying) return pair;
                for (var index = _LevelRatioPairs.Count - 1; index >= 0; index--)
                {
                    var levelRatioPair = _LevelRatioPairs[index];
                    if (LevelCompleteRatio >= levelRatioPair.levelCompleteRatio)
                    {
                        pair = levelRatioPair;
                        break;
                    }
                }

                return pair;
            }
        }

        private int counter = 0;
        [ShowInInspector] private bool IsAlreadyRepeat=> AlreadyRepeatCounter==0;
        [ShowInInspector] private int AlreadyRepeatCounter=>counter % CurrentRatios.alreadyFilledRepeatCount;

        
        [Button]
        public List<BaseSeatModifier> Random()
        {
            if (isSequence && sequence.Any())
            {
                _lastRequest= sequence.First().modifiers;
                sequence.RemoveAt(0);
            }
            else
            {
                counter++;
                var current = CurrentRatios;
                var isAlreadyFilledSeat = Utils.RandomBool(current.alreadyFilledRatio);
                _lastRequest = IsAlreadyRepeat && isAlreadyFilledSeat ? RandomInNotAvaliableSeats() : RandomInInspectors();
            }
            return _lastRequest;
        }
        [Button]
        private List<BaseSeatModifier> RandomInNotAvaliableSeats()
        {
            var notAvailableSeats = SeatManager.Instance.NotAvailableSeats;
            return notAvailableSeats.Any() ? notAvailableSeats.Random().Unlocked.SeatModifiers : RandomInInspectors();
        }
        [Button]
        private List<BaseSeatModifier> RandomInInspectors()
        {
            var list=new List<BaseSeatModifier>();

            var inspectors = SeatManager.Instance.Inspectors;
            for (var i = 0; i < inspectors.Count; i++)
            {
                var isInspector = Utils.RandomBool(CurrentRatios.ratios[i]);

                if (isInspector)
                {
                    var inspectorModifiers = inspectors[i].AllModifiers;
                    if (!inspectorModifiers.Any()) continue;

                    var validModifiers = new List<BaseSeatModifier>();
                    for (var index = 0; index < inspectorModifiers.Count; index++)
                    {
                        var m = inspectorModifiers[index];
                        var tempList = list.ToList();
                        tempList.Add(m);
                        if (SeatManager.Instance.IsThereUnlockedFitSeat(tempList.ToArray()))
                        {
                            validModifiers.Add(m);
                        }
                    }

                    if (!validModifiers.Any()) continue;
                    var modifier = validModifiers.Random();
                    list.Add(modifier);
                }
            }

            return list;
        }

        [SerializeField,ReadOnly] private List<BaseSeatModifier> _lastRequest=new List<BaseSeatModifier>();

        
    }
}