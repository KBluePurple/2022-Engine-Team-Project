using System;
using AchromaticDev.Util;
using UnityEngine;

public enum GameState
{
    Play,
    Pause,
}

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private Player player;
    
        private float _time;
        private int _level;
        private bool _paused;
        
        public EventHandler<GameState> OnGameStateChanged;

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
                    OnGameStateChanged.Invoke(this, GameState.Pause);
                }
                else
                {
                    _paused = false;
                    OnGameStateChanged.Invoke(this, GameState.Play);
                }
            }
        }
    }
}