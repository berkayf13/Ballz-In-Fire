
using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpecialOrderSource : Singleton<SpecialOrderSource>
{
    [SerializeField] private GameObject _specialOrderPrefab;
    [SerializeField,ReadOnly] private SpecialOrder _current;
    [SerializeField] private Transform holder;
    

    private void Update()
    {
        if(!_current || _current.IsUsed)
            Spawn();
    }

    [Button]
    public void Spawn()
    {
        _current = Instantiate(_specialOrderPrefab,holder.position,holder.rotation,holder).GetComponent<SpecialOrder>();
        _current.SpawnAnimation();

    }
}
