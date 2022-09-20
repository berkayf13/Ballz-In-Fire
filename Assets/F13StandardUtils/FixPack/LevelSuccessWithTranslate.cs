using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



public class LevelSuccessWithTranslate : BaseLevelSuccess
{
    [SerializeField] private string _tagString;
    [SerializeField,ReadOnly] private List<string> _translated=new List<string>();
    [SerializeField] private List<Language> _languages = new List<Language>();
    [SerializeField] private List<string> _successTagStrng=new List<string>();
    [SerializeField] private List<int> _starCountList=new List<int>();
    
    public string TagString
    {
        get => _tagString;
        set => _tagString = value;
    }
    
    private void Awake()
    {
        PrepareLanguages();
    }

    private void PrepareLanguages()
    {
        foreach (var language in _languages)
        {
            language.UpdateDictionary();
        }
    }


    [System.Serializable]
    public class KeyPair
    {
        public char key;
        public char value;
    }

    [System.Serializable]
    public class Language
    {
        public string id;
        public List<KeyPair> keymap = new List<KeyPair>();
        public Dictionary<char, char> keyDic = new Dictionary<char, char>();

        public void UpdateDictionary()
        {
            keyDic.Clear();
            foreach (var keyPair in keymap)
            {
                keyDic.Add(keyPair.key,keyPair.value);
            }
        }

        public string Translate(string code)
        {
            var trasnlated = string.Empty;
            var charArray = code.ToCharArray();
            foreach (var c in charArray)
            {
                if (keyDic.TryGetValue(c, out char t))
                {
                    trasnlated += t;

                }
                else
                {
                    trasnlated += '_';
                }
            }

            return trasnlated;
        }
    }

    protected override bool Value
    {
        get
        {
            var code = _tagString;
            _translated.Clear();
            foreach (var language in _languages)
            {
                var translate = language.Translate(code);
                _translated.Add(translate);
            }
            
            foreach (var translate in _translated)
            {
                var index= _successTagStrng.FindIndex(s=>s.Equals(translate));
                var value=index > -1;
                if (value)
                {
                    successRatio = (float)_starCountList[index] / maxStar;
                    return true;
                }
            }
            return false;
        }
    }


}
