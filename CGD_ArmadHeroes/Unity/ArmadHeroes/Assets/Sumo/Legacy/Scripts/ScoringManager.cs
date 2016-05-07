using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace sumo
{
    public class ScoringManager : MonoBehaviour {
                
        public GameObject[] lifeDisplays;
        public GameObject rankingDisplay;
        List<GameObject> Players = new List<GameObject>();
        Stack<int> Rankings = new Stack<int>();

        // Use this for initialization
        void Start()
        {
            foreach (GameObject display in lifeDisplays)
            {
                display.SetActive(false);
            }
            Players.AddRange(GameObject.FindGameObjectsWithTag("Sumo/Player"));
            for (int i = 0; i < Players.Count; i++)
            {
                lifeDisplays[i].SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

            //Just debug stuff for testing lives.
            /* if (Input.GetMouseButtonDown(0))
             {
                 Players[0].GetComponent<PlayerLives>().LoseLife();
                 Debug.Log((Players[0].name));

             }*/

            displayLives();

        }

        public void addLoser(int playerNo, GameObject loser)
        {
            Rankings.Push(playerNo);
            int loserIndex = Players.IndexOf(loser);
            displayLives();
            //Players.RemoveAt(loserIndex);
            lifeDisplays[loserIndex].GetComponentsInChildren<Image>()[0].enabled = false;
            displayRankings();
        }


        void displayLives()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].GetComponent<sumo_PlayerLives>().lives == 2)
                {
                    lifeDisplays[i].GetComponentsInChildren<Image>()[2].enabled = false;
                }
                if (Players[i].GetComponent<sumo_PlayerLives>().lives == 1)
                {
                    lifeDisplays[i].GetComponentsInChildren<Image>()[1].enabled = false;
                }
                if (Players[i].GetComponent<sumo_PlayerLives>().lives == 0)
                {
                    lifeDisplays[i].GetComponentsInChildren<Image>()[0].enabled = false;
                }

            }

        }

        void displayRankings()
        {
            rankingDisplay.GetComponent<Text>().text += Rankings.Pop();
        }
    }
}
