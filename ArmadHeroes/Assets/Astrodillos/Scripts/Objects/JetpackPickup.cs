using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos{
	public class JetpackPickup : MonoBehaviour {

		public AudioClip pickupSfx;

		void OnTriggerEnter2D(Collider2D col){

			//If the colliding rigidbody is a player
			if (col.tag == "Player") {
				
				JetpackManager.instance.SpawnJetpackOnActor (col.GetComponentInParent<Actor_Armad> ());
				SoundManager.instance.PlayClip (pickupSfx);
				gameObject.SetActive (false);
			}
		}
	}
}
