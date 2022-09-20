using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelSuccess : BaseLevelSuccess
{
    [SerializeField,ReadOnly] private List<GameObject> _successList=new List<GameObject>();
    [SerializeField] private  int successCount = 1;
    
    
    public void IncrementSuccess(GameObject successObject)
    {
        if (!_successList.Contains(successObject))
        {
            _successList.Add(successObject);
        }
    }
    public void DecrementSuccess(GameObject failObject)
    {
        if (_successList.Contains(failObject))
        {
            _successList.Remove(failObject);
        }
    }

    protected override bool Value => _successList.Count == successCount;
    
}
