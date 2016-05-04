using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace Astrodillos{
	public class PlayerManager : MonoBehaviour
    {

		//Singleton
		public static PlayerManager instance;


		public GameObject playerPrefab;
		public Transform[] spawnPoints;
		public GameObject UICanvas;
        public bool playersMade = false;
		public List<Actor_Armad> players = new List<Actor_Armad>();
		int alivePlayerCount = 0;

        public Actor_Armad GetPlayer(int callerID)
        {
			for (int i = 0; i < players.Count; i++) {
				if (players [i].playerNumber == callerID) {
					return players [i];
				}
			}

			return null;
        }

		public void SendAllPlayerData(){
			for (int i = 0; i < players.Count; i++) {
				players [i].SendPlayerData ();
			}
		}


		// Use this for initialization
		void Awake ()
        {
			instance = this;


		}



		//Create a player
		public void InitialisePlayer(int _player)
        {

            //Empty player slot so do not spawn

			if(_player>0 && !GlobalPlayerManager.instance.GetPlayerData(_player).activePlayer)
            {
                return;
            }
            
			GameObject newPlayer = Instantiate (playerPrefab);
            newPlayer.SetActive(true);
            newPlayer.transform.SetParent (gameObject.transform);
           	players.Add (newPlayer.GetComponent<Actor_Armad>());

			players [players.Count - 1].Init (GlobalPlayerManager.instance.GetPlayerData(_player).controllerIndex, _player);

			//Set character
			if (GlobalPlayerManager.instance != null) {
				players[players.Count - 1].SetCharacter(GlobalPlayerManager.instance.GetPlayerData(_player).character);
			}


		}

      


		public void SpawnPlayers(){
			for(int i = 0; i<players.Count; i++){
				players[i].gameObject.transform.SetParent(transform);
				players[i].Spawn(spawnPoints[i].position);
			}

			alivePlayerCount = players.Count;
            playersMade = true;
		}

		public void GivePlayersJetpacks(){
			JetpackManager.instance.DestroyPickups ();
			for(int i = 0; i<players.Count; i++){
				//if player doesn't have a jetpack atm
				if(!JetpackManager.instance.HasJetpack(players[i])){
					JetpackManager.instance.SpawnJetpackOnActor(players[i]);
				}

			}
		}

		public void KillPlayer(){
			alivePlayerCount--;

			//If one player alive
			if (alivePlayerCount == 1)
            {
				//Make player win round
				Gametype_Astrodillos.instance.EndRound ();
			} 
		}


		

		public int GetPlayerCount(bool alivePlayersOnly = false){
			if (alivePlayersOnly) 
            {
				return alivePlayerCount;
			}
			return players.Count;
		}

	}
}
