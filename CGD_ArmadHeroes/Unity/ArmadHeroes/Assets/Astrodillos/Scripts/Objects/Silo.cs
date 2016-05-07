using UnityEngine;
using System.Collections;
using DG.Tweening;
using ArmadHeroes;

namespace Astrodillos{
	public class Silo : MonoBehaviour {

		#region Public Properties
		public Animator siloAnimator;
		public Collider2D enterTrigger, collisionBox;
		public RocketShip rocketShip;
		public GameObject enterPrompt;
		public AudioClip siloOpenSfx;
		#endregion

		#region Private Properties
		private Tweener rocketTween;
		private int playersInTrigger = 0;
		//How long from game start till silo opens
		private float openTimer = 10.0f;
		#endregion
	
		#region Unity Behaviours
		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			if (openTimer > 0) {
				openTimer -= Time.deltaTime;
				if (openTimer <= 0) {
					OpenSilo ();
				}
			}
		}

		void OnTriggerEnter2D(Collider2D col){
			
			if (col.tag == "Player" && enterTrigger.enabled) {
				Actor_Armad player = col.gameObject.GetComponent<Actor_Armad>();
				if (player && player.armadState == ArmadState.gameplay) {
					enterPrompt.SetActive(true);
					playersInTrigger++;
				}

			}
		}

		void OnTriggerStay2D(Collider2D col){
			if (col.tag == "Player" && enterTrigger.enabled) {
				Actor_Armad player = col.gameObject.GetComponent<Actor_Armad>();
				if (player && player.armadState == ArmadState.gameplay) {
					if(player.controller.activateButton.JustPressed()){
						rocketShip.EnterRocket(player);
						OnTriggerExit2D(col);
					}
				}

			}
		}

		void OnTriggerExit2D(Collider2D col){
			
			if (col.tag == "Player") {
				Actor_Armad player = col.gameObject.GetComponent<Actor_Armad>();
				if (player) {
					playersInTrigger--;
				}

			}

			if(playersInTrigger<=0){
				playersInTrigger = 0;
				enterPrompt.SetActive(false);
			}
		}

		#endregion

		public void Reset(){
			if (!gameObject.activeInHierarchy) {
				return;
			}
			playersInTrigger = 0;
			enterTrigger.enabled = false;
			enterPrompt.SetActive (false);
			siloAnimator.SetBool ("isOpen", false);
			gameObject.tag = "Astrodillos/ActorOnly";
			openTimer = 10;
			if (rocketTween != null && rocketTween.IsPlaying ()) {
				rocketTween.Kill ();
			}
		}

		void OpenSilo(){

			siloAnimator.SetBool ("isOpen", true);
			rocketShip.gameObject.SetActive (true);

			SoundManager.instance.PlayClip (siloOpenSfx);

			rocketTween = rocketShip.transform.DOMoveY (-3.4f, 1.0f);
			rocketTween.SetEase (Ease.OutSine);

			rocketTween.OnComplete (() => {
				enterTrigger.enabled = true;
				gameObject.tag = "Untagged";
			});
		}

		public void DisableEnterTrigger(){
			enterTrigger.enabled = false;
			enterPrompt.SetActive (false);
			playersInTrigger = 0;
		}
	}
}
