using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSet : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private void Reset()
    {
        TryGetComponent(out _meshRenderer);
    }

    [SerializeField] private int matIndex;
    public void SetMaterial(Material material)
    {
        var materials = _meshRenderer.materials;
        materials[matIndex] = material;
        _meshRenderer.materials = materials;
    }
}
