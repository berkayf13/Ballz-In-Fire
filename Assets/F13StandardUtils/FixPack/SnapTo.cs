using Sirenix.OdinInspector;
using UnityEngine;

public class SnapTo : MonoBehaviour
{
    [SerializeField] private bool sameSnapRotation = true;
    [SerializeField,HideIf(nameof(sameSnapRotation))] private Vector3 snapEuler=Vector3.zero;
    [SerializeField] private Vector3 snapFailedEuler = new Vector3(0,45,0);

    [SerializeField] private Vector3 snapOffset=Vector3.zero;
    public void Snap(GameObject toObject)
    {
        transform.position = toObject.transform.position + snapOffset;
        transform.eulerAngles = sameSnapRotation? toObject.transform.eulerAngles: snapEuler;
    }

    public void SnapFailed()
    {
        transform.eulerAngles = snapFailedEuler;
    }
    
}
