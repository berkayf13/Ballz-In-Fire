using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Section
{
    [Range(0f,1f)] public float ratio;
    public List<GameObject> sectionObjects=new List<GameObject>();
}

public class SectionManager : MonoBehaviour
{
    [SerializeField,Range(0f,1f),OnValueChanged(nameof(UpdateCurrentRatio))] private float _currentRatio = 0;
    [SerializeField] private List<Section> _sections=new List<Section>();
    [SerializeField] private bool _isCompletedSectionsStayOpen = true;
    [SerializeField] private int _currentSection=0;
    public UnityEvent OnSectionChanged=new UnityEvent();

    public float CurrentRatio
    {
        get => _currentRatio;
        set
        {
            _currentRatio = Mathf.Clamp(value,0f,1f);
            UpdateCurrentRatio();
        }
    }

    public int CurrentSection => _currentSection;

    public bool IsCompletedSectionsStayOpen => _isCompletedSectionsStayOpen;

    public void UpdateCurrentRatio()
    {
        _currentSection = 0;
        var sectionOpenState = false;
        for (var i = _sections.Count - 1; i >= 0; i--)
        {
            var section = _sections[i];
            var isActive = section.ratio<=_currentRatio;
            isActive &= !sectionOpenState;
            foreach (var o in section.sectionObjects)
            {
                o.SetActive(isActive);
            }

            if (_currentSection< i && isActive) _currentSection = i;

            if (!_isCompletedSectionsStayOpen && isActive)
            {
                sectionOpenState = true;
            }

        }
    }

    private void Awake()
    {
        UpdateCurrentRatio();
    }

    private int _lastCurrenSection = -1;
    private void Update()
    {
        if (_lastCurrenSection != _currentSection)
        {
            _lastCurrenSection = _currentSection;
            OnSectionChanged.Invoke();
        }
    }
}
