using System.Collections;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseAlignObjects<T> : MonoBehaviour where T:Component
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private List<T> _objects=new List<T>();
    [SerializeField,ReadOnly] private List<T> _lastOrdered=new List<T>();
    
    [SerializeField] private Vector3 alignPositionWeight=new Vector3(1,0,0);

    protected abstract Vector3 PositionOf(Transform obj);
    protected abstract void Move(T obj,Vector3 pos);
    protected abstract float SizeOf(T obj);
    
    public SerializedEvent<List<GameObject>> OnPositionUpdated=new SerializedEvent<List<GameObject>>();
    protected void Update()
    {
        var ordered = _objects.OrderBy(o =>
        {
            var pos = PositionOf(o.transform);
            var orderValue = alignPositionWeight.x * pos.x;
            orderValue += alignPositionWeight.y * pos.y;
            orderValue += alignPositionWeight.z * pos.z;
            return orderValue ;
        }).ToList();
        if (_lastOrdered.Count != ordered.Count)
        {
            _lastOrdered = ordered;
            UpdatePositions();
            return;
        }

        var willPositionUpdate = false;
        
        for (var i = 0; i < ordered.Count; i++)
        {
            if(!ordered[i].Equals(_lastOrdered[i]))
            {
                willPositionUpdate = true;
                _lastOrdered = ordered;
                break;
            }
        }
        if(willPositionUpdate) UpdatePositions();
        
    }
    
    private void UpdatePositions()
    {
        var pos = PositionOf(_startPos);
        for (var index = 0; index < _lastOrdered.Count; index++)
        {
            var obj = _lastOrdered[index];
            var halfSize=alignPositionWeight* SizeOf(obj) * .5f;
            pos+= halfSize;
            Move(obj,pos);
            pos += halfSize;
        }
        OnPositionUpdated.Invoke(_lastOrdered.Select(o=>o.gameObject).ToList());
    }

    public void Add(GameObject obj)
    {
        if (obj.TryGetComponent(out T component) && !_objects.Contains(component))
        {
            _objects.Add(component);
        }
    }
    public void Remove(GameObject obj)
    {
        if (obj.TryGetComponent(out T component) && _objects.Contains(component))
        {
            _objects.Remove(component);
        }
    }

}
