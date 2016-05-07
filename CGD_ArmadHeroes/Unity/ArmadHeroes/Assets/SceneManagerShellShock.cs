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
            
            for (int i = 0; i < players.Length/*ArmadHeroes.ControllerManager.instance.controllerCount*/; ++i)
            {
                if (ArmadHeroes.GlobalPlayerManager.instance.playerData[i].activePlayer)
                {
                   
                    players[i].Init(i, ArmadHeroes.GlobalPlayerManager.instance.playerData[i].controllerIndex);
                }
                else
                {
                    players[i].gameObject.SetActive(false);
                }

               // players[i].SetPlayerID(ArmadHeroes.GlobalPlayerManager.instance.);
                //call a function that returns the player id based on the relevant controller id
            }
        }


    }
}
