using DG.Tweening;
using UnityEngine;

public class Player : LivingEntity
{
    private Color _originalColor;
    private static readonly int EmissionColorCache = Shader.PropertyToID("_EmissionColor");

    private Color EmissionColor
    {
        get => meshRenderer.material.GetColor(EmissionColorCache);
        set => meshRenderer.material.SetColor(EmissionColorCache, value);
    }

    private void Await()
    {
        _originalColor = EmissionColor;
    }

    public override void HitFeedback(Attack attack)
    {
        EmissionColor = new Color(4, 4, 4);
        DOTween.To(() => EmissionColor, x => EmissionColor = x, _originalColor, 0.5f);
    }

    public Player() : base(EntityType.Player)
    {
    }
}