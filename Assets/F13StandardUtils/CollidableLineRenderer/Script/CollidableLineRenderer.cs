using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


public enum CollisionMode
{
    Trigger,
    Collision,
    All
}

public class CollidableLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField, ReadOnly] private List<Collider> _collisions = new List<Collider>();
    [SerializeField] private CollisionMode _collisionMode = CollisionMode.All;

    public BoxCollider BoxCollider => _boxCollider;

    public List<Collider> Collisions => _collisions;

    public Transform StartPosition => _startPosition;

    public Transform EndPosition => _endPosition;

    [ShowInInspector] public float Distance => (_endPosition.position - _startPosition.position).magnitude;

    public void SetCollisionMode(CollisionMode mode)
    {
        _collisionMode = mode;
        UpdateCollisionMode();
    }

    private void UpdateCollisionMode()
    {
        UpdateCollisions();
        switch (_collisionMode)
        {
            case CollisionMode.Trigger:
                _collisions = _collisions.Where(c => c.isTrigger).ToList();
                break;
            case CollisionMode.Collision:
                _collisions = _collisions.Where(c => !c.isTrigger).ToList();
                break;
            case CollisionMode.All:
            default:
                break;
        }
    }

    public void SetColor(Color color)
    {
        _line.material.color = color;
    }

    public List<Collider> GetCollisions()
    {
        return _collisions.Where(c=>c).ToList();
    }

    public List<T> GetComponentsInCollisions<T>() where T: Component
    {
        return _collisions.Where(x=>x)
            .Select(x => x.GetComponent<T>())
            .Where(x => x != null).ToList();
    }

    public List<Collider> FindColliders(Vector3 insidePoint)
    {
        var list = new List<Collider>();
        foreach (var collision in _collisions)
        {
            if (collision && collision.bounds.Contains(insidePoint))
            {
                list.Add(collision);
            }
        }

        return list;
    }

    public List<T> FindComponents<T>(Vector3 insidePoint) where T: Component
    {
        var list = new List<T>();
        foreach (var collision in _collisions)
        {
            if (collision && collision.bounds.Contains(insidePoint) && collision.TryGetComponent(out T component))
            {
                list.Add(component);
            }
        }
        return list;
    }


    private void UpdateLineRendererPositions()
    {
        _line.positionCount = 2;
        _line.SetPosition(0, _startPosition.position);
        _line.SetPosition(1, _endPosition.position);
    }

    private void UpdateBoxColliderBounds()
    {
        _boxCollider.transform.LookAt(_endPosition);
        _boxCollider.transform.position = (_startPosition.position + _endPosition.position) * 0.5f;
        var size = _boxCollider.size;
        size.z = (_endPosition.position - _startPosition.position).magnitude;
        _boxCollider.size = size;
    }

    private void UpdateCollisions()
    {
        _collisions = Physics.OverlapBox(_boxCollider.transform.position,
                _boxCollider.size * 0.5f,
                _boxCollider.transform.rotation)
            .Where(c => !c.Equals(_boxCollider)).ToList();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!Application.isPlaying) return;
        if(_collisionMode == CollisionMode.Trigger) _collisions.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!Application.isPlaying) return;
        if(_collisionMode == CollisionMode.Trigger) _collisions.Remove(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!Application.isPlaying) return;
        if(_collisionMode == CollisionMode.Collision && !_collisions.Contains(other.collider)) _collisions.Add(other.collider);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(!Application.isPlaying) return;
        if(_collisionMode == CollisionMode.Collision && _collisions.Contains(other.collider)) _collisions.Remove(other.collider);
    }


    private void Update()
    {
        if (!_line) return;
        if (!_boxCollider) return;
        if (!_startPosition) return;
        if (!_endPosition) return;

        UpdateLineAndCollider();
        if(!Application.isPlaying) return;
        if (_collisionMode == CollisionMode.All)
        {
            UpdateCollisions();
        }
    }

    [DisableInPlayMode]
    [Button]
    private void UpdateLineAndCollider()
    {
        UpdateLineRendererPositions();
        UpdateBoxColliderBounds();
    }
}