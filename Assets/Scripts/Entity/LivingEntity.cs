using Script.Manager;
using UnityEngine;

public record Attack(int Damage, Entity Attacker);

[RequireComponent(typeof(MeshRenderer))]
public abstract class LivingEntity : Entity, IHitFeedback
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    public bool IsDead { get; private set; }

    // ReSharper disable once InconsistentNaming
    protected MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public abstract void HitFeedback(Attack attack);

    public void Damage(Attack attack)
    {
        if (IsDead) return;

        attack = OnDamage(attack);

        Health -= attack.Damage;
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
            OnDeath(attack);
        }
        else
        {
            HitFeedback(attack);
        }
    }

    protected virtual Attack OnDamage(Attack attack)
    {
        return attack;
    }

    protected virtual void OnDeath(Attack attack)
    {
        EntityManager.Instance.DestroyEntity(this);
    }

    protected LivingEntity(EntityType type) : base(type)
    {
    }
}