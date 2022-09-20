using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace F13StandardUtils.Scripts.Core
{
    public static class Utils
    {
        public static float CalculateThrowUpYPosition(float ratio,float maxYPosition)
        {
            float y = 0f;
            if (ratio<0.5f)
            {
                    
                var degree = ratio * 2 * 90;
                y = Mathf.Sin(degree*Mathf.Deg2Rad) * maxYPosition;
            }
            else
            {
                var degree = (1-ratio) * 2 * 90;
                y = Mathf.Sin(degree*Mathf.Deg2Rad) * maxYPosition;
            }

            return y;
        }
        
        public static bool RandomBool() => UnityEngine.Random.Range(0, 2) == 0;
        public static bool RandomBool(float trueRatio) => UnityEngine.Random.Range(0f, 1f) <= trueRatio;

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList(); 
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static T MinItem<T>(this IEnumerable<T> enumerable,Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0) 
                return default(T);
            else
            {
                var min = list[0];
                for(int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(min)) < 0)
                    {
                        min = list[i];
                    }
                }
                return min;
            }
        }
        public static T MaxItem<T>(this IEnumerable<T> enumerable,Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0) 
                return default(T);
            else
            {
                var max = list[0];
                for(int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(max)) > 0)
                    {
                        max = list[i];
                    }
                }
                return max;
            }
        }
        
        public static int MinItemIndex<T>(this IEnumerable<T> enumerable,Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0) 
                return -1;
            else
            {
                var minIndex = 0;
                for(int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(list[minIndex])) < 0)
                    {
                        minIndex = i;
                    }
                }
                return minIndex;
            }
        }
        
        public static int MaxItemIndex<T>(this IEnumerable<T> enumerable,Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0)
                return -1;
            else
            {
                var maxIndex = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(list[maxIndex])) > 0)
                    {
                        maxIndex = i;
                    }
                }

                return maxIndex;
            }
        }
        
        public static T RandomEnum<T>() where T:Enum
        {
            var enums = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(0, enums.Length);
            return (T) enums.GetValue(random);
        }
        
        public static Vector3 RandomNormalizedVector(bool randX=true,bool randY=true,bool randZ=true)
        {
            var randVec = new Vector3(
                randX?UnityEngine.Random.Range(-1f, 1f):0f,
                randY?UnityEngine.Random.Range(-1f, 1f):0f,
                randZ?UnityEngine.Random.Range(-1f, 1f):0f);
            return randVec.normalized;
        }
        
        public static List<T> Clamp<T>(this List<T> list, T min, T max) where T : IComparable<T>
        {
            for (var i = 0; i < list.Count; i++)
            {
                var val = list[i];
                if (val.CompareTo(min) < 0) val= min;
                else if(val.CompareTo(max) > 0) val= max;

                list[i] = val;
            }
            return list;
        }
        
        public static List<T> FromJson<T>(string key , string json)
        {
            var o = JObject.Parse(json);
            var array = (JArray)o[key];
            var list=array.ToObject<List<T>>();
            return list;
        }
        
        public static string ToJson<T>(string key , List<T> list)
        {
            JArray array = (JArray)JToken.FromObject(list);
            JObject o = new JObject();
            o[key] = array;
            return o.ToString(Formatting.None);
        }
        
        public static byte[] ToByteArray<T>(T obj)
        {
            if(obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if(data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        

        public static string Encrypt<T>(string key, T willEncrypt)
        {
            byte[] data = ToByteArray<T>(willEncrypt);
            var encrypted= Encrypt(key, data);
            return encrypted;
        }
        
        public static T Decrypt<T>(string key, string encrypted)
        {
            var data = Decrypt(key, encrypted);
            var decrypted=FromByteArray<T>(data);
            return decrypted;
        }

        public static string Encrypt(string key,byte[] data)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        public static byte[] Decrypt(string key, string encrypted)
        {
            byte[] data = Convert.FromBase64String(encrypted);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return results;
                }
            }
        }

        public static bool ScreenToWorldPlanePoint(this Camera camera, Vector3 screenPoint, out Vector3 worldPlanePoint,
            float planeY = 0f)
        {
            var plane = new Plane(Vector3.up, new Vector3(0, planeY, 0));
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            if (plane.Raycast(ray, out float distance))
            {
                worldPlanePoint = ray.GetPoint(distance);
                return true;
            }
            worldPlanePoint = Vector3.zero;
            return false;
        }

        public static bool IsThereAnyUIObject(this EventSystem eventSystem,Vector2 screenPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        
       public static Coroutine StartWaitForSecondCoroutine(this MonoBehaviour monob,float seconds, Action action)
        {			
            if (action == null)
            {
                throw new ArgumentNullException(nameof(Utils)+"."+nameof(StartWaitForSecondCoroutine)+"() "+nameof(action)+" is null");
            }
            return monob.StartCoroutine(WaitForSecondCoroutine(seconds, action));
        }
       public static Coroutine StartWaitForEndOfFrameCoroutine(this MonoBehaviour monob, Action action)
       {			
           if (action == null)
           {
               throw new ArgumentNullException(nameof(Utils)+"."+nameof(StartWaitForEndOfFrameCoroutine)+"() "+nameof(action)+" is null");
           }
           return monob.StartCoroutine(WaitForEndOfFrameCoroutine(action));
       }
       public static Coroutine StartWaitUntilCoroutine(this MonoBehaviour monob,Func<bool> predicate, Action action)
       {			
           if (action == null)
           {
               throw new ArgumentNullException(nameof(Utils)+"."+nameof(StartWaitUntilCoroutine)+"() "+nameof(action)+" is null");
           }
           return monob.StartCoroutine(WaitUntilCoroutine(predicate,action));
       }
       public static Coroutine StartWhileCoroutine(this MonoBehaviour monob,Func<bool> predicate, Action action)
       {			
           if (action == null)
           {
               throw new ArgumentNullException(nameof(Utils)+"."+nameof(StartWhileCoroutine)+"() "+nameof(action)+" is null");
           }
           return monob.StartCoroutine(WaitWhileCoroutine(predicate,action));
       }
       
       
       private static IEnumerator WaitForSecondCoroutine(float seconds, Action action)
       {
           yield return new WaitForSeconds(seconds);
           action.Invoke();
       }
       
       private static IEnumerator WaitForEndOfFrameCoroutine(Action action)
       {
           yield return new WaitForEndOfFrame();
           action?.Invoke();
       }
       private static IEnumerator WaitUntilCoroutine(Func<bool> predicate,Action action)
       {
           yield return new WaitUntil(predicate);
           action?.Invoke();
       }
       private static IEnumerator WaitWhileCoroutine(Func<bool> predicate,Action action)
       {
           yield return new WaitWhile(predicate);
           action?.Invoke();
       }
       
       
        
    }
}