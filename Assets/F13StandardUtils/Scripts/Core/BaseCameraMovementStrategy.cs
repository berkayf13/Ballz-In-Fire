using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class BaseCameraMovementStrategy : MonoBehaviour
    {
        public float minClampX =-2;
        public float maxClampX =2;
        public float minClampZ =-2;
        public float maxClampZ =10;
        public float speed = 1f;
        
        private void Awake()
        {
            if (Application.isMobilePlatform)
            {
                Input.multiTouchEnabled = false;
            }
        }
    }
}