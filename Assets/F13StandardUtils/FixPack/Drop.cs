using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.level5.NewScripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Drop : MonoBehaviour
{
    public SerializedEvent<GameObject> OnSuccess = new SerializedEvent<GameObject>();
    public UnityEvent OnFailed = new UnityEvent();
    public SerializedEvent<GameObject> OnCancel =new SerializedEvent<GameObject>();

    [SerializeField,ReadOnly] private DropArea _dropArea;
    [SerializeField,ReadOnly] private List<DropArea> _list;
    [SerializeField] private bool onlyNearestDropArea = true;

    public DropArea DropArea => _dropArea;

    public bool IsDropped => _dropArea != null;


    private void FixedUpdate()
    {
        if(IsDropped) return;
        if(!_list.Any()) return;
        _list = _list.OrderBy(i => (transform.position - i.transform.position).magnitude).ToList();
        if (onlyNearestDropArea)
        {
            var dropArea = _list.First();
            _list.Clear();
            _list.Add(dropArea);
        }
        foreach (var dropArea in _list)
        {
            if (dropArea.Drop(gameObject))
            {
                _dropArea = dropArea;
                _list.Clear();
                OnSuccess.Invoke(dropArea.gameObject);
                Debug.Log("Drop Success"+dropArea,gameObject);
                return;
            }
        }
        _list.Clear();
        OnFailed.Invoke();
        Debug.Log("Drop Failed",gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out DropArea dropArea))
        {
            Debug.Log("Enter"+other.name,gameObject);
            if(!_list.Contains(dropArea)) _list.Add(dropArea);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DropArea dropArea))
        {
            Debug.Log("Exit"+other.name,gameObject);
            if(_list.Contains(dropArea)) _list.Remove(dropArea);
            if(dropArea.Equals(_dropArea)) Cancel();
        }
    }

    public void Cancel()
    {
        if (!IsDropped) return;
        if(_dropArea.Cancel())
        {
            var obj = _dropArea;
            OnCancel.Invoke(obj.gameObject);
            _list.Clear();
            _dropArea = null;
            Debug.Log("Drop Cancel",obj);

        }
    }
}
