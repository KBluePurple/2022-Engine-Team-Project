using DG.Tweening;
using UnityEngine;

public class Player : LivingEntity
{
    private Color _originalColor;
    private static readonly int EmissionColorCache = Shader.PropertyToID("_EmissionColor");

    private Color EmissionColor
    {
        get => meshRenderer.sharedMaterial.GetColor(EmissionColorCache);
        set
        {
            meshRenderer.sharedMaterial.SetColor(EmissionColorCache, value);
            meshRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // 이걸 해줘야 업데이트가 적용됨
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _originalColor = EmissionColor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(new Attack(0, this));
        }
    }

    private Tween _hitFeedbackTween;

    public override void HitFeedback(Attack attack)
    {
        EmissionColor = new Color(4, 4, 4);

        _hitFeedbackTween?.Kill();
        _hitFeedbackTween = DOTween.To(() => EmissionColor, x => EmissionColor = x, _originalColor, 0.5f);
    }

    public Player() : base(EntityType.Player)
    {
    }
}