//Armad Heroes 
//SUMO 
//Gareth Griffiths, Peter Maloney, Alex Nunns, Jake Downing
//Round Manager
//
//Manages the UI, player control, player respawn and level reset between rounds

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace sumo
{
    public class sumo_RoundManager : MonoBehaviour
    {
        public Canvas canvas;
        public Text message;
        public Vector3 StartPos, FinishPos;
	
		public int numOfRounds;
        int roundNo;

		//Spawn positions for players (Trying to make it a bit more consistent)
		public int spawnDistanceFromMiddle;
		public Vector3[] spawnPositions = new Vector3[8];

        public List<GameObject> Players = new List<GameObject>();
        Stack<int> Rankings = new Stack<int>();

		public GameObject shadow;
		GameObject[] shadows = new GameObject[8];

		public AudioClip playersLanding;

       delegate void displayMessage();      

        // Use this for initialization
        void Start()
        {   
			//Sets up a set of positions around the ring for spawning players at when they respawn.
			spawnPositions [0] = new Vector3 (-spawnDistanceFromMiddle, spawnDistanceFromMiddle, 0);
			spawnPositions [1] = new Vector3 (spawnDistanceFromMiddle, -spawnDistanceFromMiddle, 0);
			spawnPositions [2] = new Vector3 (spawnDistanceFromMiddle, spawnDistanceFromMiddle, 0);
			spawnPositions [3] = new Vector3 (-spawnDistanceFromMiddle, -spawnDistanceFromMiddle, 0);
			spawnPositions [4] = new Vector3 (0, spawnDistanceFromMiddle, 0);
			spawnPositions [5] = new Vector3 (0, -spawnDistanceFromMiddle, 0);
			spawnPositions [6] = new Vector3 (spawnDistanceFromMiddle, 0, 0);
			spawnPositions [7] = new Vector3 (-spawnDistanceFromMiddle, 0, 0);
           

			//Sets round text start position and position to move too
            StartPos = canvas.transform.position + new Vector3(-((Screen.width / 2) + 300), 0f, 0f);
            FinishPos = canvas.transform.position + new Vector3(((Screen.width / 2) + 300), 0f, 0f);
            roundNo = 1;
			Players.Clear ();
			if (GameObject.FindGameObjectsWithTag ("Sumo/Player").Length > 0)
			{
				Players.AddRange (GameObject.FindGameObjectsWithTag ("Sumo/Player"));
			}

			//Sets up first round


        }

        // Update is called once per frame
        void Update()
		{
			//If screen width changes, update the text sliding positions
			if (((Screen.width / 2) + 300) != StartPos.x) {
				StartPos = canvas.transform.position + new Vector3 (-((Screen.width / 2) + 300), 0f, 0f);
				FinishPos = canvas.transform.position + new Vector3 (((Screen.width / 2) + 300), 0f, 0f);
			}

			//If only one player left, round has ended
			if (Rankings.Count == Players.Count - 1 || Input.GetKeyDown (KeyCode.R)) {

				//Give winner points
				for (int i = 0; i < Players.Count; i++) 
				{
					if(!Rankings.Contains(i))
					{
						ArmadHeroes.SumoManager.instance.countDown = false;
						sumo.sumo_VehicleMovementV2 currentPlayerScript = Players [i].GetComponent<sumo_VehicleMovementV2> ();
						currentPlayerScript.chevron_score += 1;
						ArmadHeroes.CanvasManager.instance.setPlayerValue (currentPlayerScript.playerNumber, currentPlayerScript.chevron_score);
						currentPlayerScript.accolade_unique += 0.001f;
						break;
					}
				}

				//If less that 3 rounds have been played, start new round
				if (roundNo < numOfRounds) 
				{	
					ArmadHeroes.SumoManager.instance.countDown = false;
					RoundOver ();
				}
				//Else, end the game and go to rankings screen
				else 
				{
					for (int i = 0; i < Players.Count; i++)
					{
						sumo.sumo_VehicleMovementV2 currentPlayerInstance = Players [i].GetComponent<sumo.sumo_VehicleMovementV2> ();

						ArmadHeroes.GlobalPlayerManager.instance.SetDebriefStats (currentPlayerInstance.playerNumber, currentPlayerInstance.chevron_score, 0f, 
							currentPlayerInstance.accolade_distance, currentPlayerInstance.accolade_distance, 0f, currentPlayerInstance.accolade_unique);
					}	

					ArmadHeroes.SumoManager.instance.goToDebrief ();
				}
			}
		}
            

        public void addLoser(int playerNo, GameObject loser)
        {			
            Rankings.Push(playerNo); 
        }

        public void RoundOver()
        {
			disablePlayers ();
            message.text = "ROUND OVER";
            message.GetComponent<RectTransform>().position = StartPos;
			Rankings.Clear();
			StartCoroutine(swipeText(ResetRound));
            roundNo++;
        }

		public void ResetRound()
		{
			for (int i = 0; i < Players.Count; i++)
			{
				Players[i].transform.position = spawnPositions[i];
				Players[i].GetComponent<sumo_VehicleMovementV2>().enabled = false;
				Players [i].GetComponentInChildren<BoxCollider2D>().enabled = false;
				Players[i].GetComponentInChildren<sumo_RingExit> ().active = false;                   
				Players[i].GetComponent<sumo_VehicleMovementV2> ().speed = 0f;
				Players[i].GetComponent<sumo_VehicleMovementV2>().state = "still";
				Players [i].GetComponent<sumo.sumo_VehicleMovementV2> ().parachute.SetActive(true);
			}

			callPlayerParacute();

		}

        public void NewRound()
        {			
			message.text = "ROUND" + roundNo;
            message.GetComponent<RectTransform>().position = StartPos;
			StartCoroutine(swipeText(enablePlayers));  

        }


		void enablePlayers()
		{
			for (int i = 0; i < Players.Count; i++)
			{
				Players[i].GetComponent<sumo_VehicleMovementV2>().enabled = true;
				Players [i].GetComponentInChildren<BoxCollider2D>().enabled = true;
				Players [i].GetComponentInChildren<sumo_RingExit> ().active = true;
				ArmadHeroes.SumoManager.instance.resetTimer ();
				ArmadHeroes.SumoManager.instance.countDown = true;
			}
		}
		public void disablePlayers()
		{
			for (int i = 0; i < Players.Count; i++)
			{
				Players [i].gameObject.SetActive (false);
			}		
		}

       

		public void callPlayerParacute()
		{
			StartCoroutine (playerParachute (NewRound));
		}

		public void addPlayer(GameObject player)
		{
			Players.Add (player);
		}



        IEnumerator swipeText(displayMessage func)//delegate void displayMessage())
        {
        for (float i = 0; i < 1; i += 0.01f)
            {
				message.GetComponent<RectTransform> ().position = Vector3.Lerp(StartPos, (StartPos + FinishPos) / 2, i);
                yield return null;
            }           
            yield return new WaitForSeconds(1.5f);
            for (float i = 0; i < 1; i += 0.01f)
            {
                message.GetComponent<RectTransform>().position = Vector3.Lerp((StartPos + FinishPos) / 2, FinishPos, i);
                yield return null;
            }
            func();  
        }

		IEnumerator playerParachute(displayMessage func)
		{
			for (int i = 0; i < Players.Count; i++)
			{
				Players [i].gameObject.SetActive (true);
			}	

			float lerpTime = 0;
			for (int i = 0; i < Players.Count; i++) 
			{
				Players [i].transform.position = spawnPositions [i] + new Vector3(0f, FindObjectOfType<Camera> ().orthographicSize, 0f);
				shadows[i] = (GameObject)Instantiate (shadow, spawnPositions [i], Quaternion.identity);
			}
			while (Players [0].transform.position != spawnPositions [0])
			{
				for (int i = 0; i < Players.Count; i++) 
				{
					Players [i].transform.position = Vector3.Lerp (spawnPositions [i] + new Vector3 (0f, FindObjectOfType<Camera> ().orthographicSize, 0f), spawnPositions [i], lerpTime);

					lerpTime += Time.deltaTime / 2;
					yield return null;
				}
			}

			for (int i = 0; i < Players.Count; i++) 
			{
				ArmadHeroes.SoundManager.instance.PlayClip (playersLanding);
				Players [i].GetComponent<sumo.sumo_VehicleMovementV2> ().parachute.SetActive(false);
				Destroy (shadows [i]);
			}

			func ();
		}
}
}
