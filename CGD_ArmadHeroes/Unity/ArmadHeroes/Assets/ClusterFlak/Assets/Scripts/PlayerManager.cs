using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;
using UnityEngine.SceneManagement;

//Player management code taken from SumoManager/Sumo
namespace ArmadHeroes
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject PlayerPrefabHere;
        public List<GameObject> Players = new List<GameObject>();   

        // Use this for initialization
        void Start()
        {
            CanvasManager.instance.init();

            //Spawns selected players
            for (int i = 0; i < 8; i++)
            {
                if (GlobalPlayerManager.instance.playerData[i].activePlayer)
                {
                    float locationX = Random.Range(-13.23f,13.46f);
                    float locationy = Random.Range(-4.75f, -7.42f);

                    //Player instantiation 
                    //
                    //For each active player, instantiate player prefab, set up player
                    GameObject newPlayer = (GameObject)Instantiate(PlayerPrefabHere, new Vector3(locationX, locationy, 0f), Quaternion.identity);
                    string characterName = CharacterProfiles.instance.TypeToString(GlobalPlayerManager.instance.GetPlayerData(i).character);
                    newPlayer.GetComponent<CF_Actor>().Init(i, GlobalPlayerManager.instance.playerData[i].controllerIndex, characterName);
                    Players.Add(newPlayer);
                }
                
            }

            enablePlayers();
            //disablePlayers();

            if (GameObject.FindGameObjectsWithTag("ClusterFlak/Player").Length > 0)
            {
                Players.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void addPlayer(GameObject player)
        {
            Players.Add(player);
        }

        void enablePlayers()
        {
                for (int i = 0; i < Players.Count; i++)
                {
                Players[i].GetComponent<CF_Actor>().enabled = true;
                }
        }
        public void disablePlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].gameObject.SetActive(false);
            }
        }
    }
}
