using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.level5.NewScripts;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelSuccessWithDropAreas : BaseLevelSuccess
{
    [SerializeField] private List<DropArea> _dropAreas=new List<DropArea>();
    [SerializeField] private List<string> _successTagStrng=new List<string>();
    [SerializeField] private List<int> _starCountList=new List<int>();

    [ShowInInspector, ReadOnly] private int _filledCount;

    public int FilledCount => _filledCount;
    
    [ShowInInspector] public string TagString
    {
        get
        {
            var str = string.Empty;
            foreach (var dropArea in _dropAreas)
            {
                str += dropArea.IsFilled ? dropArea.CurrentTag : "_";
            }

            return str;
        }
    }
    protected override bool Value
    {
        get
        {
            var tagString = TagString;
            _filledCount = tagString.Length-tagString.Count(f => (f == '_'));
            var index= _successTagStrng.FindIndex(s=>s.Equals(tagString));
            var value=index > -1;
            if (value) successRatio = (float)_starCountList[index] / maxStar;
            return value;
        }
    }


}
