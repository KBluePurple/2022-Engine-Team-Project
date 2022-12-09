using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AchromaticDev.Util.Pooling
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        internal readonly Dictionary<GameObject, Pool> prefabDict = new();
        internal readonly Dictionary<GameObject, PoolObject> poolObjectCache = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (_instance != null) return;

            var go = new GameObject("PoolManager");
            _instance = go.AddComponent<PoolManager>();
            DontDestroyOnLoad(go);

            var pools = Resources.LoadAll<Pool>("Pools");

            foreach (var poolSetting in pools)
            {
                poolSetting.Initialize(_instance.transform);
                _instance.prefabDict.Add(poolSetting.prefab, poolSetting);
            }
        }

        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.Log("PoolManager: Prefab is null");
                return null;
            }

            if (Instance.prefabDict.ContainsKey(prefab))
                return _instance.prefabDict[prefab].GetObject(prefab, position, rotation, parent);

            Debug.Log("PoolManager: No pool found for " + prefab.name);

            Instance.prefabDict.Add(prefab, ScriptableObject.CreateInstance<Pool>());
            Instance.prefabDict[prefab].prefab = prefab;
            Instance.prefabDict[prefab].Initialize(Instance.transform);

            return Instance.prefabDict[prefab].GetObject(prefab, position, rotation, parent);
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static void Destroy(GameObject gameObject)
        {
            if (!Instance.poolObjectCache.ContainsKey(gameObject)) return;

            var prefab = Instance.poolObjectCache[gameObject].pool.prefab;

            if (Instance.prefabDict.ContainsKey(prefab))
            {
                Instance.prefabDict[prefab].ReturnObject(gameObject);
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