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


        //private List<GameObject> players = null;
        private GameObject[] spawnPoints;
        private List<GameObject> spawnPointsCopy;

        private Dictionary<int, GameObject> players;
        private Dictionary<int, PlayerData> playerData;

        //holds the list of spawn points used
        private int spawnHolderElement;
        private bool loadData = false;
        void OnLevelWasLoaded(int level)
        {
            updateSpawnList();
            spawnPlayers();
            if (loadData)
            {
                loadPlayerData();
            }
            else
            {
                loadData = true;
            }
        }

        void Awake()
        {
            instance = this;

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

            playerData = new Dictionary<int, PlayerData>();
            foreach (KeyValuePair<int, GameObject> obj in players)
            {
                Player player = obj.Value.GetComponent<Player>();

                PlayerData data = new PlayerData();
                data.score = player.getChevronScore();
                data.sponge = player.getTimesShot();
                data.camper = player.getDistanceWalked();
                data.runner = player.getDistanceWalked();
                data.rambo = player.getShotsFired();
                data.unique = player.getZonesCaptured();

                playerData.Add(player.playerNumber, data);
            }
        }

        public void loadPlayerData()
        {
            //for (int playerId = 0; playerId < getNumberOfPlayers(); playerId++)
            //{
            if (players.Count > 0)
            {
                foreach (KeyValuePair<int, GameObject> obj in players)
                {
                    Player player = obj.Value.GetComponent<Player>();
                    Debug.Log(player);
                    PlayerData data = playerData[obj.Value.GetComponent<Player>().playerNumber];
                    player.updateData(data.score, data.sponge, data.camper, data.runner, data.rambo, data.unique);
                }
            }
        }

        public void updateSpawnList()
        {
            GameObject spawnList = GameObject.Find("SpawnList");
            spawnPointsCopy = new List<GameObject>();
            if (spawnList != null)
            {
                foreach (GameObject obj in spawnList.GetComponent<SpawnManager>().spawnsHolder[getNumberOfPlayers() - 1].spawns)
                {
                    spawnPointsCopy.Add(obj);
                }
            }
        }

        public void spawnPlayers()
        {
            players = new Dictionary<int, GameObject>();
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

        public Dictionary<int, GameObject> getPlayers()
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
                int spawnElement = Random.Range(0, spawnPointsCopy.Count - 1);
                player.spawnPoint = spawnPointsCopy[spawnElement].gameObject.transform.position;
                spawnPointsCopy.RemoveAt(spawnElement);
                playerObject.transform.position = player.spawnPoint;
                player.color = colours[playerId]; // assign a color to the player
                player.Init(playerId, GlobalPlayerManager.instance.playerData[playerId].controllerIndex, CharacterProfiles.instance.GetProfile(GlobalPlayerManager.instance.playerData[playerId].character).characterName);
                players.Add(playerId, playerObject);
            }
        }
    }
}