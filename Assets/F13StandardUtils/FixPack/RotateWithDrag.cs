using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class RotateWithDrag : MonoBehaviour
{
    public Vector3 sensivity = new Vector3(0f,7.5f,0f);
    [SerializeField,ReadOnly]private bool isRotating;
    private Vector3 lastPosition;

    public bool IsRotating => isRotating;
    
    public UnityEvent OnRotateStart = new UnityEvent();
    public UnityEvent OnRotateEnd = new UnityEvent();
    public UnityEvent OnRotating = new UnityEvent();

    private void Update()
    {
        if (isRotating)
        {
            var currentPosition = Input.mousePosition;

            var diffVector = ( currentPosition- lastPosition);
            if (diffVector.magnitude > float.Epsilon)
            {
                var amount = diffVector.x < 0 ? diffVector.magnitude : -diffVector.magnitude;
                transform.Rotate(sensivity * amount * Time.deltaTime);
                lastPosition = currentPosition;
                OnRotating.Invoke();
            }
        }

    }

    protected void OnMouseDown()
    {
        isRotating = true;
        lastPosition = Input.mousePosition;
        OnRotateStart.Invoke();
    }

    protected void OnMouseUp()
    {
        isRotating = false;
        OnRotateEnd.Invoke();
    }

    public void SetRotation(Vector3 euler)
    {
        transform.eulerAngles = euler;
    }
    
    public void ResetRotation()=>SetRotation(Vector3.zero);
}
