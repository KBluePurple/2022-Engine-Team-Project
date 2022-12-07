using Script.Manager;
using UnityEngine;
using UnityEngine.Events;

public record Attack(int Damage, Entity Attacker);

public abstract class LivingEntity : Entity, IHitFeedback
{
    public UnityEvent OnDeath;
    public UnityEvent OnHit;

    public bool IsDead { get; private set; }

    [field: SerializeField] public int Health { get; private set; } = 100;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;

    // ReSharper disable once InconsistentNaming
    protected MeshRenderer[] meshRenderers;

    protected virtual void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public abstract void HitFeedback(Attack attack);

    public void TakeDamage(Attack attack)
    {
        if (IsDead) return;

        attack = OnDamageHandle(attack);

        Health -= attack.Damage;
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