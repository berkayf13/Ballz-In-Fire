using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable] public class SwerveEvent : UnityEvent<float>{}
    public class SwerveController : Singleton<SwerveController>
    {
        public bool canSwerve = true;
        [SerializeField] private float swerveSpeed = 0.5f;
        public float clampX = 60;
        public SwerveEvent OnSwerveChanged= new SwerveEvent();

        private float _currentDuration=0;
        public float lastPosition;
        public float NormalizedPosition => _currentDuration / clampX;
        

        private void Awake()
        {
            if (Application.isMobilePlatform)
            {
                Input.multiTouchEnabled = false;
            }
        }
        
        private void FixedUpdate()
        {
            Swerve();
        }

        private void Swerve()
        {
            if (!canSwerve) return;
            Simulate(Input.GetMouseButtonDown(0),Input.GetMouseButton(0),Input.mousePosition);

        }

        public void Simulate(bool isDown, bool isPressed, Vector3 mousePosition)
        {
            if (isDown)
            {
                lastPosition = mousePosition.x;
            }

            if (isPressed)
            {
                if (lastPosition == 0)
                {
                    lastPosition = mousePosition.x;
                }

                float currentX = mousePosition.x;
                float deltaX = currentX - lastPosition;
                float targetX = deltaX * swerveSpeed * Time.deltaTime;
                _currentDuration += (targetX);
                _currentDuration = Mathf.Clamp(_currentDuration, -clampX, clampX);
                lastPosition = mousePosition.x;
                OnSwerveChanged.Invoke(_currentDuration);
            }
        }

        public void Reset()
        {
            lastPosition = 0;
            _currentDuration = 0;
        }
    }
}