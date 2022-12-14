using System.Collections;
using Script.Manager;
using UnityEngine;
using UnityEngine.Events;

public record Attack(int Damage, Entity Attacker);

public abstract class LivingEntity : Entity, IHitFeedback
{
    public UnityEvent OnDeath;
    public UnityEvent OnHit;
    public bool IsDead { get; private set; }
    public float invulnerabilityTime = 0.5f;
    
    [field: SerializeField] public int Health { get; private set; } = 100;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    
    // ReSharper disable once InconsistentNaming
    protected Renderer[] renderers;
    
    private float _invulnerabilityTimer = 0f;

    protected virtual void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        
        StartCoroutine(InfiniteLoop());
    }

    public abstract void HitFeedback(Attack attack);

    public void TakeDamage(Attack attack)
    {
        if (IsDead) return;
        if (_invulnerabilityTimer > 0f) return;

        attack = OnDamageHandle(attack);

        Health -= attack.Damage;
        invulnerabilityTime = Mathf.Max(invulnerabilityTime, 0f);
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
            OnDeathHandle(attack);
        }
        else
        {
            HitFeedback(attack);
        }
    }

    private IEnumerator InfiniteLoop()
    {
        while (true)
        {
            yield return null;
            if (_invulnerabilityTimer > 0)
            {
                _invulnerabilityTimer -= Time.deltaTime;
            }
        }
    }

    protected virtual Attack OnDamageHandle(Attack attack)
    {
        return attack;
    }

    protected virtual void OnDeathHandle(Attack attack)
    {
        EntityManager.Instance.DestroyEntity(this);
    }

    protected LivingEntity(EntityType type) : base(type)
    {
    }
}