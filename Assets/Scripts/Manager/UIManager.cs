using System;
using AchromaticDev.Util;
using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmVcam;
    private CinemachineTransposer cmTrans;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup startMenu; 
    
    private void Start()
    {
        cmTrans = cmVcam.GetCinemachineComponent<CinemachineTransposer>();
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.alpha = 0;
        GameManager.Instance.OnPauseStateChanged += HandlePauseStateChanged;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
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
                OnTitle();
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

    private void OnPause()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => pauseMenu.gameObject.SetActive(true));
        sequence.Append(pauseMenu.DOFade(1, 0.2f));
        sequence.Join(pauseMenu.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
        sequence.Play();
    }

    private void OnTitle() // 타이틀 화면으로 왔을 때
    {
        var sequence = DOTween.Sequence()
            .AppendCallback(() => startMenu.gameObject.SetActive(true))
            .Append(startMenu.DOFade(1, 1.5f))
            .Join(startMenu.transform.DOScale(1, 1.5f).SetEase(Ease.OutBack))
            .SetUpdate(true)
            .Play();

        var camMoveSeq = DOTween.Sequence()
            .SetUpdate(true)
            .Append(DOTween.To(() => cmTrans.m_FollowOffset.y, x => cmTrans.m_FollowOffset.y = x, 2, 2f))
            .Play();
    }

    private void OnStart() // 게임 시작할 때
    {
        var sequence = DOTween.Sequence()
            .Append(startMenu.DOFade(0, 1.5f))
            .Join(startMenu.transform.DOScale(0, 1.5f).SetEase(Ease.InBack))
            .AppendCallback(() => startMenu.gameObject.SetActive(false))
            .SetUpdate(true)
            .Play();

        var camMoveSeq = DOTween.Sequence()
            .Append(DOTween.To(() => cmTrans.m_FollowOffset.y, x => cmTrans.m_FollowOffset.y = x, 8, 2f))
            .SetUpdate(true)
            .Play();
    }
}