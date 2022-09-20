using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CylinderObstacle : MonoBehaviour
{
    [SerializeField] private float _defHealth;
    [SerializeField] private float _health;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private TextMeshPro _tmp;
    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;
    [SerializeField] private Color _color3;
    [SerializeField] private Color _color4;


    private void Awake()
    {
        UpdateColor();
        UpdateText();


    }
    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<BallBullet>();
        if (bullet)
        {
            if (_health>0)
            {
                _health--;
                Destroy(bullet.gameObject);
                DamagaScale();
                ScaleDown();
                UpdateText();
                if (_health == 0) Destroy(transform.parent.gameObject);
            }

        }
    }

    private void DamagaScale()
    {
        transform.DOScale(new Vector3(_health / _defHealth+.025f, 1+0.25f, _health / _defHealth + .025f), 0.02f).OnComplete((() =>
        {
            transform.DOScale(new Vector3(_health / _defHealth, 1, _health / _defHealth), 0.01f);
        }));
    }

    private void UpdateColor()
    {
        if (_health <= 40) _mesh.material.color = _color4;
        if (_health <= 30) _mesh.material.color = _color3;
        if (_health <= 20) _mesh.material.color = _color2;
        if (_health <= 10) _mesh.material.color = _color1;

    }

    private void ScaleDown()
    {
        transform.DOScale(new Vector3(_health / _defHealth, 1, _health / _defHealth),0.25f);
        _mesh.transform.DOLocalMoveY(_mesh.transform.localPosition.y-0.25f, 0.25f);
    }

    private void UpdateText()
    {
        _tmp.text = "" + _health;
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.parent.gameObject);
    }
}
