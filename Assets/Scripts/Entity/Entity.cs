using AchromaticDev.Util.Pooling;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PoolObject))]
public abstract class Entity : MonoBehaviour
{
    public EntityType Type { get; init; }
    public string Id { get; private set; }

    private PoolObject _poolObject;
    private bool _isInitialized;
    
    protected Entity(EntityType type)
    {
        Type = type;
    }
    
    public void Initialize(string id)
    {
        if (_isInitialized)
        {
            Debug.LogError("Entity is already initialized");
            return;
        }

        Id = id;
        _isInitialized = true;
    }
    
    private void Awake()
    {
        _poolObject = GetComponent<PoolObject>();
        _poolObject.onSpawn.AddListener(OnSpawn);
        _poolObject.onDespawn.AddListener(OnDespawn);
    }

    protected virtual void OnSpawn()
    {
        
    }

    protected virtual void OnDespawn()
    {
        
    }
}