using System;
using AchromaticDev.Util;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public EventHandler OnGameStart;
        public EventHandler OnGameEnd;
        public EventHandler OnGamePaused;
        public EventHandler OnGameResumed;

        [SerializeField] private Player player;
    
        private float _time;
        private int _level;
        private bool _paused;
    
        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= 60 * _level)
                player.Skills[_level++].Unlock();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_paused == false)
                {
                    _paused = true;
                    OnGamePaused?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _paused = false;
                    OnGameResumed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}