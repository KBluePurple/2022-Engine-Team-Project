using AchromaticDev.Util;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Camera mainCamera;
        public Player player;
    }
}