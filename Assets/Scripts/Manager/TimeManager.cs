using System;
using AchromaticDev.Util;
using DG.Tweening;
using UnityEngine;

namespace Manager
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private void Start()
        {
            GameManager.Instance.OnGamePaused += OnGamePaused;
            GameManager.Instance.OnGameResumed += OnGameResumed;
        }

        private void OnGamePaused(object sender, EventArgs e)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.5f).SetUpdate(true);
        }
        
        private void OnGameResumed(object sender, EventArgs e)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.5f).SetUpdate(true);
        }
    }
}