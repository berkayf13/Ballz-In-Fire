using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keyData = new List<TKey>();

        [SerializeField]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }

    [System.Serializable]
    public class PoolDictionary : UnitySerializedDictionary<Type, List<MonoBehaviour>> { }
    public class PoolManager : Singleton<PoolManager>
    {
        public static int POOL_CLEAN_SIZE = 300;
        [SerializeField] private PoolDictionary activePool = new PoolDictionary();
        [SerializeField] private PoolDictionary passivePool = new PoolDictionary();


        public T Instantiate<T>(GameObject prefab) where T: MonoBehaviour
        {
            var type = typeof(T);
            if (!passivePool.TryGetValue(type, out var passiveTypePool))
            {
                passiveTypePool = new List<MonoBehaviour>();
                passivePool.Add(type,passiveTypePool);
            }

            if (passiveTypePool.Any())
            {
                var mono = passiveTypePool.First();
                if (!mono)
                {
                    CleanGhostsInPools();
                    return Instantiate<T>(prefab);
                }
                else
                {
                    passiveTypePool.Remove(mono);
                    if(!mono.gameObject.activeSelf) mono.gameObject.SetActive(true);
                    activePool[type].Add(mono);
                    return mono as T;
                }
            }
            else
            {
                var component = Instantiate(prefab,transform).GetComponent<T>();
                if (!activePool.TryGetValue(type, out var activeTypePool))
                {
                    activeTypePool = new List<MonoBehaviour>();
                    activePool.Add(type,activeTypePool);
                }
                activeTypePool.Add(component);
                return component;
            }
        }

        public void DestroyAll<T>()
        {
            var type = typeof(T);
            if (activePool.TryGetValue(type,out var activeTypePool))
            {
                var list = activeTypePool.ToList();
                var passiveTypePoo = passivePool[type];
                foreach (var mono in list)
                {
                    if (activeTypePool.Contains(mono))
                    {
                        activeTypePool.Remove(mono);
                        mono.gameObject.SetActive(false);
                        mono.transform.SetParent(transform);
                        passiveTypePoo.Add(mono);
                    }
                }
                list.Clear();
            }

            if (passivePool.TryGetValue(type,out var passiveTypePool))
            {
                while (passiveTypePool.Count>POOL_CLEAN_SIZE)
                {
                    var monob = passiveTypePool.Last();
                    passiveTypePool.RemoveAt(passiveTypePool.Count-1);
                    GameObject.Destroy(monob.gameObject);
                }
            }
        }

        public bool AnyInPassive<T>()
        {
            var type = typeof(T);
            return passivePool.TryGetValue(type, out var passiveTypePool) && passiveTypePool.Any();
        }
    
        public void Destroy<T>(T poolObject) where T: MonoBehaviour
        {
            var type = typeof(T);
            Destroy((MonoBehaviour)poolObject,type);
        }

        public void Destroy(MonoBehaviour poolObject, Type type)
        {
            if (activePool.TryGetValue(type,out var activeTypePool))
            {
                var mono = poolObject;
                activeTypePool.Remove(mono);
                mono.gameObject.SetActive(false);
                if(mono.transform.parent!=transform) 
                    mono.transform.SetParent(transform);
                passivePool[type].Add(mono);
                return;
            }
            GameObject.Destroy(poolObject.gameObject);
            Debug.LogWarning("PoolManager.Destroy(): pool not contains poolObject. poolObject directly destroyed!");
        }

        public void PrePool<T>(GameObject prefab,int count,Transform parent=null , UnityAction<T,int> action=null) where T:MonoBehaviour
        {
        
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                var poolObject = Instantiate<T>(prefab);
                if(parent!=null) poolObject.transform.SetParent(parent);
                if(action!=null) action.Invoke(poolObject.GetComponent<T>(),i);
                list.Add(poolObject);
            }
            for (var i = 0; i < count; i++)
            {
                Destroy<T>(list[i]);
            }
            list.Clear();
            list = null;
        }
    
        [Button]
        public void CleanGhostsInPools()
        {
            Debug.Log("PoolManager.CleanGhostsInPools: Cleaning pool STARTED!!");

            foreach (var keyValuePair in activePool)
            {
                var objPool = keyValuePair.Value;
                var willDelete=new List<MonoBehaviour>();
                foreach (var poolObj in objPool)
                {
                    if(!poolObj)
                        willDelete.Add(poolObj);
                }
                foreach (var deleteObj in willDelete)
                {
                    objPool.Remove(deleteObj);
                    Debug.Log("PoolManager.CleanGhostsInPools: Cleaned in activePool");
                }
                willDelete.Clear();
                willDelete = null;
            }
            foreach (var keyValuePair in passivePool)
            {
                var objPool = keyValuePair.Value;
                var willDelete=new List<MonoBehaviour>();
                foreach (var poolObj in objPool)
                {
                    if(!poolObj)
                        willDelete.Add(poolObj);
                }
                foreach (var deleteObj in willDelete)
                {
                    objPool.Remove(deleteObj);
                    Debug.Log("PoolManager.CleanGhostsInPools: Cleaned in passivePool");
                }
                willDelete.Clear();
                willDelete = null;
            }
            Debug.Log("PoolManager.CleanGhostsInPools: Cleaning pool FINISHED!!");

        }
    }
}