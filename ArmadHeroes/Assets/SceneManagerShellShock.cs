using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class SceneManagerShellShock : MonoBehaviour
    {

        private static SceneManagerShellShock singleton;
        public static SceneManagerShellShock Singleton() { return singleton; }

        public RewiredController[] players;


        void Awake()
        {
            singleton = this;
            OnGameBegin();
        }
        // Use this for initialization
        void Start()
        {
            ArmadHeroes.CanvasManager.instance.init();
           // OnGameBegin();
        }

        void OnGameBegin()
        {
           // Debug.Log("GAME BEGUN");
            players = FindObjectsOfType<RewiredController>();

            for (int i = 0; i < players.Length; ++i)
            {
                players[i].SetPlayerID(i);
            }
        }


    }
}
