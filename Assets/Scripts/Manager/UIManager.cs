using System;
using DG.Tweening;
using Manager;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private CanvasGroup pauseMenu;
    
    [Header("GameOver")]
    [SerializeField] private CanvasGroup gameOverMenu;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeScreen;

    private void Awake()
    {
        Debug.Log("UIManager Awake");
        fadeScreen.DOFade(0, 1f).From(1f).SetUpdate(true);
    }

    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.alpha = 0;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        GameManager.Instance.OnGameOver += HandleGameOver;
        GameManager.Instance.OnRestart += HandleRestart;
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


    private void HandleGameOver(object sender, EventArgs e)
    {
        gameOverMenu.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(gameOverMenu.DOFade(1, 2f))
            .Join(DOTween.To(() => gameOverText.characterSpacing, x => gameOverText.characterSpacing = x, 0, 2f)
                .From(170)).SetEase(Ease.OutCubic)
            .Append(infoText.DOFade(1, 1f))
            .Join(scoreText.DOFade(1, 1f))
            .SetUpdate(true)
            .Play();
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

    private void HandleRestart(object sender, EventArgs e)
    {
        fadeScreen.DOFade(1, 1f).From(0).SetUpdate(true);
    }
}