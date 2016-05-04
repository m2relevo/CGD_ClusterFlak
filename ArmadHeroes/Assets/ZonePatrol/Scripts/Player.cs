using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol
{
    public enum WeaponsType
    {
        Shootgun,
        Machinegun,
        Pistol
    }

    public class Player : PlayerActor
    {

        #region Public Player
        public float maxHealth = 100;
        public Vector3 spawnPoint;
        public ArmaCanvas HUD;
        public Color color;
        public List<Weapon> weaponsList;
        public float respawnTime = 2.0f;
        public float invulnerabilityTime = 2.5f;
        public float blinkInterval = 0.1f;
        public AudioClip hurtSound;
        #endregion


        private bool reSpawning = false;
        private float respawnTimer = 0;
        private float invulnerabilityTimer = 0; //god mode time
        private float blinkTimer = 0; //god mode time

        void Start()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            HUD.Deactivate();
            EquipWeapon(m_pistol);
            m_weapon.infiniteAmmo = true;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            switch (GameManager.instance.state)
            {
                case ArmadHeroes.GameStates.game:

                    // Player can't do anything before round start !
                    if (!GameBehaviours.getInstance().startRound)
                    {
                        if (!reSpawning)
                        {
                            Tick();
                            Move();
                            Shoot();

                            if (controller.hudButton.JustPressed())
                            {
                                HUD.Activate();
                            }
                        }
                        else
                        {
                            respawnTimer -= Time.deltaTime;
                            if (respawnTimer <= 0)
                            {
                                reSpawning = false;
                                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                    }
                    break;
                case ArmadHeroes.GameStates.pause:
                    break;
                case ArmadHeroes.GameStates.gameover:
                    break;
                default:
                    break;
            }
        }

        public void EquipeWeaponByType(WeaponsType type)
        {
            switch(type)
            {
                case WeaponsType.Shootgun:
                    EquipWeapon(m_shotgun);
                    break;

                case WeaponsType.Machinegun:
                    EquipWeapon(m_machinegun);
                    break;

                case WeaponsType.Pistol:
                    EquipWeapon(m_pistol);
                    break;
            }
        }

        public void Init(int id, int controllerId, string name)
        {
            ActorName = name;
            m_override.SetCharacter(name);
			m_controllerID = controllerId;
			playerID = id;
        }

        public void Shoot()
        {
            if (controller.shootButton.IsDown() && m_weapon)
            {
                if(m_weapon.CheckCoolDown())
                {
                    Debug.Log("Fired");
                    m_weapon.Shoot(gameObject.transform.localPosition, shootDir, playerID, this, Color.white);
                    accolade_shotsFired++;
                }
            }

            if (controller.shootButton.JustReleased() && m_weapon)
            {
                m_weapon.StopFire();
            }
        }

        private void Move()
        {
            // Angle while walking
            Vector2 walkDirection = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());
            updateLookAngle(walkDirection);

            if (walkDirection != Vector2.zero)
            {
                //direction = Quaternion.AngleAxis(-45, Vector3.forward) * direction;
                m_armaAnimator.SetBool("walking", true);
                gameObject.transform.localPosition += new Vector3(walkDirection.x, walkDirection.y, 0) * moveSpeed * Time.deltaTime;
                ++accolade_distance;
                return;
            }
            m_armaAnimator.SetBool("walking", false);
        }

        public override void TakeDamage(float damage)
        {
            Debug.Log("Damage Take: " + damage);
            base.TakeDamage(damage);

            // Update Health Bar
            HUD.UpdateSliderTwo(Mathf.Clamp(health, 0, 100));
        }

        public bool isAlive()
        {
            return health > 0;
        }

        public void addScore()
        {
            chevron_score += 10;
        }

        public void addZoneScore()
        {
            accolade_unique++;
        }

        public int getScore()
        {
            return chevron_score;
        }

        public void sendData()
        {
            GlobalPlayerManager.instance.SetDebriefStats(playerID, chevron_score, accolade_timesShot, accolade_distance, accolade_distance, accolade_shotsFired, accolade_unique);
        }

        public void updateData(int score, float sponge, float camper, float runner, float rambo, float unique)
        {
            chevron_score = score;
            accolade_timesShot = sponge;
            accolade_distance = camper;
            accolade_distance = runner;
            accolade_shotsFired = rambo;
            accolade_unique = unique;
        }

        public int getChevronScore()
        {
            return chevron_score;
        }

        public float getTimesShot()
        {
            return accolade_timesShot;
        }

        public float getDistanceWalked()
        {
            return accolade_distance;
        }

        public float getShotsFired()
        {
            return accolade_shotsFired;
        }

        public float getZonesCaptured()
        {
            return accolade_unique;
        }

        public virtual void OnTriggerEnter2D(Collider2D col)
        {
            // Check if player is alive and it is a bullet
            Projectile bullet = col.gameObject.GetComponent<Projectile>();
            if (isAlive() && bullet && bullet.callerID != playerID)
            {
                if (invulnerabilityTimer <= 0) // if not in god mode time then calculate damage
                {
                    TakeDamage(bullet.damage);
                    ++accolade_timesShot;
                    if (!isAlive())
                    {
                        SoundManager.instance.PlayClip (hurtSound, false, 0.5f);
                        respawn();
                    }
                }
            }
        }

        protected override void Tick()
        {
            base.Tick();
            //if god mode time is more than 0
            if (!reSpawning && invulnerabilityTimer > 0)
            {
                //decrement
                invulnerabilityTimer -= Time.deltaTime;
                if(invulnerabilityTimer <= 0)
                {
                    //make sure the player stops blinking and remains visible after god mode time
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }

                blinkTimer -= Time.deltaTime;
                if ( blinkTimer <= 0)
                {
                    //blink the player sprite
                    gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
                    blinkTimer = blinkInterval;
                }
            }
        }

        public void respawn()
        {
            reSpawning = true;
            respawnTimer = respawnTime;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            invulnerabilityTimer = invulnerabilityTime;
            health = maxHealth;
            transform.position = spawnPoint;

            // Update Health Bar
            HUD.UpdateSliderTwo(Mathf.Clamp(health, 0, 100));
            HUD.Deactivate();
        }

        protected void changeSpriteAngle(float angle)
        {
            spriteRenderer.gameObject.transform.localScale = getScale(angle);

            //Set the animation angle to get the right sprite
            m_armaAnimator.SetFloat("angle", Mathf.Abs(angle));
        }

        private void updateLookAngle(Vector2 walkDirection)
        {
            // Angle while aiming
            Vector2 aimAngle = new Vector2(controller.aimX.GetValue(), controller.aimY.GetValue());

            if (aimAngle != Vector2.zero)
            {
                // Use the aim angle as the base player angle !
                float angle = Mathf.Atan2(aimAngle.x, -aimAngle.y) * Mathf.Rad2Deg;

                // update the animation angle
                changeSpriteAngle(angle);

                shootDir = aimAngle;
            }

            else if (walkDirection != Vector2.zero)
            {
                //Update the current angle of the player
                float angle = Mathf.Atan2(walkDirection.x, -walkDirection.y) * Mathf.Rad2Deg;
                changeSpriteAngle(angle);

                shootDir = walkDirection;
            }
        }

        private Vector3 getScale(float angle)
        {
            if (angle > 0f && angle < 180f)
            {
                return new Vector3(1f, 1f, 1f);
            }
            else
            {
                return new Vector3(-1f, 1f, 1f);
            }
        }

    }
}