using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public abstract class PrefabsContainer<T> : ScriptableObject, IPrefabsContainer<T> where T : Enum
    {
        [SerializeField, ArrayElementTitle("key")]
        private PrefabData<T>[] prefabs;

        private Dictionary<T, GameObject> prefabsCache;
        
        private void Awake()
        {
            if (prefabs == null)
                return;
            
            prefabsCache = new Dictionary<T, GameObject>(prefabs.Length);

            foreach (var prefabData in prefabs)
                prefabsCache.Add(prefabData.GetKey(), prefabData.GetPrefab());
        }
        
        public TP GetPrefab<TP>(T prefabKey)
        {
            var prefab = prefabsCache.GetValueOrDefault(prefabKey);
        
            return prefab == null ? default : prefab.GetComponent<TP>();
        }
        
        [Serializable]
        private sealed class PrefabData<TKey> where TKey : Enum
        {
            [SerializeField] 
            private TKey key;
            
            [SerializeField] 
            private GameObject prefab;
        
            public TKey GetKey() 
                => key;
        
            public GameObject GetPrefab() 
                => prefab;
        }
    }
}