using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class GlobalRotate: BaseRotate
    {
        public override Quaternion Current
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
    }
}