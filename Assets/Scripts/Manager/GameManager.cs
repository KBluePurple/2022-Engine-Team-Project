using System;
using System.Collections;
using AchromaticDev.Util;
using UnityEngine;

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
        [SerializeField] private BulletManager bulletManager; 
    
        private float _time;
        private int _level;
        private bool _paused;

        #region 스코어에 필요한 변수
        public float score = 0;
        public float hightScore = 0;

        private string hashNowScore = "NowScore";
        private string hashHighScore = "HightScore";

        public bool isGameEnd = false;
        #endregion

        public GameState nowGameState = GameState.Title;

        public EventHandler<PauseState> OnPauseStateChanged;
        public EventHandler<GameState> OnGameStateChanged;
        public Action OnScoreUpdate;

        private void Start()
        {
            // json저장이나 할까
            //hightScore = PlayerPrefs.GetInt(hashHighScore);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= 60 * _level)
                player.Skills[_level++].Unlock();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (nowGameState == GameState.Title) return;
                if (_paused == false)
                {
                    _paused = true;
                    OnPauseStateChanged.Invoke(this, PauseState.Pause);
                }
                else
                {
                    _paused = false;
                    OnPauseStateChanged.Invoke(this, PauseState.Play);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (nowGameState != GameState.Title) return;
                nowGameState = GameState.InGame;
                bulletManager.StartSpawn();
                OnGameStateChanged.Invoke(this, GameState.InGame);
            }

            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    OnScoreUpdate?.Invoke();
            //}
        }
    }
}