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
        private DepthOfField _depthOfField;

        private void Start()
        {
            GameManager.Instance.OnPauseStateChanged += HandleGameStateChanged;
            GameManager.Instance.OnGameOver += HandleGameOver;
            volume.profile.TryGet(out _colorAdjustments);
            volume.profile.TryGet(out _depthOfField);
        }

        private void HandleGameStateChanged(object sender, PauseState e)
        {
            switch (e)
            {
                case PauseState.Pause:
                    DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
                        -100, 0.5f).SetUpdate(true);
                    break;
                case PauseState.Play:
                    DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
                        0, 0.5f).SetUpdate(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
        
        private void HandleGameOver(object sender, EventArgs e)
        {
            DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
                -100, 5f).SetUpdate(true);
            DOTween.To(() => _depthOfField.gaussianStart.value, x => _depthOfField.gaussianStart.value = x,
                0, 5f).SetUpdate(true);
        }
    }
}