using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public class Indexer<T> where T : ScriptableObject
    {
        protected Dictionary<string, int> stringIndex = new Dictionary<string, int>();
        protected Dictionary<int, T> index = new Dictionary<int, T>();

        public int LoadedCount => index.Count;

        public virtual void UpdateIndex(string filePath)
        {
            index.Clear();

            var objects = Resources.LoadAll<T>(filePath);

            foreach (var item in objects)
            {
                if (stringIndex.ContainsKey(item.name))
                    Debug.LogWarning($"Duplicate key found {item.name}", item);
                else
                {
                    int hash = item.name.GetStableHashCode();
                    stringIndex.Add(item.name, hash);
                    index.Add(hash, item);
                }
            }
        }

        public T GetItem(int hashKey)
        {
            if (!index.TryGetValue(hashKey, out T value))
                Debug.LogWarning($"Hash not found {hashKey}, returning default");

            return value;
        }

        public int GetHash(ScriptableObject so)
        {
            return GetHash(so.name);
        }

        public int GetHash(string key)
        {
            if (stringIndex.TryGetValue(key, out var hash))
            {
                return hash;
            }
            else
            {
                return key.GetStableHashCode();
            }
        }
    }
}