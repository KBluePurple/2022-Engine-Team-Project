using System;
using AchromaticDev.Util;
using UnityEngine;
using UnityEngine.Serialization;

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

        [NonSerialized] public float PlayTime;
        private int _level;
        private bool _paused;

        public EventHandler<GameState> OnGameStateChanged;

        private void Update()
        {
            PlayTime += Time.deltaTime;
            if (PlayTime >= 60 * _level)
                player.Skills[_level++].Unlock();

            if (Input.GetKeyDown(KeyCode.Escape))
                if (_paused == false)
                    Pause();
                else
                    Resume();
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
    }
}