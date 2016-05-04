using UnityEngine;
using System.Collections;

using ArmadHeroes;

namespace Astrodillos{
	public class Gametype_Astrodillos : MonoBehaviour {

		//Singleton
		public static Gametype_Astrodillos instance;

		//Two different settings for the gametype
		public GameObject space;
		public GameObject ground;

		public GameObject[] levels;
		public Collider2D[] dropZones;
		public ParticleSystem explosion;


		public IcbmManager asteroidManager;
		public RocketShip rocketShip;
		public Silo silo;
		public SpaceICBM spaceICBM;
		public HeliIntegration helicopter;
		public AudioClip explosionSfx;

		ParticleSystem.ShapeModule explosionShape;
		float roundTime = 90;
        float gameTime = 0;


		public enum GameState{
			None,
			Ground,
			Space
		}

		GameState gameState;

	    private int roundCount = 3;
		private int currentRound = 0;
		private bool fading = false;
		

		// Use this for initialization
		void Awake () {
			instance = this;
			gameTime = roundTime;
			explosionShape = explosion.shape;
		}

		void Start(){
			//Spawn the number of players for controllers
			int initCount = Mathf.Max (8, ArmadHeroes.ControllerManager.instance.controllerCount);  //This can probably be just 8

			for (int i = 0; i<initCount; i++) {
				PlayerManager.instance.InitialisePlayer(i);
			}
			SetState (GameState.Ground);

            CanvasManager.instance.init();

			
			StartRound ();
		}
		
		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			if (fading) {
				return;
			}


			if (gameTime > 0) {
				gameTime -= Time.deltaTime;
				CanvasManager.instance.updateTime (gameTime);
			} else {
				EndRound ();
			}
		}

	

		#region Round Management

		public void EndRound()
	    {
			if (fading) {
				return;
			}
			fading = true;
			GameManager.instance.FadeToBlack (() => 
            {
				levels [currentRound].SetActive (false);
				currentRound++;
				//start the next round
				if (currentRound < roundCount)
				{

					levels [currentRound].SetActive (true);

					StartRound();
					
				}
				else
				{
					EndGame();
				}

				gameTime = roundTime;
				CanvasManager.instance.updateTime(gameTime);

			});
				
				
			
				
		}


		void EndGame()
        {
			PlayerManager.instance.SendAllPlayerData ();
			UnityEngine.SceneManagement.SceneManager.LoadScene ("DebriefScene");
			SoundManager.instance.StopAllClips();
		}
	   
		void StartRound()
	    {
			gameTime = roundTime;
			CanvasManager.instance.updateTime(gameTime);
			fading = false;
			//Spawn players in
			bool useGravity = (gameState == GameState.Ground);

			PlayerManager.instance.SpawnPlayers ();
			asteroidManager.RemoveAll ();
			KillRemainingCrates();

			
			if (useGravity) {
				helicopter.Reset ();
				JetpackManager.instance.Reset ();
				rocketShip.Reset ();
				silo.Reset ();
			} else {
				spaceICBM.Reset ();
			}
		}
		#endregion


		#region State Management

		//Sets a new game state
		public void SetState(GameState newState){
	        if (gameState != GameState.None)
	        {
	            LeaveState();
	        }

			gameState = newState;

			EnterState ();
		}

		void EnterState(){
			switch (gameState) {
			case GameState.Ground:
				ground.SetActive(true);

				break;
			case GameState.Space:
				PlayerManager.instance.SpawnPlayers ();
               
				PlayerManager.instance.GivePlayersJetpacks ();
				gameTime = roundTime;
				CanvasManager.instance.updateTime(gameTime);
				space.SetActive(true);
	            StartRound();
				break;

			}
		}
        void KillRemainingCrates()
        {
            GameObject[] crates = GameObject.FindGameObjectsWithTag("Astrodillos/crate");
            foreach (GameObject c in crates)
            {
                Destroy(c);
            }
        }
		void LeaveState(){
			switch (gameState) {
			case GameState.Ground:
				ground.SetActive(false);
				break;
			case GameState.Space:
				space.SetActive(false);
				break;
				
			}
		}

		#endregion

		public int GetCurrentLevel()
		{
			return currentRound - 1;
		}

		public Vector2 GetDropPoint(){
			//Try up to 5 times
			if (currentRound > 2) {
				return Vector2.zero;
			}
			for(int j = 0; j<5; j++){
				Vector2 randomPos = new Vector3 (Random.Range (-10, 10), Random.Range (-5, 4));
				if (dropZones [currentRound].OverlapPoint (randomPos) || j==4) {
					return randomPos;
				}
			}
			return Vector2.zero;
		}

		public void Explosion(Vector3 explosionPos, bool playSound = true, float areaOfEffect = 0){
			explosion.gameObject.transform.position = explosionPos;

			explosionShape.radius = areaOfEffect == 0 ? 0.25f : areaOfEffect * 0.75f;
			explosion.Emit ((int)(explosionShape.radius*300));      

			if (playSound) {
				SoundManager.instance.PlayClip (explosionSfx, explosionPos);
			}



			if (areaOfEffect>0) {
				foreach (Actor_Armad actor in PlayerManager.instance.players) {
					float distance = Vector3.Distance (actor.transform.position, explosionPos);
					if (distance < areaOfEffect) {
						actor.TakeDamage (50);
					}
				}
			}

		}


		public bool UseGravity(){
			return gameState == GameState.Ground;
		}


	}
}
