using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public List<string> detectTags =new List<string>(){"Untagged"};
    
    [SerializeField] private SerializedEvent<GameObject> OnEnterTrigger=new SerializedEvent<GameObject>();
    [SerializeField] private SerializedEvent<GameObject> OnExitTrigger=new SerializedEvent<GameObject>();
    [SerializeField] private SerializedEvent<GameObject> OnEnterCollision=new SerializedEvent<GameObject>();
    [SerializeField] private SerializedEvent<GameObject> OnExitCollision=new SerializedEvent<GameObject>();

    

    private void OnTriggerEnter(Collider other)
    {
        if (detectTags.Contains(other.tag))
        {
            OnEnterTrigger.Invoke(other.gameObject);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (detectTags.Contains(other.tag))
        {
            OnExitTrigger.Invoke(other.gameObject);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (detectTags.Contains(other.gameObject.tag))
        {
            OnEnterCollision.Invoke(other.gameObject);

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (detectTags.Contains(other.gameObject.tag))
        {
            OnExitCollision.Invoke(other.gameObject);
        }
    }
}
