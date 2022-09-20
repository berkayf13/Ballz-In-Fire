using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class BoolDelegate : BaseDelegate<bool>
    {
        [SerializeField] private bool isInverse = false;

        protected override bool Value => isInverse ? !base.Value : base.Value;
    }
}