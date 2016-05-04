using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Collections;

namespace ShellShock
{
    public class CheckPlayer : MonoBehaviour
    {

        public int playerId;
        private PlayerLogic player;

        //GameObject[] playerInstance;
        public GameObject prefab;

        bool[] playerJoined;
        bool[] playerReady;

        public Text[] textCurrent;
        string[] textPlayerJoin;
        string[] textPlayerReady;
        string[] textPlayerFinal;

        GameObject moviePlay;

        public Vector2[] playerPositions;


        void Start()
        {

            //player = ReInput.players.GetPlayer(playerId);
            //playerInstance = new GameObject[4];

            textPlayerJoin = new string[4];
            textPlayerReady = new string[4];
            textPlayerFinal = new string[4];

            playerJoined = new bool[4];
            playerReady = new bool[4];

            for (int i = 0; i < 4; ++i)
            {
                textPlayerJoin[i] = "Press START To Join";
                textPlayerReady[i] = "Press START To Ready Up";
                textPlayerFinal[i] = "Player" + (i + 1) + " Ready";

                playerJoined[i] = false;
                playerReady[i] = false;

                textCurrent[i].text = textPlayerJoin[i];
            }
        }

        void Update()
        {

        }
    }
}
