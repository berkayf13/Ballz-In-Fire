using Sirenix.OdinInspector;
using UnityEngine;

public class SnapArea : MonoBehaviour
{
    [SerializeField] private bool sameSnapRotation = true;
    [SerializeField,HideIf(nameof(sameSnapRotation))] private Vector3 snapEuler=Vector3.zero;
    [SerializeField] private Vector3 snapOffset=Vector3.zero;
    public void Snap(GameObject snapObject)
    {
        snapObject.transform.position = transform.position + snapOffset;
        snapObject.transform.eulerAngles = sameSnapRotation? transform.eulerAngles: snapEuler;

    }
}
