/// <summary>
/// Base class for ArmaTillery players - based from moveNShoot class
/// Created and implemented by Daniel Weston - 02/01/16
/// Edited by Sam Endean - 28/04/2016
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using ArmadHeroes;

namespace Armatillery
{
    public class ArmaPlayer : ArmadHeroes.PlayerActor
    {
        #region Public Members        
        public AudioClip tpChargeSound, tpPoofSound;
        public GameObject TeleportUI, explosionUI, hasteUI, rapidfireUI;
        public BulletModifier m_PowerUp = BulletModifier.vanilla;
        public ParticleSystem shells,muzzleFlash,TPcharge,TPpoof,HPRegenPS,LaserCharge;
        public Barricade barricade; //the barricade object of the actor
        public ArmaCanvas m_UI;
        public Rigidbody2D playerRigid;
        public bool spawnedAtStart = false;
        #endregion

        #region Protected Members
        protected bool grounded, shooting, allowShoot = true;
        protected Vector3 direction = Vector3.zero;//the movement vector for the current frame
        #endregion

        #region Private Memebers
        public ArmaPlayerStates m_state;
        private bool m_building = false;
		private float ballSpeed;
		private Vector3 lastDir;
        #endregion

        #region Unity callbacks
        protected virtual void Awake()
        {
            type = ActorType.Player;
            m_armaAnimator = this.GetComponent<Animator>();
            //set initial player state
            m_state = ArmaPlayerStates.move;

			ballSpeed = moveSpeed;
        }

