using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Astrodillos{
	public class JetpackManager : MonoBehaviour {

		public static JetpackManager instance;

		public GameObject jetpackPrefab;
		public GameObject jetpackPickupPrefab;

		public List<Jetpack> jetpacks = new List<Jetpack>();
		List<JetpackPickup> pickups = new List<JetpackPickup>();

		// Use this for initialization
		void Awake () {
			instance = this;
		}



		/// <summary>
		/// Removes Jetpacks from players and sets all as inactive
		/// </summary>
		public void Reset(){
			for(int i = 0; i<jetpacks.Count; i++){
				jetpacks[i].Reset();
			}
			for(int i = 0; i<pickups.Count; i++){
				pickups [i].gameObject.SetActive (false);
			}
		}

		public void SpawnPickup(Vector3 position){
			GameObject jetpack = GetPooledPickup ();
			jetpack.transform.SetParent (gameObject.transform);
			jetpack.transform.position = position;
		}

		GameObject GetPooledPickup(){
			//CHeck current pool for inactive jetpacks we can use
			for (int i = 0; i<pickups.Count; i++) {
				if(!pickups[i].gameObject.activeSelf){
					pickups[i].gameObject.SetActive(true);
					return pickups[i].gameObject;
				}
			}

			//If none in current pool, instantiate a new one
			GameObject newJetpack = Instantiate (jetpackPickupPrefab);
			JetpackPickup jetpack = newJetpack.GetComponent<JetpackPickup> ();
			pickups.Add (jetpack);
			return newJetpack;
		}

		GameObject GetPooledJetpack(){
			//CHeck current pool for inactive jetpacks we can use
			for (int i = 0; i<jetpacks.Count; i++) {
				if(!jetpacks[i].gameObject.activeSelf){
					jetpacks[i].gameObject.SetActive(true);
					return jetpacks[i].gameObject;
				}
			}

			//If none in current pool, instantiate a new one
			GameObject newJetpack = Instantiate (jetpackPrefab);
			Jetpack jetpack = newJetpack.GetComponent<Jetpack> ();
			jetpacks.Add (jetpack);
			return newJetpack;
		}

		public void SpawnJetpackOnActor(Actor_Armad actor){
			if (!JetpackManager.instance.HasJetpack (actor)) {
				GetPooledJetpack ();
				jetpacks [jetpacks.Count - 1].Pickup (actor);
			}

		}

		public void DestroyPickups(){
			for(int i = pickups.Count-1; i>=0; i--){
				pickups [i].gameObject.SetActive (false);
			}
		}

		public bool HasJetpack(Actor_Armad actor){
			for(int i = jetpacks.Count-1; i>=0; i--){
				if(jetpacks[i].GetAttachedActor() == actor){
					return true;
				}
			}

			return false;
		}
	}
}
