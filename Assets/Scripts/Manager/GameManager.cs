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

       public GameState nowGameState = GameState.Title;

        public EventHandler<PauseState> OnPauseStateChanged;

        public EventHandler<GameState> OnGameStateChanged;
        public EventHandler OnGameOver;
        public EventHandler OnRestart;
        
        private bool _isGameOver;

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
            
            if (Input.GetKeyDown(KeyCode.Return) && _isGameOver)
                Restart();
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
            OnGameStateChanged.Invoke(this, GameState.Pause);
        }

        public void Resume()
        {
            _paused = false;
            OnGameStateChanged.Invoke(this, GameState.Play);
        }

        public void GameOver()
        {
            OnGameOver.Invoke(this, EventArgs.Empty);
            _isGameOver = true;
        }

    }
}