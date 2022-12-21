using System;
using AchromaticDev.Util;
using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenu;
    
    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.alpha = 0;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Pause:
                OnPause();
                break;
            case GameState.Play:
                OnPlay();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }
    }

    private void OnPlay()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(pauseMenu.DOFade(0, 0.2f));
        sequence.Join(pauseMenu.transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
        sequence.AppendCallback(() => pauseMenu.gameObject.SetActive(false));
        sequence.SetUpdate(true);
        sequence.Play();
    }

    private void OnPause()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => pauseMenu.gameObject.SetActive(true));
        sequence.Append(pauseMenu.DOFade(1, 0.2f));
        sequence.Join(pauseMenu.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
        sequence.Play();
    }
}