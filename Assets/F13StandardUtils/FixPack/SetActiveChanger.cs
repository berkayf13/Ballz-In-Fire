using UnityEngine;

public class SetActiveChanger : MonoBehaviour
{
    public void Enable(GameObject go)
    {
        go.SetActive(true);
    }
    public void Disable(GameObject go)
    {
        go.SetActive(false);
    }
}
