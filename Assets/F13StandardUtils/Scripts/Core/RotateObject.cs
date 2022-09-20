using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class RotateObject : MonoBehaviour
{
    
    private Sequence _sequence;

    [SerializeField] private List<Vector3> targetVectors;
    [SerializeField] private List<float> durations;
    [SerializeField] private List<Ease> eases;
    [SerializeField] private bool rotationX, rotationY, rotationZ;
    [SerializeField] private float rotationSpeed;


    private void OnEnable()
    {
       
        Start();
        Update();
    }
    
    private void Start()
    {

        _sequence = DOTween.Sequence();
        for (int i = 0; i < targetVectors.Count; i++)
        {
            if (durations.Count >= i && eases.Count >= i)
                _sequence.Append(transform.DOLocalMove(targetVectors[i], durations[i]).SetEase(eases[i]));
        }

        _sequence.SetLoops(-1);
    }

    private void Update()
    {
        if (rotationX) transform.Rotate(transform.right * Time.deltaTime * rotationSpeed);
        if (rotationY) transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        if (rotationZ) transform.Rotate(transform.forward * Time.deltaTime * rotationSpeed);
    }
    
}