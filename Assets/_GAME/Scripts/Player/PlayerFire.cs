using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _weapon;
    [SerializeField] private GameObject _ballPrefap;
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private float _bulletSpeed;

    public float bulletFireRate = 0.5f;
    public float bulletRange = 2;
    public float bulletBouncy = 1;
    public float bulletCount = 1;

    private bool onUpdateCoroutine;

    private void Update()
    {
        if (GameController.Instance.IsPlaying && MoveZ.Instance.isMove)
        {
            if (!onUpdateCoroutine)
            {
                StartCoroutine(Fire());
            }
        }
    }
    private void CreateBall()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            _bulletSpawnPos.transform.rotation = Quaternion.Euler(0, Random.Range(-1f - bulletCount, 1f + bulletCount), 0);
            var bullet = Instantiate(_ballPrefap, _bulletSpawnPos.position, _bulletSpawnPos.rotation).GetComponent<Bullet>();
            bullet.transform.parent = transform.parent;
            bullet.SetVelocity(_bulletSpawnPos.transform.forward * _bulletSpeed);
            bullet.SetRange(bulletRange);
            bullet.SetBouncy(bulletBouncy);
            var bulletGroup=bullet.GetComponent<BulletGroup>();
            bulletGroup.SetCurrent(_weapon.Gun.Current);

        }


    }

    private IEnumerator Fire()
    {
        onUpdateCoroutine = true;
        PlayerAnimController.Instance.Attack();
        yield return new WaitForSeconds(bulletFireRate);
        CreateBall();
        onUpdateCoroutine = false;
    }




}
