using UnityEngine;
using UnityEngine.Events;

namespace AchromaticDev.Util.Pooling
{
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector] public Pool pool;
        public UnityEvent onSpawn;
        public UnityEvent onDespawn;
    }
}