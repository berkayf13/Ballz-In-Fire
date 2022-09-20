using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class ListCombiner : MonoBehaviour
{
    [SerializeField,ReadOnly] private List<GameObject> _listA=new List<GameObject>();
    [SerializeField,ReadOnly] private List<GameObject> _listB=new List<GameObject>();
    [SerializeField,ReadOnly] private List<GameObject> _listC=new List<GameObject>();
    [SerializeField,ReadOnly] private List<GameObject> _listD=new List<GameObject>();

    [SerializeField] private int _limitA = 10;
    [SerializeField] private int _limitB = 10;
    [SerializeField] private int _limitC = 10;
    [SerializeField] private int _limitD = 10;

    
    [SerializeField,ReadOnly] private List<GameObject> _combined=new List<GameObject>();
    
    public SerializedEvent<List<GameObject>> OnCombinedListUpdated=new SerializedEvent<List<GameObject>>();
    

    public void SetListA(List<GameObject> list)
    {
        var limit = list.Count > _limitA ? _limitA : list.Count;
        _listA = list.GetRange(0,limit);
        UpdateCombined();
    }
    public void SetListB(List<GameObject> list)
    {
        var limit = list.Count > _limitB ? _limitB : list.Count;
        _listB = list.GetRange(0,limit);
        UpdateCombined();

    }
    public void SetListC(List<GameObject> list)
    {
        var limit = list.Count > _limitC ? _limitC : list.Count;
        _listC = list.GetRange(0,limit);;
        UpdateCombined();
    }
    public void SetListD(List<GameObject> list)
    {
        var limit = list.Count > _limitD ? _limitD : list.Count;
        _listD = list.GetRange(0,limit);;
        UpdateCombined();
    }
    private void UpdateCombined()
    {
        _combined.Clear();
        _combined.AddRange(_listA);
        _combined.AddRange(_listB);
        _combined.AddRange(_listC);
        _combined.AddRange(_listD);
        OnCombinedListUpdated.Invoke(_combined);
    }
}
