using System;
using AchromaticDev.Util;
using DG.Tweening;
using UnityEngine;

namespace Manager
{
    public class TimeManager : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnPauseStateChanged += OnGameStateChanged;
        }

        private static void OnGamePaused()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.5f).SetUpdate(true);
        }

        private static void OnGameResumed()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.5f).SetUpdate(true);
        }

        private void OnGameStateChanged(object sender, PauseState e)
        {
            switch (e)
            {
                case PauseState.Pause:
                    OnGamePaused();
                    break;
                case PauseState.Play:
                    OnGameResumed();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
    }
}