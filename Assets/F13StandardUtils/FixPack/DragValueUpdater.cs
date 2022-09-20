using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class DragValueUpdater : MonoBehaviour
    {
        [SerializeField] private float _dragSensivity = 1f;
        [SerializeField,ReadOnly] private bool isDragging;
        [SerializeField,ReadOnly] private Vector3 lastMousePosition;

        [SerializeField] private bool isClamp = true;
        [SerializeField,ShowIf(nameof(isClamp))] private Vector3 _minClamp = new Vector3(-1,-2.5f);
        [SerializeField,ShowIf(nameof(isClamp))] private Vector3 _maxClamp = new Vector3(1,2.5f);
        [SerializeField] private Vector3 _current;
        [SerializeField,ReadOnly] private Vector3 _lastDiff;

        public SerializedEvent<Vector3> OnValueChanged = new SerializedEvent<Vector3>();
        public SerializedEvent<float> OnValueChangedX = new SerializedEvent<float>();
        public SerializedEvent<float> OnValueChangedY = new SerializedEvent<float>();
        public SerializedEvent<Vector3> OnDiff = new SerializedEvent<Vector3>();
        public SerializedEvent<float> OnDiffX = new SerializedEvent<float>();
        public SerializedEvent<float> OnDiffY = new SerializedEvent<float>();

        public Vector3 Current => _current;
        public Vector3 LastDiff => _lastDiff;
        

        private void Update()
        {
            var currentMousePosition = Input.mousePosition;
            if (isDragging)
            {
                var diffVector = ( currentMousePosition- lastMousePosition);
                diffVector = diffVector *_dragSensivity;
                _current += diffVector;
                var newCurrent= isClamp?_current.Clamp(_minClamp, _maxClamp):_current;
                _lastDiff = newCurrent - _current;
                _current = newCurrent;
                OnValueChanged.Invoke(_current);
                OnValueChangedX.Invoke(_current.x);
                OnValueChangedY.Invoke(_current.y);
                OnDiff.Invoke(_lastDiff);
                OnDiffX.Invoke(_lastDiff.x);
                OnDiffY.Invoke(_lastDiff.x);


            }
            lastMousePosition = currentMousePosition;

        }
        
        void OnMouseDown()
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        void OnMouseUp()
        {
            isDragging = false;
        }
    }
}