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
            Time.timeScale = 1;
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            GameManager.Instance.OnGameOver += OnGameOver;
        }

        private static void OnGamePaused()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.5f).SetUpdate(true);
        }

        private static void OnGameResumed()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.5f).SetUpdate(true);
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            switch (e)
            {
                case GameState.Pause:
                    OnGamePaused();
                    break;
                case GameState.Play:
                    OnGameResumed();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
        
        private void OnGameOver(object sender, EventArgs e)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 1f).SetUpdate(true);
        }
    }
}