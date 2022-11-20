using Unity.VisualScripting;
using UnityEngine;

public record Attack(int Damage);

[RequireComponent(typeof(MeshRenderer))]
public abstract class Entity : MonoBehaviour, IHitFeedback
{
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDead { get; private set; }
    
    private MeshRenderer _meshRenderer;

    public void TakeAttack(Attack attack)
    {
        if (this is IDamageable damageable)
        {
            damageable.TakeDamage(attack.Damage);
        }

        if (this is IHitFeedback hitFeedback)
        {
            hitFeedback.HitFeedback(attack);
        }
    }

    public virtual void HitFeedback(Attack attack)
    {
        
    }
}
