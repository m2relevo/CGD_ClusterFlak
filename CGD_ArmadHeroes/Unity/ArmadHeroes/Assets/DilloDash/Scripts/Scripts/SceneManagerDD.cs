using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class SceneManagerDD : MonoBehaviour
    {
        private static SceneManagerDD singleton;
        public static SceneManagerDD Singleton() { return singleton; }

        public bool finalMode = true;

        void Awake()
        {
            singleton = this;
        }

        void Start()
        {
            OnGameBegin();
        }

        void OnGameBegin()
        {
            //Debug split controller - this should happen and be controlled in the menus
            if (!finalMode)
            {
                bool splitControllerMode = GameControllerDD.Singleton().splitControllerMode;
                if (splitControllerMode)
                {
                    int count = ArmadHeroes.ControllerManager.instance.controllerCount;
                    for (int i = 0; i < count; ++i)
                    {
                        ArmadHeroes.Controller controller = ArmadHeroes.ControllerManager.instance.GetController(i);
                        ArmadHeroes.ControllerManager.instance.SplitController(controller);
                    }
                }

                //GameData data = GameData.Singleton();
                //Pass along player data to controller to manager
                for (int i = 0; i < GameControllerDD.Singleton().testPlayers; ++i)
                {
                    //Normal
                    if (!splitControllerMode)
                    {
                        PlayerManagerDD.Singleton().NewPlayer(i + 1, i);
                    }
                    else
                    {
                        //SplitController
                        PlayerManagerDD.Singleton().NewPlayer((i * 2) + 1, i);
                        PlayerManagerDD.Singleton().NewPlayer((i * 2) + 2, i + (GameStateDD.Singleton().maxPlayers / 2));
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8; ++i)
                {
                    PlayerManagerDD.Singleton().NewPlayer(i);
                }
            }

            GameControllerDD.Singleton().Init();
        }

        void OnGameEnd()
        {
            //Pass player data back to gamedata
           // GameData data = GameData.Singleton();
        }
    }
}
