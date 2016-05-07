using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace ArmadHeroes
{
	public enum sumoSounds
	{
		COLLIDE_CLIP = 0,
		EXPLOSION_CLIP,
		ROLLING_CLIP,
		MAX_CLIP_COUNT
	}

	public class SumoManager : MonoBehaviour {


		//Create actor for each joined player
		//Set up round
		//Set up Accolades in use
		//Start round one

		//At end of round
		//Increment player score
		//Reset round

		//If all rounds played
		//Gather and assign accolade values
		//
		public GameObject playerPrefab;
		public sumo.sumo_RoundManager _roundManager;

		public GameObject[] mineRings;
		public bool dropOne, dropTwo;

		public float roundDuration = 60;
		float roundTimer;

		public AudioClip rollingAudio;
		AudioSource rollingSource;

		static private SumoManager m_instance = null;
		static public SumoManager instance { get { return m_instance; } }

		public AudioClip[] sumoSoundList =  new AudioClip[(int)sumoSounds.MAX_CLIP_COUNT];
		public List<AudioSource> sumoSoundSourceList = new List<AudioSource>();


		public bool countDown = false;


		void Awake()
		{  

			if (m_instance == null)
			{
				m_instance = this;
			}

			roundTimer = roundDuration;

		}

		// Use this for initialization
		void Start () 
		{
			dropOne = false;
			dropTwo = false;

			ArmadHeroes.CanvasManager.instance.init ();

			//Spawns selected players
			for(int i = 0; i < 8; i++)
			{	
				if(GlobalPlayerManager.instance.playerData [i].activePlayer)
				{
					//Player instantiation 
					//
					//For each active player, instantiate player prefab, set up player
					GameObject newPlayer = (GameObject)Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
					string characterName = CharacterProfiles.instance.TypeToString(GlobalPlayerManager.instance.GetPlayerData(i).character);
					newPlayer.GetComponent<sumo.sumo_VehicleMovementV2> ().Init(i, GlobalPlayerManager.instance.playerData [i].controllerIndex, characterName);
					_roundManager.addPlayer (newPlayer);
				}
			}
			_roundManager.disablePlayers ();
			_roundManager.ResetRound ();
		}
		
		// Update is called once per frame
		void Update ()
		{	
			if (roundTimer >= 0 && countDown)
			{
				roundTimer -= Time.deltaTime;
			}
			else
			{
				roundTimer = roundDuration;
			}

			ArmadHeroes.CanvasManager.instance.updateTime (roundTimer);

			checkIfPlayRollingSound ();

			if (Input.GetKeyDown (KeyCode.R))
			{			
				GetComponentInChildren<sumo.sumo_RoundManager> ().RoundOver ();
			}

			if (roundTimer < 50f && !dropOne) 
			{
				mineRings [0].GetComponent<SumDropRing> ().drop ();
				dropOne = true;
			}
			if (roundTimer < 30f && !dropTwo) 
			{
				mineRings [1].GetComponent<SumDropRing> ().drop ();
				dropTwo = true;
			}

		}


		public void resetTimer()
		{
			roundTimer = roundDuration;
			dropOne = false;
			dropTwo = false;
		}

		public void checkIfPlayRollingSound()
		{
			for (int i = 0; i < _roundManager.Players.Count; i++)
			{
				if (!_roundManager.Players [0].activeSelf) 
				{
					SoundManager.instance.FadeAndKillAudio (rollingSource, 0.05f);
					break;
				}

				if (_roundManager.Players [i].GetComponent<sumo.sumo_VehicleMovementV2> ().speed > 0 && !SoundManager.instance.IsClipnamePlaying(rollingAudio.name))
				{
					rollingSource = SoundManager.instance.PlayClip (rollingAudio, true, 0.2f);
					break;
				}
				else if(i == _roundManager.Players.Count - 1)
				{
					SoundManager.instance.FadeAndKillAudio (rollingSource, 0.05f);
				}
			}

		}

		public void resetMineRings()
		{
			for (int i = 0; i < mineRings.Length; i++)
			{
				mineRings [i].GetComponent<SumDropRing> ().reset();
			}
		}

		public void goToDebrief()
		{	
			StartCoroutine (waitForExplosionSoundToFinish ());
			SceneManager.LoadScene ("DebriefScene");		
		}

		public IEnumerator waitForExplosionSoundToFinish()
		{
			yield return new WaitForSeconds (0.39f);
		}
	}


}
