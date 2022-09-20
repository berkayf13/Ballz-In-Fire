using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TowerCreator : MonoBehaviour
{

    [SerializeField] private GameObject _towerObject;
    [SerializeField] private Vector3 _towerCreatePos;
    [SerializeField] private int _towerCreateCount;
    [SerializeField] private List<GameObject> towerParts;
    [SerializeField] private Collider _towerCreatorCollider;
    [SerializeField] private GameObject _collectObject;
    private int _damageCount;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private string triggerTag;


    private void Awake()
    {
        Create();
    }
    
    
    private void Create()
    {

        for (int i = 0; i < _towerCreateCount; i++)
        {
            
            var newPart =Instantiate(_towerObject);
            newPart.transform.SetParent(this.transform);
            newPart.transform.localPosition = _towerCreatePos;
            _towerCreatePos += Vector3.up*0.5f;
            newPart.transform.eulerAngles = Vector3.up*i*2;
            towerParts.Add(newPart);
        }
        _collectObject.transform.localPosition = _towerCreatePos + Vector3.up*.75f;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(triggerTag))
        {
            _damageCount+=1;
            if (_damageCount==3)
            {
                DestroyTowerPart();
                _damageCount = 0;
                transform.Rotate(Vector3.up*5);
            }
            TowerDamagaScale();
            
        }

       
    }

    private void TowerDamagaScale()
    {
        gameObject.transform.DOScale(new Vector3(1.1f, 1f, 1.1f), 0.02f).OnComplete((() =>
        {
            gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.01f);
        }));
    }

    private void DestroyTowerPart()
    {
        Destroy(towerParts[0]);
        towerParts.Remove(towerParts[0]);
        foreach (var towerPart in towerParts)
        {
            towerPart.transform.localPosition -= Vector3.up * .5f;
        }
        
        
        if (towerParts.Count == 0)
        {
            _towerCreatorCollider.enabled = false;
            _particleSystem.Play();
        }
        _collectObject.transform.localPosition -= Vector3.up* 0.5f;
    }
}
