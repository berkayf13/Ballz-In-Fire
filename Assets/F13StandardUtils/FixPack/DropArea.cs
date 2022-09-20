using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class DropArea : MonoBehaviour
    {
        [SerializeField] private List<string> _tagList = new List<string>();
        [SerializeField,ReadOnly] private GameObject _currentObject;

        public GameObject CurrentObject => _currentObject;
        [ShowInInspector] public bool IsFilled => _currentObject != null;
        [ShowInInspector] public string CurrentTag => IsFilled? _currentObject.tag: string.Empty;
        

        public bool Drop(GameObject go)
        {
            if (IsFilled) return false;
            if (!_tagList.Contains(go.tag)) return false;
            _currentObject = go;
            return true;
        }

        public bool Cancel()
        {
            if (IsFilled)
            {
                var obj = _currentObject;
                _currentObject = null;
                return true;
            }

            return false;
        }
    }
}