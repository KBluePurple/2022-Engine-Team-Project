using System;
using DG.Tweening;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmVcam;
    private CinemachineTransposer cmTrans;
    
    [Header("Pause")]
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup startMenu;
    
    [Header("Settings")]
    [SerializeField] private CanvasGroup settingsMenu;

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

    [SerializeField] private TMP_Text nowScoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        cmTrans = cmVcam.GetCinemachineComponent<CinemachineTransposer>();
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.alpha = 0;
        GameManager.Instance.OnPauseStateChanged += HandlePauseStateChanged;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        GameManager.Instance.OnGameOver += HandleGameOver;
        GameManager.Instance.OnRestart += HandleRestart;
        GameManager.Instance.OnScoreUpdate -= ScoreUpdate;
        GameManager.Instance.OnScoreUpdate += ScoreUpdate;
    }

    private void HandlePauseStateChanged(object sender, PauseState e)
    {
        switch (e)
        {
            case PauseState.Pause:
                OnPause();
                break;
            case PauseState.Play:
                OnPlay();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }
    }

    private void HandleGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Title:

                break;
            case GameState.InGame:
                OnStart();
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
        sequence.Join(pauseMenu.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
        sequence.Play();
    }

    private void HandleRestart(object sender, EventArgs e)
    {
        fadeScreen.DOFade(1, 1f).From(0).SetUpdate(true);
    }

    private void OnStart() // 게임 시작할 때
    {
        var sequence = DOTween.Sequence()
            .Append(startMenu.DOFade(0, 1.5f))
            .Join(startMenu.transform.DOScale(0, 1.5f).From(1))
            .AppendCallback(() => startMenu.gameObject.SetActive(false))
            .SetUpdate(true)
            .Play();

        var camMoveSeq = DOTween.Sequence()
            .Append(DOTween.To(() => cmTrans.m_FollowOffset.y, x => cmTrans.m_FollowOffset.y = x, 8, 2f))
            .SetUpdate(true)
            .Play()
            .OnComplete(() =>
            {
                StartCoroutine(ScoreCheck());
            });
    }

    IEnumerator ScoreCheck()
    {
        while (true)
        {
            if (GameManager.Instance.isGameEnd)
                break;
            GameManager.Instance.score++;
            ScoreUpdate();
            yield return new WaitForSeconds(1f);
        }
    }

    private void ScoreUpdate()
    {
        nowScoreText.text = $"현재 {(int)GameManager.Instance.score}점";
    }
    
    public void OpenSettings()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => settingsMenu.gameObject.SetActive(true));
        sequence.Append(settingsMenu.DOFade(1, 0.2f));
        sequence.Join(settingsMenu.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
        sequence.Play();
    }
    
    public void CloseSettings()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(settingsMenu.DOFade(0, 0.2f));
        sequence.Join(settingsMenu.transform.DOScale(0, 0.2f).From(1));
        sequence.AppendCallback(() => settingsMenu.gameObject.SetActive(false));
        sequence.SetUpdate(true);
        sequence.Play();
    }
}