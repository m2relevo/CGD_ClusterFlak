using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol
{
    public class PlayerManager : MonoBehaviour
    {
        struct PlayerData
        {
            public int score;
            public float sponge;
            public float camper;
            public float runner;
            public float rambo;
            public float unique;
        }
        private static PlayerManager instance;
        public static PlayerManager getInstance() { return instance; }

        public GameObject playerPrefab;
        public List<Color> colours = new List<Color>();

        private List<GameObject> players = null;
        private GameObject[] spawnPoints;
        private List<GameObject> spawnPointsCopy;
        private PlayerData[] playerData;

        //holds the list of spawn points used
        private int spawnHolderElement;

        void OnLevelWasLoaded(int level)
        {
            updateSpawnList();
            spawnPlayers();
            loadPlayerData();
        }

        void Awake()
        {
            instance = this;
            players = new List<GameObject>();
            playerData = new PlayerData[8];
            //assign colors to all players
            colours.Add(Color.blue);
            colours.Add(Color.red);
            colours.Add(Color.yellow);
            colours.Add(Color.green);
            colours.Add(Color.blue);
            colours.Add(Color.red);
            colours.Add(Color.yellow);
            colours.Add(Color.green);
        }

        void Start()
        {
            if (!GlobalPlayerManager.instance)
            {
                return;
            }
        }

        public void savePlayerData()
        {
            for (int playerId = 0; playerId < players.Count; playerId++)
            {
                Player player = players[playerId].GetComponent<Player>();

                playerData[playerId].score = player.getChevronScore();
                playerData[playerId].sponge = player.getTimesShot();
                playerData[playerId].camper = player.getDistanceWalked();
                playerData[playerId].runner = player.getDistanceWalked();
                playerData[playerId].rambo = player.getShotsFired();
                playerData[playerId].unique = player.getZonesCaptured();
            }
        }

        public void loadPlayerData()
        {
            for (int playerId = 0; playerId < getNumberOfPlayers(); playerId++)
            {
                Player player = players[playerId].GetComponent<Player>();
                PlayerData data = playerData[playerId];
                player.updateData(data.score, data.sponge, data.camper, data.runner, data.rambo, data.unique);
            }
        }

        public void updateSpawnList()
        {
            GameObject spawnList = GameObject.Find("SpawnList");
            spawnPointsCopy = new List<GameObject>();

            foreach(GameObject obj in spawnList.GetComponent<SpawnManager>().spawnsHolder[getNumberOfPlayers()-1].spawns)
            {
                spawnPointsCopy.Add(obj);
            }
        }

        public void spawnPlayers()
        {
            players = new List<GameObject>();
            for (int playerId = 0; playerId < GlobalPlayerManager.instance.playerData.Length; playerId++)
            {
                // Check if the player is active
                if (GlobalPlayerManager.instance.playerData[playerId].activePlayer)
                {
                    addPlayer(playerId);
                }
            }
        }

        public int getNumberOfPlayers()
        {
            int numOfPlayers = 0;
            for (int playerId = 0; playerId < GlobalPlayerManager.instance.playerData.Length; playerId++)
            {
                // Check if the player is active
                if (GlobalPlayerManager.instance.playerData[playerId].activePlayer)
                {
                    numOfPlayers++;
                }
            }
            return numOfPlayers;
        }

        public List<GameObject> getPlayersList()
        {
            return players;
        }

        public GameObject getPlayerById(int id)
        {
            return players[id];
        }

        public void addPlayer(int playerId)
        {
            GameObject playerObject = Instantiate(playerPrefab) as GameObject;

            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();

                //select random spawn location
                int spawnElement = Random.Range(0, spawnPointsCopy.Count-1);
                player.spawnPoint = spawnPointsCopy[spawnElement].gameObject.transform.position;
                spawnPointsCopy.RemoveAt(spawnElement);
                playerObject.transform.position = player.spawnPoint;
                player.color = colours[playerId]; // assign a color to the player
                player.Init(playerId, GlobalPlayerManager.instance.playerData[playerId].controllerIndex, CharacterProfiles.instance.GetProfile(GlobalPlayerManager.instance.playerData[playerId].character).characterName);

                players.Add(playerObject);
            }
        }
    }
}