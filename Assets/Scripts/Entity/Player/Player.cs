using System.Linq;
using DG.Tweening;
using UnityEngine;
public class Player : LivingEntity
{
    private Color[] _originalColors;
    private static readonly int EmissionColorCache = Shader.PropertyToID("_EmissionColor");

    private Color[] EmissionColors
    {
        get => meshRenderers.Select(x => x.material.GetColor(EmissionColorCache)).ToArray();
        set
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material.SetColor(EmissionColorCache, value[i]);
                meshRenderers[i].material.EnableKeyword("_EMISSION"); // 이걸 해줘야 업데이트가 적용됨
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _originalColors = EmissionColors;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(new Attack(1, this));
        }
    }

    private Tween _hitFeedbackTween;

    public override void HitFeedback(Attack attack)
    {
        EmissionColors = _originalColors.Select(_ => new Color(4, 4, 4)).ToArray();

        _hitFeedbackTween?.Kill();
        _hitFeedbackTween = DOTween.To(() => EmissionColors[0],
            x => EmissionColors = _originalColors.Select(_ => x).ToArray(), _originalColors[0], 0.5f);
    }

    public Player() : base(EntityType.Player)
    {
    }
}