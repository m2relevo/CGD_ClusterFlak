using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace DilloDash
{
    public class GameStateDD : MonoBehaviour
    {
        private static GameStateDD singleton = null;
        public static GameStateDD Singleton() { return singleton; }

        [System.NonSerialized] public bool isGamePaused = false;
        [System.NonSerialized] public int maxPlayers = 8;

        void Awake()
        {
            singleton = this;
        }

        void Update()
        {
            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    isGamePaused = false;
                    break;
                case GameStates.pause:
                    isGamePaused = true;
                    break;
                case GameStates.gameover:
                    isGamePaused = true;
                    break;
                default:
                    break;
            }
        }
    }
}
