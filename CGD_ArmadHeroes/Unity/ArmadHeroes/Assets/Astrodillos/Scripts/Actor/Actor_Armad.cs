using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos
{
	public class Actor_Armad : PlayerActor
    {
		//Public member variables
	//	public SpriteRenderer spriteRenderer;
		public Animator animator;
		public Rigidbody2D body;
		public SpriteRenderer shadow;
        public ParticleSystem muzzleFlash, bulletCasing, walkDust;
		public ArmaCanvas armaCanvas;

		public Transform bulletSpawnTransform;

        //audio
        public AudioClip hitSfx, rollSfx;
      

		//State
		public ArmadState armadState { get; private set; }
		public float Health { get { return health; } }
		public bool inBall { get; private set; }

		//Member variables
		private CharacterAnimationOverride animationOverride; //Change characters using this

		private Renderer muzzleFlashRenderer, bulletCasingRenderer, walkDustRenderer;

		private string 		characterName;				//The name of the character
		private Weapon 		equippedWeapon; 			//The weapon the player is currently using
		private Weapon 		defaultWeapon; 				//Weapon player starts with
		private Weapon 		fireButtonEquippedWeapon; 	//The weapon that was equipped when the fire button was pressed
        
		//Movement
		private float currentHeight = 0; 	//Height off the ground - for jetpacks
		private float maxHeight = 1.5f; 	//Max height the actor can go off of the ground
		private float walkSpeed = 3; 		//Speed modifier
		private float ballMaxTime = 0.5f; 		//Total time to spend in ball mode
		private float ballTime = 0;			//Current time spent in ball mode

		private ScreenWrap 	wrap;


		private bool useGravity {
			set{ }
			get { return Gametype_Astrodillos.instance.UseGravity(); }
		}


        // Use this for initialization
        void Awake () 
        {
			defaultWeapon = m_RPG;
           	currentAngle = 0;
            wrap = gameObject.GetComponent<ScreenWrap>();
            smokin.GetComponent<Renderer>().sortingLayerName = "Midground";
			muzzleFlashRenderer = muzzleFlash.GetComponent<Renderer> ();
			bulletCasingRenderer = bulletCasing.GetComponent<Renderer> ();
			walkDustRenderer = walkDust.GetComponent<Renderer> ();

			animationOverride = GetComponent<CharacterAnimationOverride> ();
            health = 100;

			m_flamethrower.onKillCallback = IncreaseKillCount;
		}
         
		#region Getters and Setters
		public void Init(int controller, int player){
			m_controllerID = controller;
			playerID = player;
		}

		public void SetCharacter(ArmadHeroes.CharacterType name){
            characterName =ArmadHeroes.CharacterProfiles.instance.TypeToString(name);

			animationOverride.SetCharacter (characterName);
		}

	
		public bool GetWalking(){
			return walking;
		}

		void SetDrag(){
			switch (useGravity) {
			case true:
				if(OnGround()){
					body.drag = 1.0f;
				}
				else{
					body.drag = 0.5f;
				}

				break;
			case false:
				body.drag = 0.0f;
				break;
			}
		}
		
		public int GetSpriteOrder(){
			return spriteRenderer.sortingOrder;
		}

        public void IncreaseKillCount()
        {
			chevron_score++;
			CanvasManager.instance.setPlayerValue(playerID, chevron_score);
        }

		public void SendPlayerData(){
			GlobalPlayerManager.instance.SetDebriefStats (playerID, chevron_score, accolade_timesShot, accolade_distance, accolade_distance, accolade_shotsFired, accolade_unique);
		}

		#endregion

		#region Unity behaviours
		// Update is called once per frame
		protected override void Update ()
        {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			base.Update ();

			if (armadState == ArmadState.gameplay) 
            {
				UpdateShooting ();
				UpdateMovement ();

				//Show Hud
				if (controller.hudButton.JustPressed ()) {
					armaCanvas.Activate ();
				}
            }

            BurnCount();
		}


       
		public virtual void OnTriggerEnter2D(Collider2D col)
		{
			//check if col is a projectile
			if (col.gameObject.GetComponent<Projectile>() && health > 0)
			{
				Projectile _projectile = col.gameObject.GetComponent<Projectile>();
				if (_projectile.callerID != playerID) {
					if ((health - _projectile.damage) <= 0) {

						PlayerManager.instance.GetPlayer(_projectile.callerID).IncreaseKillCount();
					}

					TakeDamage(_projectile.damage);
					accolade_timesShot++;

				}
			}
		}
		#endregion

		#region Custom behaviours
		public void UpdateShooting()
		{
			if (controller.shootButton.JustPressed ()) {
				fireButtonEquippedWeapon = equippedWeapon;
			}
			//Won't fire if weapon has changed since button press
			if (!inBall && controller.shootButton.IsDown () && (equippedWeapon == fireButtonEquippedWeapon)) {

				//float xScale = transform.localScale.x; //Commented out because it is never used
				float fireAngle = currentAngle;
				//Correct rotation for bullet -  this works but could be done more efficiently
				if (Mathf.Abs (currentAngle) == 90) {
					fireAngle -= 90;//0 or 180
				} else if (currentAngle == 30 || currentAngle == -150) {
					fireAngle -= 60;//-30 or -210(30)
				} else if (currentAngle == -30 || currentAngle == 150) {
					fireAngle -= 120;//-150 or 30
				} else { // 0 or 180
					fireAngle -= 90; //-90 or 90 
				}

				//Muzzle flash sorting
				muzzleFlashRenderer.sortingOrder = (fireAngle == -270 || fireAngle == 90) ? spriteRenderer.sortingOrder - 1 : spriteRenderer.sortingOrder + 1;

				//Check if current weapon is empty
				if (equippedWeapon.GetCurrentAmmo () == 0) {
					DropWeapon ();
				}


				if (equippedWeapon.firesBullets && equippedWeapon.CheckCoolDown ()) {
					accolade_shotsFired++;
				}

				equippedWeapon.SetLayer (gameObject.layer);
				equippedWeapon.Shoot(bulletSpawnTransform.position, Quaternion.AngleAxis(fireAngle, Vector3.forward)*Vector3.right, playerID, this, Color.white, ActorType.Player, BulletModifier.vanilla, currentHeight);



				//Display ammo count on UI
				if (!equippedWeapon.infiniteAmmo) {
					armaCanvas.UpdateCount(equippedWeapon.GetCurrentAmmo());
				}
			} else if (controller.shootButton.JustReleased ()) {
				equippedWeapon.StopFire ();
			}
		}

		void UpdateMovement(){

			walking = false;
			//Rotation
			//Angle of right analog stick sets animation/rotation of player
			Vector2 aimAngle = new Vector2(controller.aimX.GetValue(), controller.aimY.GetValue());
			//If there is input on the stick
			if (aimAngle != Vector2.zero) {
				//Update the current angle of the player
				float angle = (int)(Mathf.Atan2 (aimAngle.x, -aimAngle.y) * Mathf.Rad2Deg);
				SetAngle (angle);
			} 


			//Left analog stick for movement
			//Move the player
			Vector2 walkAngle = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());



			//Set aiming angle to walk angle if not aiming
			if(walkAngle != Vector2.zero && aimAngle == Vector2.zero){
				float angle = (int)(Mathf.Atan2 (walkAngle.x, -walkAngle.y) * Mathf.Rad2Deg);
				SetAngle (angle);
			}

			if(OnGround()){


				walking = walkAngle != Vector2.zero;

				if (!inBall) {

                    Steps();
					

					body.velocity = new Vector2 (walkAngle.x, walkAngle.y) * walkSpeed;

					accolade_distance += body.velocity.magnitude;

					//Enter ball mode?
					if (controller.boostButton.JustPressed ()) {
						EnterBall (walkAngle);
					}
				} else {
					ballTime += Time.deltaTime;
					if (ballTime >= ballMaxTime) {
						ExitBall ();
					}
				}


			}

			if (useGravity) {
				//Update sprite order
				UpdateSpriteOrder ();
			}

			animator.SetBool ("walking", walking);

		}

        protected override void Steps()
        {
            //Dust particles at feet
            if (walking)
            {
                walkDust.Emit(1);
            }
            if (walking && footstepCooldown <= 0)
            {
                if (footstepSfx)
                {
                    SoundManager.instance.PlayClip(footstepSfx, transform.position);
                }
                leftFootprint.startRotation = Mathf.Deg2Rad * -(currentAngle - 90);
                rightFootprint.startRotation = leftFootprint.startRotation;
                leftFootprint.Emit(1);
                rightFootprint.Emit(1);

                footstepCooldown = 0.15f;
            }
            else if (footstepCooldown > 0)
            {
                footstepCooldown -= Time.deltaTime;
            }
        }

        public void Enable()
        {
			gameObject.SetActive (true);
		}

		public void Disable(){
			gameObject.SetActive (false);
		}
		
		public void Spawn(Vector2 position){

			transform.position = new Vector3 (position.x, position.y, 0);
			transform.localEulerAngles = Vector3.zero;
			health = 100;
			SetState (ArmadState.gameplay);

			//Enable collider
			bodyCollider.enabled = true;

			//Reset velocity
			body.velocity = Vector2.zero;
			body.isKinematic = false;
			spriteRenderer.enabled = true;
			ballTime = 0;

			//Face towards map centre
			float angle = -Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg;
			SetAngle (angle);

			armaCanvas.Deactivate ();


			//Set weapon back to default
			SetEquippedWeapon (defaultWeapon);

			//Start on ground collision layer
			currentHeight = 0;
			gameObject.layer = LayerMask.NameToLayer ("Astrodillos/Bottom");

			//Show shadow if not in space
			shadow.gameObject.SetActive (useGravity);
			shadow.gameObject.gameObject.transform.localPosition = new Vector3 (0, -0.33f, 0);
			wrap.enabled = !useGravity;
			SetDrag ();

			UpdateSpriteOrder ();
			Enable ();

			animator.SetBool ("isHead", false);
			animator.SetBool ("isDead", false);
			if (inBall) {
				ExitBall ();
			}
		}

		void EnterBall(Vector2 enterDirection){
			animator.SetTrigger ("enterBall");
			body.AddForce (new Vector2 (enterDirection.x, enterDirection.y) * 5, ForceMode2D.Impulse);
			inBall = true;
			ballTime = 0;
			controller.StartVibration (0.3f, 0.3f);
			SoundManager.instance.PlayClip (rollSfx, transform.position);
		}

		void ExitBall(){
			animator.SetTrigger ("exitBall");
			inBall = false;
			ballTime = 0;
		}

		void UpdateSpriteOrder(){
			spriteRenderer.sortingOrder = SpriteOrdering.GetOrder (shadow.transform.position.y);
			shadow.sortingOrder = spriteRenderer.sortingOrder-1;
			bulletCasingRenderer.sortingOrder = shadow.sortingOrder;
			walkDustRenderer.sortingOrder = shadow.sortingOrder;
		}



        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            health = Mathf.Clamp (health, 0, 100);
           // armadUI.UpdateHealth (health / 100);
            armaCanvas.UpdateSliderTwo(health);

			controller.FlashAltColour(Color.red);

			SoundManager.instance.PlayClip (hitSfx, transform.position);
            //Vibrate controller
            controller.StartVibration (damage);

            if (health <= 0)
            {
                //Kill the player
                SetState(ArmadState.dead);

            }
        }


		void Kill(){
			
			shadow.gameObject.SetActive (false);

			animator.SetBool ("isDead", true);

			//Hide UI
			//armadUI.Hide ();
            armaCanvas.Deactivate();

			//Disable collider
			bodyCollider.enabled = false;

			PlayerManager.instance.KillPlayer ();
		}
		
		
		//Is the actor on the ground or not?
		bool OnGround(){
			return currentHeight == 0 && useGravity;
		}



		public float GetHeight(){
			return currentHeight;
		}


		public void ChangeHeight(float change){
			float oldHeight = currentHeight;

			currentHeight += change * Time.deltaTime;

			//Clamp the height
			currentHeight = Mathf.Clamp (currentHeight, 0, maxHeight);

			float totalChange = currentHeight - oldHeight;

			//Update the collision layer the player is in
			gameObject.layer = SpriteOrdering.CollsionLayerFromHeight (currentHeight);
			spriteRenderer.gameObject.layer = gameObject.layer;

			//Change the y position of the actor
			transform.localPosition += new Vector3(0,totalChange,0);
			//And the opposite for the shadow
			shadow.transform.localPosition -= new Vector3(0,totalChange,0);

			SetDrag ();

			if (currentHeight == 0) {
				shadow.gameObject.gameObject.transform.localPosition = new Vector3 (0, -0.33f, 0);
			} else {
				accolade_unique += Time.deltaTime;

			}

		}


		//Applys force to the body of the player
		public void ApplyForce(Vector2 force){
			body.AddForce (force);
		}

		#region Weapon Behaviours
		/// <summary>
		/// Sets the equipped weapon back to default
		/// </summary>
		public void DropWeapon(){
			equippedWeapon.StopFire ();
			SetEquippedWeapon (defaultWeapon);
		}

		/// <summary>
		/// Updates the equipped weapon
		/// </summary>
		/// <param name="newWeapon">Weapon to equip</param>
		public void SetEquippedWeapon(Weapon newWeapon){
			if (equippedWeapon != null) {
				//Set prev weapon to inactive
				equippedWeapon.gameObject.SetActive(false);
			}
			equippedWeapon = newWeapon;


			equippedWeapon.gameObject.SetActive (true);
			equippedWeapon.Reload ();

			//Update ammo UI
			if (equippedWeapon.infiniteAmmo) {
				armaCanvas.UpdateCount("");
			} else {
				armaCanvas.UpdateCount(equippedWeapon.GetCurrentAmmo());
			}

		}

		#endregion


		/// <summary>
		/// When player enters the rocket ship
		/// </summary>
		public void EnterRocket(Transform window, int sortingOrder){
			transform.SetParent (window);
			currentHeight = 0;
			animator.SetBool ("isHead", true);
			SetState (ArmadState.inactive);
			shadow.gameObject.SetActive (false);
			spriteRenderer.sortingOrder = sortingOrder;

            equippedWeapon.StopFire();

			//Hide UI
            armaCanvas.Deactivate();

			//Disable physics
			body.isKinematic = true;

			//Disable collider
			bodyCollider.enabled = false;

		}

        //public function to set burning
       
        //counts down burntime, turns effect off
        void BurnCount()
        {
            if (smokin.isPlaying)
            {
                burnTime -= Time.deltaTime;
               
                if (burnTime <= 0)
                {
                    smokin.Stop();
                }

            }
        }
		#endregion

		#region State Management
		public void SetState(ArmadState newState){
			LeaveState ();
			armadState = newState;
			EnterState ();
		}

		void EnterState(){
			switch (armadState) {
			case ArmadState.gameplay:
				break;
			case ArmadState.inactive:
				break;
			case ArmadState.dead:
				equippedWeapon.StopFire ();
				Kill();
				break;
			}
		}

		void LeaveState(){
			switch (armadState) {
			case ArmadState.gameplay:
				break;
			case ArmadState.inactive:
				break;
			case ArmadState.dead:
				break;
			}
		}

        public void WeaponSwitch(WeaponType weapon)
        {
            switch (weapon)
            {
                case WeaponType.FLAMETHROWER:
                    {
                        SetEquippedWeapon(m_flamethrower);
                        break;
                    }
                case WeaponType.LASER:
                    {
                        SetEquippedWeapon(m_laser);
                        break;
                    }
                case WeaponType.MACHINEGUN:
                    {
                        SetEquippedWeapon(m_machinegun);
                        break;
                    }
                case WeaponType.SHOTGUN:
                    {
                        SetEquippedWeapon(m_shotgun);
                        break;
                    }
            }
        }
		#endregion     
	}  
}