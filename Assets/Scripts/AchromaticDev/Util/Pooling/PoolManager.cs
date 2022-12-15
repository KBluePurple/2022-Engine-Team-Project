using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util;
using UnityEngine;

namespace KBluePurple.Util
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<GameObject, Pool> _prefabDict = new();
        internal readonly Dictionary<GameObject, PoolObject> PoolObjectCache = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (_instance != null) return;

            var go = new GameObject("PoolManager");
            _instance = go.AddComponent<PoolManager>();
            DontDestroyOnLoad(go);
        }

        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.Log("PoolManager: Prefab is null");
                return null;
            }

            if (Instance._prefabDict.ContainsKey(prefab))
                return _instance._prefabDict[prefab].GetObject(prefab, position, rotation, parent);

            Debug.Log("PoolManager: No pool found for " + prefab.name);

            Instance._prefabDict.Add(prefab, ScriptableObject.CreateInstance<Pool>());
            Instance._prefabDict[prefab].prefab = prefab;
            Instance._prefabDict[prefab].Initialize(Instance.transform);

            return Instance._prefabDict[prefab].GetObject(prefab, position, rotation, parent);
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static void Destroy(GameObject gameObject)
        {
            if (!Instance.PoolObjectCache.ContainsKey(gameObject)) return;

            var prefab = Instance.PoolObjectCache[gameObject].pool.prefab;

            if (Instance._prefabDict.ContainsKey(prefab))
            {
                Instance._prefabDict[prefab].ReturnObject(gameObject);
            }
            else
            {
                Debug.LogWarning($"PoolManager: {prefab.name} is not in the pool dictionary.");
                Object.Destroy(gameObject);
            }
        }

        public static void Destroy(GameObject gameObject, float delay)
        {
            Instance.StartCoroutine(ReturnObject(gameObject, delay));
        }

        private static IEnumerator ReturnObject(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}