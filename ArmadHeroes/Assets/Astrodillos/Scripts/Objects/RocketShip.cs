using UnityEngine;
using System.Collections;
using DG.Tweening;
using ArmadHeroes;

namespace Astrodillos{
	public class RocketShip : MonoBehaviour {

		#region Public Properties
		public ParticleSystem rocketSmoke;
		public Transform window;
		public AudioClip icbmLaunchSfx;
		public Silo silo;
		#endregion

		#region Private Properties
		//private Rigidbody2D body;
		private SpriteRenderer spriteRenderer;
		//private Renderer smokeRenderer;

		private int playersOnShip = 0;
	//	private int requiredPlayerCount = 1;
		private bool preppingLaunch = false;
		private float launchCountdown = 5;

		private Sequence launchSequence;
		private Vector3 startPos;
		#endregion

		#region Unity Behaviours
		// Use this for initialization
		void Awake () {
			spriteRenderer = GetComponent<SpriteRenderer> ();

			gameObject.SetActive (false);

			startPos = gameObject.transform.position;

		}
			
		
		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			if (preppingLaunch && launchCountdown>0) {
				launchCountdown -= Time.deltaTime;
				if (launchCountdown <= 0) {
					TakeOff ();
				}
			}
		}

		#endregion

		public void Reset(){
			playersOnShip = 0;
			preppingLaunch = false;
			launchCountdown = 5;

			if (launchSequence != null && launchSequence.IsPlaying ()) {
				launchSequence.Kill ();
			}

			transform.position = startPos;


		}

		public void EnterRocket(Actor_Armad player){
		//	int index = (requiredPlayerCount-1)-playersOnShip;
			player.EnterRocket(window, spriteRenderer.sortingOrder+1);


			player.transform.localPosition = Vector3.zero;
			playersOnShip++;

			if (playersOnShip > 1) {
				player.spriteRenderer.enabled = false;
			}

			if (!preppingLaunch) {
				PrepLaunch ();
			}
			

		}

		void PrepLaunch(){
			preppingLaunch = true;

			rocketSmoke.Play ();
		}

		void TakeOff(){

			//Hide enter prompts
			silo.DisableEnterTrigger();

			//Vibrate all controllers
			for (int i = 0; i < ControllerManager.instance.controllerCount; i++) {
				ControllerManager.instance.GetController (i).StartVibration (0.5f, 7.5f);
			}

			//Shake screen
			GameManager.instance.ShakeScreen(6.0f,0.3f,50);

			//Sfx
			SoundManager.instance.PlayClip(icbmLaunchSfx);
			
			launchSequence = DOTween.Sequence ();

			//Tween rocket off of screen
			Tweener moveTween = transform.DOMoveY(20,7.5f);
			moveTween.SetEase (Ease.InCirc);

			launchSequence.Append (moveTween);
			launchSequence.InsertCallback (6.0f,()=>{
				rocketSmoke.Stop();
			});

			launchSequence.AppendInterval (1.0f);

			launchSequence.OnComplete (()=>{
				GameManager.instance.FadeToBlack(()=>{
					Gametype_Astrodillos.instance.SetState (Gametype_Astrodillos.GameState.Space);
					gameObject.SetActive(false);
				});
			});
		}

		           
	}
}
