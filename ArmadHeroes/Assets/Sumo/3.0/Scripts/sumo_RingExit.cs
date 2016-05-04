using UnityEngine;
using System.Collections;


namespace sumo
{
public class sumo_RingExit : MonoBehaviour
{
		public bool killOnExit;
		//Becuase collisions are tested even when not active...
		public bool active;

		public GameObject explosion;	

		void OnTriggerEnter2D(Collider2D collider)
		{

			if (collider.gameObject.tag == "Sumo/Mine") 
			{
				if (collider.gameObject.tag == "Sumo/Mine") 
				{
					FindObjectOfType<sumo.sumo_RoundManager> ().addLoser (GetComponentInParent<sumo.sumo_VehicleMovementV2>().playerNumber, this.transform.parent.gameObject);
					GameObject currentMine = (GameObject)Instantiate (explosion, collider.transform.position, Quaternion.identity);
					ArmadHeroes.SoundManager.instance.PlayClip (ArmadHeroes.SumoManager.instance.sumoSoundList[(int)ArmadHeroes.sumoSounds.EXPLOSION_CLIP]);
					ArmadHeroes.SumoManager.instance.sumoSoundSourceList.Add(ArmadHeroes.SoundManager.instance.PlayClip (ArmadHeroes.SumoManager.instance.sumoSoundList[(int)ArmadHeroes.sumoSounds.EXPLOSION_CLIP]));
					currentMine.GetComponent<sumo_explosionKiller> ().explosionSoundSource = ArmadHeroes.SumoManager.instance.sumoSoundSourceList [ArmadHeroes.SumoManager.instance.sumoSoundSourceList.Count - 1];
					this.transform.parent.gameObject.SetActive (false);
				}
			}

			if (!killOnExit && active)
			{
				if (collider.tag == "Sumo/Ground")
				{
					transform.parent.gameObject.GetComponent<sumo_PlayerLives>().LoseLife();
					if (this.transform.parent.gameObject.GetComponent<sumo_PlayerLives>().lives > 0)
					{
						{
							//Invoke("RespawnPlayers", 5.0f);
						}
					}
					this.transform.parent.gameObject.SetActive(false);
				}
			}

		}



		void OnTriggerExit2D(Collider2D collider)
		{

			if (killOnExit && active)
			{                
				if (collider.tag == "Sumo/Ground")
				{
					Debug.Log("PLAYER LEFT AND DEAD");
					transform.parent.gameObject.GetComponent<sumo_PlayerLives>().LoseLife();
					if (this.transform.parent.gameObject.GetComponent<sumo_PlayerLives>().lives > 0)
					{
						{
							transform.parent.gameObject.SetActive(false);
						}
					}                 

				}
			}

		}

		void RespawnPlayers()
		{
			this.transform.parent.gameObject.SetActive(true);
			if (this.transform.parent.gameObject.tag == "Sumo/Player")
			{
				this.transform.parent.gameObject.transform.position = new Vector3(0.16f, -4f, 0f);

			}
		}
	}
}


