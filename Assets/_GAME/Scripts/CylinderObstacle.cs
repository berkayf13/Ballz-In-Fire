using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CylinderObstacle : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private TextMeshPro _tmp;
    [SerializeField] private Transform _model;
    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;
    [SerializeField] private Color _color3;
    [SerializeField] private Color _color4;

    private float _defHealth;
    private float _scaleDown;
    private float _diff;

    private void Awake()
    {
        UpdateText();
        UpdateColorNScale();
        _defHealth = _health;
        _diff = transform.localScale.x - 0.75f;
        _scaleDown = _diff / _defHealth;

    }
    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<BallBullet>();
        if (bullet)
        {
            if (_health > 0)
            {
                _health--;
                DamagaAnim();
                UpdateText();
                UpdateColor();
                transform.localScale -=  Vector3.one*_scaleDown;
                if (_health == 0) Destroy(gameObject);
            }

        }
    }

    private void DamagaAnim()
    {
        _model.transform.DOScale(new Vector3(1.025f, 1.025f, 1.025f), 0.02f).OnComplete((() =>
          {
              _model.transform.DOScale(new Vector3(1, 1, 1), 0.01f);
          }));
    }

#if UNITY_EDITOR
    [Button]
    private void UpdateColorNScale()
    {
        if (_health > 30)
        {
            _mesh.material.color = _color4;
            transform.localScale = new Vector3(1.75f, 1.375f, 1.75f);
        }

        if (_health <= 30)
        {
            _mesh.material.color = _color3;
            transform.localScale = new Vector3(1.5f, 1.25f, 1.5f);
        }

        if (_health <= 20)
        {
            _mesh.material.color = _color2;
            transform.localScale = new Vector3(1.25f, 1.125f, 1.25f);
        }

        if (_health <= 10)
        {
            _mesh.material.color = _color1;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }
#endif

    private void UpdateColor()
    {
        if (_health > 30) _mesh.material.DOColor(_color4,0.25f);
        if (_health <= 30) _mesh.material.DOColor(_color3, 0.25f);
        if (_health <= 20) _mesh.material.DOColor(_color2, 0.25f);
        if (_health <= 10) _mesh.material.DOColor(_color1, 0.25f);

    }
    private void UpdateText()
    {
        _tmp.text = "" + _health;
    }


}
