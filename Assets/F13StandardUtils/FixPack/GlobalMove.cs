using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class GlobalMove : BaseMove
    {
        public override Vector3 CurrentPosition
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}