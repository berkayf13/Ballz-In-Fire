using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMoney : MonoBehaviour
{
    private int _money;
    public void SetMoney(int money)
    {
        _money = money;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            MoneyManager.Instance.IncrementMoney(_money);
            Destroy(gameObject);
        }
    }

}
