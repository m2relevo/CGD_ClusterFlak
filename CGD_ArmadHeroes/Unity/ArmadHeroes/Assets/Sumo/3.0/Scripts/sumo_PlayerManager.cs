//Armad Heros 
//SUMO 
//Gareth Griffiths, Peter Maloney, Alex Nuns, Jake Downing
//Player Manager
//
//Manages player instances and player join

using UnityEngine;
using System.Collections.Generic;

public class sumo_PlayerManager : MonoBehaviour
{

    List<GameObject> playerList;
    public GameObject playerPrefab;
    int numberOfPlayers = 4;
    bool playersReady;

	// Use this for initializatio
	void Start ()
    {
        playerList = new List<GameObject>(numberOfPlayers);
        playersReady = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(playerList.Count);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) == true && playerList.Count>0)
        {
            playersReady = true;
        }

	}

    void OnGUI()
    {

        if (playersReady == true)
        {
            //Start coundown 
        }

        else //Show Join menu, On click add player gameobject to playerList
        {
            if (GUI.Button(new Rect(200, 10, 100, 50), "Player1 Join"))
            {
                Debug.Log("Player1 joined");
                playerPrefab.name = "Player1";
                playerAdd(playerPrefab);
            }

            if (GUI.Button(new Rect(400, 10, 100, 50), "Player2 Join"))
            {
                Debug.Log("Player2 joined");
                playerPrefab.name = "Player2";
                playerAdd(playerPrefab);
            }

            if (GUI.Button(new Rect(600, 10, 100, 50), "Player3 Join"))
            {
                Debug.Log("Player3 joined");
                playerPrefab.name = "Player3";
                playerAdd(playerPrefab);
            }

            if (GUI.Button(new Rect(800, 10, 100, 50), "Player4 Join"))
            {
                Debug.Log("Player4 joined");
                playerPrefab.name = "Player4";
                playerAdd(playerPrefab);
            }
        }
    }

    //Adds a new player to the array
    void playerAdd(GameObject playerObject)
    {
        //Check if smaller than the number of players
        if (playerList.Count< numberOfPlayers)
        {
            playerList.Add(playerObject);
        }
    }

    //May need this later
    //Returns the player array
    List<GameObject> getplayerList()
    {
        return playerList;
    }

}
