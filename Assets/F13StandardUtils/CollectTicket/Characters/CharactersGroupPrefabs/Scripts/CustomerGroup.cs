using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public enum CustomerType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Type6,
    Type7,
    Type8,
    Type9,
    Type10
}

public class CustomerGroup : BaseEnumObject<CustomerType, CharacterUnitAnimController>
{

    public QuickOutline Outline => CurrentObject.GetComponent<QuickOutline>();
    public Transform Holder => GetHolder();

    public void Random()
    {
        var t = Utils.RandomEnum<CustomerType>();
        SetCurrent(t);
        CurrentObject.gameObject.SetActive(true);
    }
    
    private Transform GetHolder()
    {
        try
        {
            return CurrentObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0)
                .GetChild(0).GetChild(0);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return CurrentObject.transform;
        }
    }
}