        public void Init(int _controllerID, int _playerID, string _name = "sarge")
        {
            spawnedAtStart = true;
            playerID = +_playerID;
            m_controllerID = _controllerID;
            health = 100.0f;
            ActorName = _name;
            this.gameObject.SetActive(true);

            m_UI.UpdateSliderOne(health);
            m_override.SetCharacter(ActorName);
            float angle = Random.value * Mathf.PI * 2;
            float x = Mathf.Cos(angle * 5) * Random.Range(3, 5f);
            float y = Mathf.Sin(angle * 5) * Random.Range(1.75f, 3f);

            transform.position = new Vector3(x, y, 0);
            transform.position += Tower.instance.transform.position + Tower.instance.spawnOffsetPosition;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            #region Powerups
            //if the col is a ClassPickup item
            if (col.gameObject.GetComponent<powerup>() && m_PowerUp == BulletModifier.vanilla)
            {
                //OR pickups for combos
                m_PowerUp = col.gameObject.GetComponent<powerup>().m_powerupType | m_PowerUp;

                if ((m_PowerUp & BulletModifier.explodeOnDeath) != 0)
                {
                    //Turn HuD icon on
                    explosionUI.SetActive(true);
                    //Set the HuD to update 
                    explosionUI.GetComponent<PowerUpCountdown>().Activate(this, BulletModifier.explodeOnDeath);
                    //Remove this powerup in 10 seconds time
                    //StartCoroutine(IRemovePowerUp(BulletModifier.explodeOnDeath));
                }
                else if ((m_PowerUp & BulletModifier.rapidFire) != 0 && m_weapon)
                {
                    rapidfireUI.SetActive(true);
                    rapidfireUI.GetComponent<PowerUpCountdown>().Activate(this, BulletModifier.rapidFire);
                    //StartCoroutine(IRemovePowerUp(BulletModifier.rapidFire));
                    m_weapon.mCoolDown /= 2;
                }
                else if ((m_PowerUp & BulletModifier.superSpeed) != 0)
                {
                    hasteUI.SetActive(true);
                    hasteUI.GetComponent<PowerUpCountdown>().Activate(this, BulletModifier.superSpeed);
                    //StartCoroutine(IRemovePowerUp(BulletModifier.superSpeed));
                    moveSpeed *= 2;
                }
                else if ((m_PowerUp & BulletModifier.teleport) != 0)
                {
                    TeleportUI.SetActive(true);
                    TeleportUI.GetComponent<PowerUpCountdown>().Activate(this, BulletModifier.teleport);
                    StartCoroutine(teleportation());
                    //StartCoroutine(IRemovePowerUp(BulletModifier.teleport));
                }
                //turn off pickup
                powerUpManager.instance.disablePowerup(col.transform.parent.gameObject);
                SoundManager.instance.PlayClip(m_armed);

            }//end if  
            #endregion

            #region Weapon Pickups
            if (col.GetComponent<Armatillery.WeaponPickup>() && m_weapon == null)
            {
                Armatillery.ArmaPlayerManager.instance.ArmedPlayers++;
                col.gameObject.SetActive(false);
                Armatillery.Weapons _weapon = col.GetComponent<Armatillery.WeaponPickup>().m_type;
                switch (_weapon)
                {
                    case Weapons.MachineGun:
                        EquipWeapon(m_machinegun);
                        break;
                    case Weapons.ShotGun:
                        EquipWeapon(m_shotgun);
                        break;
                    case Weapons.FlameThrower:
                        EquipWeapon(m_flamethrower);
                        break;
                    case Weapons.LaserGun:
                        EquipWeapon(m_laser);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region Projectiles 
            //check if col is a projectile
            if (col.gameObject.GetComponent<ProjectileBase>() && health > 0)
            {
                ProjectileBase _projectile = col.gameObject.GetComponent<ProjectileBase>();
                //make sure owner of projectile is not the same as the ActorType
                if (_projectile.type != type)
                {
                    //default all bullets will deal damage
                    TakeDamage(_projectile.damage);
                }
            }
            #endregion
        }

        public override void EquipWeapon(Weapon _weapon)
        {
            base.EquipWeapon(_weapon);
            _weapon.infiniteAmmo = true;
        }
        #endregion

        #region 'Custom' callbacks
        /// <summary>
        /// Controlled update for objects, 
        /// Tick is called in the update 
        /// pending on the current game state
        /// </summary>
        protected override void Tick()
        {
            base.Tick();

            if (transform.rotation.z != 0)
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

            if(controller.hudButton.JustPressed())
            {
                m_UI.Activate();
            }

			#region Anchor Button
			if (controller.anchorButton.JustReleased())
			{
                m_building = false;
				if (m_state == ArmaPlayerStates.grounded && !barricade.complete)
				{
					barricade.CancelBuilding();
				}
			}
				
			if (controller.anchorButton.JustPressed())
			{
				grounded = !grounded;
                m_building = true;

				if (m_state == ArmaPlayerStates.grounded)
				{
					//remove the barricade
					barricade.RemoveBarricade();
					m_UI.UpdateSliderOne(barricade.HealthPercentage());
					m_state = ArmaPlayerStates.move;
				}
				else if (m_state == ArmaPlayerStates.move)
				{
					m_state = ArmaPlayerStates.grounded;
				}
			}

			if (controller.anchorButton.IsDown() && !barricade.complete && m_state == ArmaPlayerStates.grounded)
            {
                m_building = true;
				barricade.ConstructBarricade();
				//feed the UI slider a percentage representing the barricade's overall health
				m_UI.UpdateSliderOne(barricade.BuildPercentage());
            }
			#endregion

			if (controller.boostButton.IsDown() && lastDir != Vector3.zero && m_state != ArmaPlayerStates.grounded)
			{
				m_armaAnimator.SetTrigger("enterBall");
				m_state = ArmaPlayerStates.ballin;
			}
			if (controller.boostButton.JustReleased() && m_state == ArmaPlayerStates.ballin)
			{
				ballSpeed = moveSpeed;
				m_armaAnimator.SetTrigger("exitBall");
				m_state = ArmaPlayerStates.move;
			}


            switch (m_state)
            {
                case ArmaPlayerStates.move:
                    Move();
                    break;
                case ArmaPlayerStates.grounded:
                    Grounded();
                    break;
				case ArmaPlayerStates.ballin:
					Ball();
					return; //do not shoot in the ball state
                default:
                    break;
            }
				
            FireWeapon();
        }
        #endregion

        #region Player behaviours
//        protected override void Pause()
//        {
//            if (controller.pauseButton.JustPressed())
//            {
//#if UNITY_PS4
//                ArmaPlayerManager.instance.VibratePlayers(0, 0);
//#endif
//                base.Pause();
//            }
//        }

        public void UpdateScore(int score)
        {
            chevron_score += score;
            CanvasManager.instance.setPlayerValue(playerID, chevron_score);
        }

        /// <summary>
        /// If player has a gun call weapon shoot function
        /// </summary>
        public void FireWeapon()
        {
            if (controller.shootButton.IsDown() && m_weapon && !m_building)
            {
                if (m_weapon.CheckCoolDown())
                {
                    if (m_weapon == m_laser)
                    {
                        if (m_laser.laserState == Weapon_Laser.LaserState.Firing)
                        {
                            if (m_laser.m_projectile.transform.localScale.y < .25f)
                            {
                                CalcKnockBack();
                                KnockBack();
                            }
                        }
                    }
                    else
                    {
                        CalcKnockBack();
                        KnockBack();
                    }
                }

                //shoot bullet
                m_weapon.Shoot(transform.position, shootDir, playerID, this, Color.blue, type, m_PowerUp);
                ++accolade_shotsFired;
            }

            if (controller.shootButton.JustReleased() && m_weapon)
            {
                m_weapon.StopFire();
            }
        }

        public void KnockBack()
        {
            //if player is not grounded
            if (!grounded && knockBack > 0)
            {
                //knock back player
                CalcKnockBack();
                playerRigid.AddForce((-shootDir * (moveSpeed * 20 * RandomKnockBack)) * Time.deltaTime, ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// Reset should be called
        /// when all players die
        /// and the game resets
        /// </summary>
        public void Reset()
        {
            m_PowerUp = BulletModifier.vanilla;
            m_weapon = null;
        }

        public void Respawn()
        {
            health = 50f;
            Tower.instance.TakeDamage(50f);
            m_UI.UpdateSliderOne(health);

            float angle = Random.value * Mathf.PI * 2;
            float x = Mathf.Cos(angle * 5) * Random.Range(3, 5f);
            float y = Mathf.Sin(angle * 5) * Random.Range(1.75f, 3f);

            transform.position = new Vector3(x, y, 0);
            transform.position += Tower.instance.transform.position + Tower.instance.spawnOffsetPosition;
        }

        /// <summary>
        /// Pending on the input stack Move 
        /// updates the movement vector according
        /// to input
        /// </summary>
        private void Move()
        {
            Vector2 stickDirection = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());
            
            FireAngle(stickDirection);
            walking = stickDirection != Vector2.zero;
            currentAngle = this.transform.eulerAngles.z;
            Steps();
            if (stickDirection != Vector2.zero)
            {
                stickDirection = Quaternion.AngleAxis(-45, Vector3.forward) * stickDirection;
                m_armaAnimator.SetBool("walking", true);

				lastDir = stickDirection;

                playerRigid.velocity += new Vector2(stickDirection.x, stickDirection.y).toIso() * moveSpeed * Time.deltaTime;
                accolade_distance += playerRigid.velocity.magnitude;
                return;
            }

			lastDir = stickDirection;

            m_armaAnimator.SetBool("walking", false);
        }

		/// <summary>
		/// moves the player in the ball until they slow to a halt
		/// whilst in state, 
		/// </summary>
		private void Ball()
		{
            if (m_weapon) { m_weapon.StopFire(); }
			Vector2 stickDirection = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());
			FireAngle(stickDirection);

			if (stickDirection != Vector2.zero)
			{
				stickDirection = Quaternion.AngleAxis(-45, Vector3.forward) * stickDirection;
				lastDir = stickDirection;
			}
			else
			{
				stickDirection = Quaternion.AngleAxis(-45, lastDir) * lastDir;
			}

			ballSpeed -= (moveSpeed / 2.5f) * Time.deltaTime;

			Vector2 movement = new Vector2(stickDirection.x, stickDirection.y).toIso() * ballSpeed * Time.deltaTime;

			playerRigid.velocity += movement;

			//if stopped, kick out of ball
			if (movement.magnitude <= 0.0f || ballSpeed <= 0.0f)
			{
				m_armaAnimator.SetTrigger("exitBall");
				ballSpeed = moveSpeed;
				m_state = ArmaPlayerStates.move;
			}
		}

        /// <summary>
        /// Called when the player state is set
        /// to Grounded. Uses player input and rotates 
        /// the crosshair
        /// </summary>
        private void Grounded()
        {
            m_armaAnimator.SetBool("walking", false);
            FireAngle(Vector2.zero);
        }

        private void FireAngle(Vector2 _moveAngle)
        {
            Vector2 stickAngle = new Vector2(controller.aimX.GetValue(), controller.aimY.GetValue());
            

            if (stickAngle != Vector2.zero && m_state != ArmaPlayerStates.ballin)
            {
                //Update the current angle of the player
                float angle = Mathf.Atan2(stickAngle.x, -stickAngle.y) * Mathf.Rad2Deg;

                //Set the animation angle
                ChangeAnimatorAngle(m_armaAnimator, "angle", angle);
                shootDir = stickAngle;

                //Normalizing stickAngle
                //  + Regular knockback amounts
                //  + Spread is also more regular
                //  - Can only fire in 8 directions now
                //shootDir = stickAngle.normalized;
            }

            else if (_moveAngle != Vector2.zero)
            {
                //Update the current angle of the player
                float angle = Mathf.Atan2(_moveAngle.x, -_moveAngle.y) * Mathf.Rad2Deg;
                ChangeAnimatorAngle(m_armaAnimator, "angle", angle);
                Vector2 movementAngle = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());
                shootDir = movementAngle;
            }
        }

        public void GiveHealth(float _health)
        {
            if (Random.value > .5f)
                HPRegenPS.Emit(1);
            accolade_unique += _health;
            health += _health;
            m_UI.UpdateSliderTwo(health);
        }

        /// <summary>
        /// RespawnPlayer should be called by any 
        /// GameObject which 'destroys'/'kills' the 
        /// player. RespawnPlayer places the player outside of 
        /// the game world and sets it state to respawning
        /// so it will not do anythung during Update!
        /// </summary>

        public void RemovePowerUp(BulletModifier _type)
        {

            if ((_type & BulletModifier.rapidFire) != 0)
            {
                m_weapon.mCoolDown *= 2;
            }
            if ((_type & BulletModifier.superSpeed) != 0)
                moveSpeed /= 2;

            m_PowerUp = BulletModifier.vanilla;
        }

        protected IEnumerator IRemovePowerUp(BulletModifier _type)
        {
            //Debug.Log("Removing " + _type);
            yield return new WaitForSeconds(10);

            if ((_type & BulletModifier.rapidFire) != 0)
            {
                m_weapon.mCoolDown *= 2;
            }
            if ((_type & BulletModifier.superSpeed) !=0)
                moveSpeed /= 2;

            //I'm sorry Dan, it's for the greater good.
            //m_PowerUp &= ~_type;
            m_PowerUp = BulletModifier.vanilla;
        }

        protected IEnumerator teleportation()
        {
            for (int i = 0; i < 3; i++)
            {
                TPcharge.Play();
                SoundManager.instance.PlayClip(tpChargeSound);

                Bounds b = powerUpManager.instance.dropBounds[Random.Range(0, powerUpManager.instance.dropBounds.Count - 1)];

                float y = Random.Range(b.min.y, b.max.y);
                float x = Random.Range(b.min.x, b.max.x);

                yield return new WaitForSeconds(2.25f);
                transform.position = new Vector3(x, y, 0);
                SoundManager.instance.PlayClip(tpPoofSound);
                TPpoof.Play();
                yield return new WaitForSeconds(1f);
            }
        }
        #endregion       

        #region Actor Behaviours
        public override void TakeDamage(float damage)
        {
            //if in the ball, no damage is taken
            if (m_state == ArmaPlayerStates.ballin || m_state == ArmaPlayerStates.grounded)
            {
                return;
            }
            controller.StartVibration(1.0f);
            base.TakeDamage(damage);
            m_UI.UpdateSliderTwo(health);
            ++accolade_timesShot;
            if (health <= 0)
            {
                if (m_weapon)
                {
                    m_weapon.StopFire();
                }
                ArmaPlayerManager.instance.RemovePlayer(this);
            }
        }
        #endregion
    }
}