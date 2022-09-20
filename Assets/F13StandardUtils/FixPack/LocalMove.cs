using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class LocalMove : BaseMove
    {
        public override Vector3 CurrentPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
    }
}