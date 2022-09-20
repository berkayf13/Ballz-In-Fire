using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public abstract class BaseEnumSpriteObject<T> : MonoBehaviour where T:Enum
{
    [SerializeField, OnValueChanged(nameof(UpdateCurrent))]
    private T _current;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int ToInt()
    {
        return Convert.ToInt32(Current);
    }

    [SerializeField] private List<Sprite> objectList = new List<Sprite>();
    public Sprite CurrentObject => objectList[ToInt()];
    public SpriteRenderer Renderer => _spriteRenderer;

    public T Current => _current;

    public virtual void SetCurrent(T value)
    {
        _current = value;
        UpdateCurrent();
    }
    
    protected virtual  void UpdateCurrent()
    {
        var index = ToInt() % objectList.Count;

        _spriteRenderer.sprite = objectList[index];
    }
}


public abstract class BaseEnumObject<T> : MonoBehaviour where T:Enum
{
    [SerializeField, OnValueChanged(nameof(UpdateCurrent))]
    private T _current;

    private int ToInt()
    {
        return Convert.ToInt32(Current);
    }

    [SerializeField] private List<GameObject> objectList = new List<GameObject>();
    public GameObject CurrentObject => objectList[ToInt()];

    public T Current => _current;

    public virtual void SetCurrent(T value)
    {
        _current = value;
        UpdateCurrent();
    }


    protected virtual  void UpdateCurrent()
    {
        var index = ToInt() % objectList.Count;
        for (var i = 0; i < objectList.Count; i++)
        {
            if(i==index) continue;
            var obj = objectList[i];
            if(obj.activeInHierarchy) 
                obj.gameObject.SetActive(false);
        }
        var activeGun = objectList[index];
        if(!activeGun.activeInHierarchy)
            activeGun.SetActive(true);
    }
}

[System.Serializable]
public class ListObject<T>
{
    public List<T> list = new List<T>();
}

public abstract class BaseEnumListObject<T> : MonoBehaviour where T:Enum
{
    [SerializeField, OnValueChanged(nameof(UpdateCurrent))]
    private T _current;

    private int ToInt()
    {
        return Convert.ToInt32(Current);
    }

    [SerializeField] private List<ListObject<GameObject>> objectList = new List<ListObject<GameObject>>();
    public List<GameObject> CurrentObject => objectList[ToInt()].list;

    public T Current => _current;

    public virtual void SetCurrent(T value)
    {
        _current = value;
        UpdateCurrent();
    }


    protected virtual void UpdateCurrent()
    {
        var index = ToInt() % objectList.Count;
        for (var i = 0; i < objectList.Count; i++)
        {
            if(i==index) continue;
            var obj = objectList[i];
            obj.list.ForEach(o=>
            {
                if(o.activeInHierarchy)
                    o.SetActive(false);
            });
        }
        objectList[index].list.ForEach(o=>
        {
            if(!o.activeInHierarchy)
                o.SetActive(true);
        });
    }
}

public abstract class BaseEnumObject<T,W> : MonoBehaviour where T:Enum,IConvertible where W: Component
{
    [SerializeField, OnValueChanged(nameof(UpdateCurrent))]
    private T _current;

    private int ToInt()
    {
        return Convert.ToInt32(Current);
    }

    [SerializeField] private List<W> objectList = new List<W>();
    public W CurrentObject => objectList[ToInt()];

    public T Current => _current;

    public virtual void SetCurrent(T value)
    {
        _current = value;
        UpdateCurrent();
    }


    protected virtual  void UpdateCurrent()
    {
        var index = ToInt() % objectList.Count;
        for (var i = 0; i < objectList.Count; i++)
        {
            if(i==index) continue;
            var obj = objectList[i];
            if(obj.gameObject.activeInHierarchy) 
                obj.gameObject.SetActive(false);
        }
        var activeObj = objectList[index];
        if(!activeObj.gameObject.activeInHierarchy)
            activeObj.gameObject.SetActive(true);
    }
    public W GetCurrentGameObject() => objectList[ToInt()];
}

public abstract class BaseEnumListObject<T,W> : MonoBehaviour where T:Enum where W: Component
{
    [SerializeField, OnValueChanged(nameof(UpdateCurrent))]
    private T _current;

    private int ToInt()
    {
        return Convert.ToInt32(Current);
    }

    [SerializeField] private List<ListObject<W>> objectList = new List<ListObject<W>>();
    public List<W> CurrentObject => objectList[ToInt()].list;

    public T Current => _current;

    public virtual void SetCurrent(T value)
    {
        _current = value;
        UpdateCurrent();
    }


    protected virtual  void UpdateCurrent()
    {
        var index = ToInt() % objectList.Count;
        for (var i = 0; i < objectList.Count; i++)
        {
            if(i==index) continue;
            var obj = objectList[i];
            obj.list.ForEach(o=>
            {
                if(o.gameObject.activeInHierarchy)
                    o.gameObject.SetActive(false);
            });
        }
        objectList[index].list.ForEach(o=>
        {
            if(!o.gameObject.activeInHierarchy)
                o.gameObject.SetActive(true);
        });
    }
}