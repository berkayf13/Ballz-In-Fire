using F13StandardUtils.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefap;
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _fireRate;
    private bool onUpdateCoroutine;

    private void LateUpdate()
    {
        if (GameController.Instance.IsPlaying)
        {
            if (!onUpdateCoroutine)
            {
                StartCoroutine(Fire());
            }
        }
    }
    private void CreateBall()
    {
        _bulletSpawnPos.transform.rotation = Quaternion.Euler(0,Random.Range(-1f,1f), 0);
        var ballBullet = Instantiate(_ballPrefap, _bulletSpawnPos.position, _bulletSpawnPos.rotation).GetComponent<BallBullet>();
        ballBullet.SetVelocity(_bulletSpawnPos.transform.forward * _bulletSpeed);
    }

    private IEnumerator Fire()
    {
        onUpdateCoroutine = true;
        PlayerAnimController.Instance.Attack();
        yield return new WaitForSeconds(_fireRate);
        CreateBall();
        onUpdateCoroutine = false;
    }




}
