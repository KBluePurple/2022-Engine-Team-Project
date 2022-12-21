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
        [SerializeField] private Player player;

        [NonSerialized] public float GameTime;
        [SerializeField] private BulletManager bulletManager;

        private float _time;
        private int _level;
        private bool _paused;

        #region 스코어에 필요한 변수

        public float nowScore = 0;
        public float hightScore = 0;

        private string hashNowScore = "NowScore";
        private string hashHighScore = "HightScore";

        #endregion

        public GameState nowGameState = GameState.Title;

        public EventHandler<PauseState> OnPauseStateChanged;
        public EventHandler<GameState> OnGameStateChanged;
        public EventHandler OnGameOver;
        public EventHandler OnRestart;

        public Action OnScoreUpdate;
        public bool _isGameOver;

        private void Start()
        {
            hightScore = PlayerPrefs.GetInt(hashHighScore);
            OnScoreUpdate?.Invoke();
        }

        private void Update()
        {
            GameTime += Time.deltaTime;
            if (GameTime >= 60 * _level)
            {
                try
                {
                    player.Skills[_level++].Unlock();
                }
                catch
                {
                    // ignored
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                if (_paused == false)
                    Pause();
                else
                    Resume();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_isGameOver) Restart();
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
                yield return new WaitForSecondsRealtime(1f);
                Debug.Log("Restart");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                
            _isGameOver = true;
        }
    }
}