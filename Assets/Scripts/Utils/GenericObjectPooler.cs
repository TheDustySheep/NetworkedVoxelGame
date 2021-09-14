using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public abstract class GenericObjectPooler<T> : Singleton<GenericObjectPooler<T>> where T : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        private Queue<T> objects = new Queue<T>();

        public virtual T Get()
        {
            //if (objects.Count == 0)
            //    AddObject();
            //
            //var obj = objects.Dequeue();
            //obj.gameObject.SetActive(true);
            //return obj;
            
            return Instantiate(prefab, transform).GetComponent<T>();
        }

        public void ReturnToPool(T obj)
        {
            Destroy(obj.gameObject);
            //obj.transform.SetParent(transform);
            //obj.gameObject.SetActive(false);      
            //objects.Enqueue(obj);
        }

        protected void AddObject()
        {
            //var go = Instantiate(prefab, transform);
            //go.SetActive(false);
            //objects.Enqueue(go.GetComponent<T>());
        }
    }
}