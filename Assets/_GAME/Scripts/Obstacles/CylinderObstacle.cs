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
    [SerializeField] private GameObject _gun;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Color _currentColor;
    [SerializeField] private float _current;
    [SerializeField, ReadOnly] private float _removed;
    [SerializeField] private CollectableMoney _money;
    [SerializeField] private int _earnMoney;
    [SerializeField] private Material _mat;
    public bool final;


    private Collider _col;
    private float _defHealth;
    private float _scaleDownXZ;
    private float _scaleDownY;
    private float _diffXZ;
    private float _diffY;


    private void Awake()
    {
        _col = GetComponent<Collider>(); ;
        UpdateText();
        CalculateDamage();
        UpdateColorNScale();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out Bullet bullet))
        {
            if (_health > 0)
            {
                _health--;
                DamagaAnim();
                UpdateText();
                UpdateColor();
                Damage();
                if (_health == 0)
                {
                    _col.enabled = false;
                    _tmp.enabled = false;
                    _model.gameObject.SetActive(false);
                    if (_gun.activeInHierarchy) _gun.transform.DOMoveY(0, 0.5f);
                    if (final && _money.gameObject.activeInHierarchy) _money.transform.DOMoveY(0, 0.5f);

                }
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
            transform.localScale = new Vector3(1.75f, 1.375f, 1.75f);
            _current = 0.9f;
        }

        if (_health <= 30)
        {
            transform.localScale = new Vector3(1.5f, 1.25f, 1.5f);
            _current = 0.6f;
        }

        if (_health <= 20)
        {
            transform.localScale = new Vector3(1.25f, 1.125f, 1.25f);
            _current = 0.3f;
        }

        if (_health <= 10)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            _current = 0f;
        }
        _currentColor = _gradient.Evaluate(_current);
        _mesh.material.color = _currentColor;

        if (final)
        {
            _money.gameObject.SetActive(true);
            _money.SetMoney(_earnMoney);
        }
        else
        {
            _money.gameObject.SetActive(false);
        }

    }
#endif

    private void UpdateColor()
    {
        if (_defHealth <= 10) return;
        _current -= _removed;
        _currentColor = _gradient.Evaluate(_current);
        _mesh.material.color = _currentColor;
    }

    private void UpdateText()
    {
        _tmp.text = "" + _health;
        _mesh.material = new Material(_mat.shader);
    }

    private void CalculateDamage()
    {
        _defHealth = _health;
        _removed = _current / (_defHealth - 10);
        if (_defHealth > 10)
        {
            _diffXZ = transform.localScale.x - 1f;
            _diffY = transform.localScale.y - 1f;
            _scaleDownXZ = _diffXZ / (_defHealth - 10);
            _scaleDownY = _diffY / (_defHealth - 10);
        }
        else
        {
            _scaleDownXZ = 0;
            _scaleDownY = 0;
        }

    }

    private void Damage()
    {
        if (_health >= 10)
        {
            transform.localScale -= Vector3.right * _scaleDownXZ;
            transform.localScale -= Vector3.forward * _scaleDownXZ;
            transform.localScale -= Vector3.up * _scaleDownY;
        }

    }
}
