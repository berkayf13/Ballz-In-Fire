using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class ModifierSet
{
    public BaseSeatModifier[] modifiers;
    public Sprite[] sprites;
}
public class SeatModifierSpriteService : Singleton<SeatModifierSpriteService>
{
    public List<ModifierSet> _modifierSetList = new List<ModifierSet>();

    
    [Button]
    private void CheckModifierSet(BaseSeatModifier[] ma)
    {
        string maId="";
        foreach (var m in ma)
        {
            maId += m.id;
        }
        
    }
    public ModifierSet FindFits(params BaseSeatModifier[] modifiers)
    {
        var modifierSet = _modifierSetList.Find(s =>
        {
            return s.modifiers.Length == modifiers.Length && modifiers.All(m => s.modifiers.Contains(m));
        });
        return modifierSet;
    }
    
    public bool TryGetSprite(out Sprite[] sprites,params BaseSeatModifier[] ma)
    {
        sprites = null;
        var modifierSet = FindFits(ma);
        if (modifierSet != null)
        {
            sprites = modifierSet.sprites;
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
