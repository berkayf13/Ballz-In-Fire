using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class LevelSuccessWithOrdering : BaseLevelSuccess
    {
        [SerializeField] private List<GameObject> objects=new List<GameObject>();
        [SerializeField] private List<string> _successTagStrng=new List<string>();
        [SerializeField] private List<int> _starCountList=new List<int>();

        
        public List<GameObject> Objects
        {
            get => objects;
        }

        [ShowInInspector] public string TagString
        {
            get
            {
                var str = string.Empty;
                foreach (var go in objects)
                {
                    str += go.tag;
                }

                return str;
            }
        }
        protected override bool Value
        {
            get
            {
                var tagString = TagString;
                var index= _successTagStrng.FindIndex(s=>s.Equals(tagString));
                var value=index > -1;
                if (value) successRatio = (float)_starCountList[index] / maxStar;
                return value;
            }
        }

        public void SetObjects(List<GameObject> list) => objects = list;

        public void SetObjects<T>(List<T> list) where T:Component
        {
            objects = list.Select(o => o.gameObject).ToList();
        }
        
        
        
        
    }
}