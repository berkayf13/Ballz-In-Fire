using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TapListenerEvent : UnityEvent<Tapable, TapableListener> { }
[System.Serializable] public class TapEvent :  UnityEvent<Tapable> { }
public class TapToTapController : Singleton<TapToTapController>
{
    public TapEvent OnTapableSelect = new TapEvent();
    public TapEvent OnTapableCancel = new TapEvent();
    public TapEvent OnTapableChange = new TapEvent();
    public TapListenerEvent OnTapListenerSelect = new TapListenerEvent();
    public TapListenerEvent OnTapListenerFailed = new TapListenerEvent();
    
    [SerializeField,ReadOnly] private Tapable _selectedTapable,_lastTapable;
    [SerializeField,ReadOnly] private TapableListener _selectedTapableListener;

    public Tapable SelectedTapable => _selectedTapable;
    public Tapable LastTapable => _lastTapable;
    public TapableListener SelectedTapableListener => _selectedTapableListener;

    public bool IsSelected => _selectedTapable != null;
    public void SelectObject(Tapable t)
    {
        if (_selectedTapable == t)
        {
            CancelSelect();
            return;
        }
        else if (_selectedTapable)
        {
            OnTapableChange.Invoke(_selectedTapable);
            CancelSelect();
        }
        _selectedTapable = t;
        OnTapableSelect.Invoke(t);
    }

    public void CancelSelect()
    {
        _lastTapable = _selectedTapable;
        OnTapableCancel.Invoke(_selectedTapable);
        _selectedTapable = null;
    }

    public void TapableListener(TapableListener l)
    {
        if (_selectedTapable )
        {
            OnTapListenerSelect.Invoke(_selectedTapable,l);
            CancelSelect();
        }
    }

}
