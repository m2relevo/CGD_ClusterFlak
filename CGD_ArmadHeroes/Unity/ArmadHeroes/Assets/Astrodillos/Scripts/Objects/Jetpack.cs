using UnityEngine;
using System.Collections;
using DG.Tweening;
using ArmadHeroes;

namespace Astrodillos{
	public class Jetpack : MonoBehaviour {

		#region Public propeties
		public ParticleSystem particles;
		public AudioClip flames;
		public Animator animator;
		public SpriteRenderer spriteRenderer;
		#endregion

		#region Private properties
		private ParticleSystem.EmissionModule emissionParticles;
		private Renderer particlesRenderer;
		private Actor_Armad attachedActor = null;
		private AudioSource jetpackFlames;
		private float vibrationStrength = 0.075f;

		//Fuel
		private float maxVelocity = 5.0f;		//Max velocity in any direction
		private float jetpackPower = 2.5f;		//Jetpack thrusting power
		private float fuel = 1.0f;				//Current fuel
		private float maxFuel = 1.0f;			//Max fuel
		private float fuelBurnRate = 0.2f;		//Burn speed
		private float fuelRefillRate = 1.5f;	//Recharge speed
		private float fuelRefillTime = 1.0f; 	//Time till recharge starts after stopping
		private float fuelRefillCounter = 0;	//Time since stopped using fuel
		private bool usingFuel = false; 		//Used fuel this frame?
		
		//Space/ground
		private bool useGravity {
			set{ }
			get { return Gametype_Astrodillos.instance.UseGravity(); }
		}

		#endregion

		#region Unity Behaviours
		// Use this for initialization
		void Awake () 
        {
			particlesRenderer = particles.GetComponent<Renderer> ();
			emissionParticles = particles.emission;
		}
		
		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			//If the jetpack is attached to a player
			if (attachedActor != null) {

				//Only show on player during gameplay
				spriteRenderer.enabled = (attachedActor.armadState == ArmadState.gameplay);

				if (!attachedActor.inBall) {
					UpdateThrusting ();
				}

				UpdateSortingOrder ();

				if (jetpackFlames != null && jetpackFlames.clip == flames) {
					SoundManager.instance.SetPan(transform.position,jetpackFlames);
				}
			} 
        }

		void LateUpdate(){
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}

			if (!usingFuel) {
				//Refill process
				fuelRefillCounter += Time.deltaTime;

				//Increase jetpack fuel
				if(fuel<maxFuel && fuelRefillCounter>=fuelRefillTime){
					ChangeFuel(fuelRefillRate);
				}

			}

			usingFuel = false;

		}

		#endregion

		void UpdateThrusting()
        {

			animator.SetFloat ("angle", Mathf.Abs(attachedActor.GetAngle()));
			animator.SetBool ("walking", attachedActor.GetWalking ());

			//Left analog stick for direction
			Vector2 stickAngle = new Vector2(attachedActor.controller.moveX.GetValue(), attachedActor.controller.moveY.GetValue());

			if (attachedActor.armadState == ArmadState.gameplay && 
			    attachedActor.controller.accelerateButton.GetValue () > 0 && HasFuel()) {
				Thrust (stickAngle.normalized, attachedActor.controller.accelerateButton.GetValue ());
			} else {
				//  isThrusting = false; Never used and causing warnings
				//Stop thrusting particles
				if (particles.emission.enabled) {
					emissionParticles.enabled = false;
				}

				if (jetpackFlames != null) {
					SoundManager.instance.FadeAndKillAudio(jetpackFlames,0.5f);
					jetpackFlames = null;
				}


				//Lower player's height
				if(useGravity){
					attachedActor.ChangeHeight(-2);
				}
			}
		}

		/// <summary>
		/// Updates the sorting order of sprites and particle systems
		/// </summary>
		void UpdateSortingOrder(){
			//Set the sprite ordering for the jetpack compared to the actor
			int actorOrder = attachedActor.GetSpriteOrder();
			spriteRenderer.sortingOrder = actorOrder+2;
			spriteRenderer.sortingOrder = (Mathf.Abs(attachedActor.GetAngle()) > 112) ? actorOrder+2 : actorOrder-1;
			particlesRenderer.sortingOrder = (Mathf.Abs(attachedActor.GetAngle()) > 112) ? spriteRenderer.sortingOrder-1 : actorOrder-1;
		}

		void Thrust(Vector2 direction, float amount){
			//Apply thrust to the body of the player

			attachedActor.ApplyForce(direction * amount * jetpackPower);
			attachedActor.body.velocity = new Vector2 (Mathf.Clamp (attachedActor.body.velocity.x, -maxVelocity, maxVelocity), Mathf.Clamp (attachedActor.body.velocity.y, -maxVelocity, maxVelocity));
			//Increase the player's height
			if (useGravity) {
				attachedActor.ChangeHeight (3 * amount);
			}

			particles.startLifetime = 0.2f*amount;

			//Decrease jetpack fuel
			ChangeFuel (-fuelBurnRate);

			//Vibrate controller
			attachedActor.controller.StartVibration(vibrationStrength, 0.1f);

			//Audio
			if (jetpackFlames == null) {
				jetpackFlames = SoundManager.instance.PlayClip(flames, transform.position, true);
			}
         
			
			//Play thrusting particles if not already
			if (!particles.isPlaying) 
            {
				particles.Play ();
               
			}
			if (!particles.emission.enabled) {
				emissionParticles.enabled = true;
			}

		}

		public void ChangeFuel(float change){
            if(attachedActor==null)
            {
                return;
            }
            if (attachedActor != null)
            {
                if(attachedActor.armadState == ArmadState.dead)
                {
                    return;
                }
            }
			fuel += change * Time.deltaTime;

			fuel = Mathf.Clamp (fuel, 0.0f, maxFuel);
			//armadUI.UpdateFuel ((float)fuel / (float)maxFuel);
			attachedActor.armaCanvas.UpdateSliderOne(((float)fuel / (float)maxFuel) * 100);

			if (change < 0) {
				fuelRefillCounter = 0;
				usingFuel = true;
			}

		}



		public bool HasFuel(){
			return fuel > 0;
		}

		/// <summary>
		/// Removes jetpack from actor, reparents to manager
		/// </summary>
		public void Reset(){
			attachedActor = null;
			spriteRenderer.enabled = true;
			animator.SetFloat ("angle", 0);
			animator.SetBool ("walking", false);
			gameObject.SetActive (false);
			if (jetpackFlames != null) {
				jetpackFlames.Stop ();
			}
		}

		//Attach the jetpack to the actor who collided with it
		public void Pickup(Actor_Armad actor){
			attachedActor = actor;	
			//Set the parent transform to the actor
			transform.SetParent(actor.spriteRenderer.gameObject.transform, false);
			animator.SetFloat ("angle", Mathf.Abs(actor.GetAngle()));

			actor.animator.SetTrigger ("resetWalk");

	
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
			transform.localScale = new Vector3 (1, 1, 1);
			UpdateSortingOrder ();
		}

		public Actor_Armad GetAttachedActor()
        {
			return attachedActor;
		}
			
	}
}