using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SimpleCursor))]
public class SimpleCursorChanger : MonoBehaviour
{
    [SerializeField] private SimpleCursor _cursor;
    public List<Sprite> _pressedList=new List<Sprite>();
    public List<Sprite> _notPressedList=new List<Sprite>();
    public List<KeyCode> _shortcuts=new List<KeyCode>();

    private void Reset()
    {
        TryGetComponent(out _cursor);
    }

    private void Update()
    {
        for (var i = 0; i < _shortcuts.Count; i++)
        {
            var keyCode = _shortcuts[i];
            if (Input.GetKeyDown(keyCode))
            {
                ChangeCursor(i);
            }
        }
    }

    [Button]
    private void ChangeCursor(int index)
    {
        _cursor.Pressed = _pressedList[index];
        _cursor.NotPressed = _notPressedList[index];
    }
}