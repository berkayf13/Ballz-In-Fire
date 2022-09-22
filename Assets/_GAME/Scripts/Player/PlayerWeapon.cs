using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GunGroup _gun;
    public GunGroup Gun => _gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GunGroup gun))
        {
            _gun.SetCurrent(gun.Current);
            Destroy(gun.gameObject);
        }
    }
}
