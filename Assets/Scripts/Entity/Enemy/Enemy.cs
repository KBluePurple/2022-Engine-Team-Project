public class Enemy : LivingEntity
{
    public Enemy() : base(EntityType.Enemy)
    {
    }

    public override void HitFeedback(Attack attack)
    {
    }
}