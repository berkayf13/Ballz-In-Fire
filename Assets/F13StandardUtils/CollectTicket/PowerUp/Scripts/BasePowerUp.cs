using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour 
{
    protected abstract float Level { get; set; }
    protected abstract void OnLevelChanged();
    
    protected bool IsMoneyEnough => MoneyManager.Instance?.MoneyCount >= LevelUpCost();
    
    protected int LevelUpCost()
    {
        return LevelCosts()[(int)Level];
    }

    protected bool IsLevelMax()
    {
        return Level >= LevelCosts().Count;
    }

    protected abstract List<int> LevelCosts();
    

    protected abstract string LevelString();

    
    private float lastLevel=-1;

    protected virtual void LateUpdate()
    {
        if (!lastLevel.Equals(Level))
        {
            OnLevelChanged();
            lastLevel = Level;
        }
    }




    
    
    

    




}
