using UnityEngine;
using UnityEngine.Events;

namespace KBluePurple.Util
{
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector] public Pool pool;
        public UnityEvent onSpawn;
        public UnityEvent onDespawn;
    }
}