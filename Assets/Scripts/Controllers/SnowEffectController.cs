using UnityEngine;
using UnityEngine.VFX;

namespace Controllers
{
    public class SnowEffectController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private VisualEffect _snowEffect;

        private void Awake()
        {
            _snowEffect = GetComponent<VisualEffect>();
        }

        private void Update()
        {
            _snowEffect.SetVector3("Player Pos", player.position);
        }
    }
}