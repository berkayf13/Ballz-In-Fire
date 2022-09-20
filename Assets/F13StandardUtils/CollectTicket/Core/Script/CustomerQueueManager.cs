using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustomerQueueManager : Singleton<CustomerQueueManager>
{
    public static List<int> CUSTOMER_QUEUE_LEVEL_COSTS = new List<int> {200, 400};
    public static int DEFAULT_QUEUE_CAPACITY = 2;
    public static float SPAWN_SPEED = 40;
    public static float MOVE_SPEED = 10;


    [SerializeField] private GameObject _customerPrefab;

    public Transform customerSpawnPoint, customerEndPoint;
    public List<Transform> queuePoints=new List<Transform>();
    [ReadOnly] public List<Customer> customerList= new List<Customer>();
    

    public int QueueCapacity =>  DEFAULT_QUEUE_CAPACITY + (int)GameController.Instance.PlayerData.QueueLenght;
    
    


    [Button]
    public void QueueCustomer(Action<Customer> onSpawn=null, Action<Customer> onQueueReceive=null)
    {
        if(customerList.Count>=QueueCapacity) return;
        int nextIndex = customerList.Count;
        if (nextIndex >= queuePoints.Count) return;
        var go = Instantiate(_customerPrefab,customerSpawnPoint.position,Quaternion.identity,transform);
        var customer = go.GetComponent<Customer>();
        var point = queuePoints[nextIndex].position;
        customer.Move(point,SPAWN_SPEED, () =>
        {
            customer.transform.LookAt(customer.transform.position+Vector3.back);
            onQueueReceive?.Invoke(customer);
        });
        customerList.Add(customer);
        onSpawn?.Invoke(customer);
    }

    [Button]
    public void DequeueAndGoAway(Action<Customer> onDeque=null)
    {
        if(!customerList.Any()) return;
        DequeueAndGo(customerList[0], customerEndPoint.position,true, onDeque);
    }
    
    [Button]
    public void DequeueAndGoAway(Customer customer,bool isDestroy=true,Action<Customer> onDeque=null)
    {
        if(!customerList.Any()) return;
        DequeueAndGo(customer, customerEndPoint.position,isDestroy, onDeque);
    }
    
    public void DequeueAndGo(Customer customer,Vector3 destPoint,bool isDestroy,Action<Customer> onDeque=null)
    {
        if(!customerList.Contains(customer)) return;
        customerList.Remove(customer);
        Go(customer, destPoint, isDestroy, onDeque);
        UpdateQueuePosition();

    }

    public void GoAway(Customer customer,bool isDestroy,Action<Customer> onArrive=null)
    {
        Go(customer,customerEndPoint.position,isDestroy,onArrive);
    }

    public void Go(Customer customer,Vector3 destPoint,bool isDestroy,Action<Customer> onArrive=null)
    {
        var point = destPoint;

        customer.Move(point,MOVE_SPEED, () =>
        {
            onArrive?.Invoke(customer);
            if(isDestroy) Destroy(customer.gameObject);
        });
    }
    
    public void DequeueCustomer(Customer customer,bool isDestroy=false,Action<Customer> onDeque=null)
    {
        if(!customerList.Contains(customer)) return;
        customerList.Remove(customer);
        onDeque?.Invoke(customer);
        if(isDestroy) Destroy(customer.gameObject);
        UpdateQueuePosition();

    }

    public void UpdateQueuePosition()
    {
        for (var i = 0; i < customerList.Count; i++)
        {
            var customer = customerList[i];
            if (DOTween.IsTweening(customer)) customer.DOKill();
            customer.Move(queuePoints[i].position,MOVE_SPEED, () =>
            {
                customer.transform.LookAt(customer.transform.position+Vector3.back);
            });
        }
    }
}
