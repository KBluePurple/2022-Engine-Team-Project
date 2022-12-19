using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Manager
{
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        private ColorAdjustments _colorAdjustments;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            volume.profile.TryGet(out _colorAdjustments);
        }

        private void HandleGameStateChanged(object sender, GameState e)
        {
            switch (e)
            {
                case GameState.Pause:
                    DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
                        -100, 0.5f).SetUpdate(true);
                    break;
                case GameState.Play:
                    DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
                        0, 0.5f).SetUpdate(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
    }
}