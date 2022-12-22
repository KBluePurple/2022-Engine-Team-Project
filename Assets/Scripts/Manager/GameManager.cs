using System;
using System.Collections;
using AchromaticDev.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState
{
    Title,
    InGame
}

public enum PauseState
{
    Play,
    Pause,
}

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Player player;

        [NonSerialized] public float GameTime;
        [SerializeField] private BulletManager bulletManager;

        private int _level;
        private bool _paused;

        #region 스코어에 필요한 변수

        public float nowScore = 0;
        public float highScore = 0;

        private string hashHighScore = "HighScore";

        #endregion

        public GameState nowGameState = GameState.Title;

        public EventHandler<PauseState> OnPauseStateChanged;
        public EventHandler<GameState> OnGameStateChanged;
        public EventHandler OnGameOver;
        public EventHandler OnRestart;

        public Action OnScoreUpdate;
        public bool isGameOver;

        private void Start()
        {
            Cursor.visible = false;
            SetHighScore();
            OnScoreUpdate?.Invoke();
        }

        private void Update()
        {
            if (nowGameState == GameState.InGame)
            {
                GameTime += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                if (_paused == false)
                    Pause();
                else
                    Resume();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isGameOver) Restart();
                if (nowGameState != GameState.Title) return;
                nowGameState = GameState.InGame;
                bulletManager.StartSpawn();
                OnGameStateChanged.Invoke(this, GameState.InGame);
            }
        }

        private void Restart()
        {
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                OnRestart?.Invoke(this, EventArgs.Empty);
                yield return new WaitForSecondsRealtime(3f);
                SceneManager.LoadScene("Middle");
                Debug.Log("Restart");
            }
        }

        public void Pause()
        {
            _paused = true;
            OnPauseStateChanged.Invoke(this, PauseState.Pause);
        }

        public void Resume()
        {
            _paused = false;
            OnPauseStateChanged.Invoke(this, PauseState.Play);
        }

        public void GameOver()
        {
            OnGameOver.Invoke(this, EventArgs.Empty);
            if (nowScore > PlayerPrefs.GetInt(hashHighScore))
                 PlayerPrefs.SetInt(hashHighScore, (int)nowScore);
                
            isGameOver = true;
        }
        
        public void Exit()
        {
            Application.Quit();
        }

        public void SetHighScore()
        {
            highScore = PlayerPrefs.GetInt(hashHighScore);

            if (highScore < nowScore)
                 highScore = nowScore;
        }

        [ContextMenu("디버깅")]
        public void SetZero()
        {
            PlayerPrefs.SetInt(hashHighScore, 0);
        }
    }
}