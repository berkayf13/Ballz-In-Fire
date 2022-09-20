using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class ShinyObject : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField,OnValueChanged(nameof(UpdateShine))] private bool isShine = false;
    [SerializeField] private List<int> _shineIndexes;
    [SerializeField] private Material _shineMat,_redShine;
    private Material[] _defaultMaterials;
    private Material[] _shineMaterials;

    public bool IsShine
    {
        get => isShine;
        set
        {
            isShine = value;
            UpdateShine();
        }
    }


    private void Awake()
    {
        StoreDefautlMaterials();
        UpdateShineIndexes();
    }

    private void Reset()
    {
        _renderer = GetComponent<Renderer>();
        _shineIndexes=new List<int>();
        for (var i = 0; i < _renderer.sharedMaterials.Length; i++)
        {
            _shineIndexes.Add(i);
        }
    }


    private void StoreDefautlMaterials()
    {
        _defaultMaterials = _renderer.sharedMaterials;
    }

    
    
    [Button,HideInEditorMode]
    private void UpdateShineIndexes()
    {
        UpdateShineMaterials();
        UpdateShine();
    }

    private void UpdateShine()
    {
        if(!Application.isPlaying) return;
        _renderer.sharedMaterials = IsShine?_shineMaterials : _defaultMaterials;
    }
    
    private void UpdateShineMaterials()
    {
        _shineMaterials = new Material[_defaultMaterials.Length];
        for (var i = 0; i < _defaultMaterials.Length; i++)
        {
            var isShining = _shineIndexes.Contains(i);
            _shineMaterials[i] = isShining ? Mat : _defaultMaterials[i];
        }
    }

    private Material Mat => isYellow ? _shineMat : _redShine;
    private bool isYellow=true;
    public void SetColor(bool color)
    {
        isYellow = color;
        UpdateShineMaterials();

    }
}
