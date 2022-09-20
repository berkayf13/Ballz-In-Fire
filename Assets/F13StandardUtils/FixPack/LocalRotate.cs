using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class LocalRotate : BaseRotate
    {
        public override Quaternion Current
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }
    }
}