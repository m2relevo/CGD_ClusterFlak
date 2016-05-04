/// <summary>
/// Base class for ArmaTillery players - based from moveNShoot class
/// Created and implemented by Sam Endean on 15/01/2016
/// Based off of ArmaPlayer base class
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using DG.Tweening;
using ArmadHeroes;

namespace Armatillery
{
    public class ArmaEnemy : ArmadHeroes.Actor
    {
        #region Public Members
        [System.NonSerialized]
        //player props
        public float shootProximity = 10f,
        angle = 0f;
		public Vector3 m_spawnPos = Vector3.zero, dropPoint;
        public int scoreValue = 10;
        #endregion

        #region Private Memebers
		private bool followIngPath; //if not following a path (dropped from a helicoptor etc) it should just move straight to the tower
		private float activateTimer = 0f;
        bool exploding = false;
        #endregion

        #region Protected Memebers
        //protected int m_pathPos;
        protected ArmaEnemyStates m_state;
        //protected List<WorldTile> m_currentPath = new List<WorldTile>(); //current path being followed

        protected GameObject currentPathNode; //Where was my last node
        protected GameObject targetPathNode; //Where is my next node
        protected int currentPathPosition = 0; //Where am I on the path
        protected int pathIndex = int.MaxValue; //Which path am I following (MAX IF NOT ASSIGNED)
        #endregion

        #region Unity callbacks
        protected virtual void Awake()
        {
            Init();

            type = ActorType.Enemy;//set the type
            moveSpeed = 16.0f;

            //set initial player state
            m_state = ArmaEnemyStates.followPath;
			m_armaAnimator = this.GetComponent<Animator> ();
        }

        protected virtual void Update()
        {
            switch (GameManager.instance.state)
            {
                case ArmadHeroes.GameStates.game:
                    Tick();
                    break;
                case ArmadHeroes.GameStates.pause:
                    break;
                case ArmadHeroes.GameStates.gameover:
                    break;
                default:
                    break;
            }
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            #region Projectiles
            //check if col is a projectile
            if (col.gameObject.GetComponent<Projectile>() && health > 0)
            {
                Projectile _projectile = col.gameObject.GetComponent<Projectile>();
                //make sure owner of projectile is not the same as the ActorType
                if (_projectile.type != type)
                {
                    BulletModifier playerPowerUp = _projectile.m_mods;

                    //default all bullets will deal damage
                    TakeDamage(_projectile.damage);
                    if ((playerPowerUp & BulletModifier.explodeOnDeath) != 0)
                    {
                        exploding = true;
                    }
                }
            }
            #endregion

            //if the enemy has died
            if(health <=0)
            {
                if (col.GetComponent<Projectile>())
                {
                    Projectile _proj = col.GetComponent<Projectile>();


                    BulletModifier playerPowerUp = _proj.m_mods;
                    if ((playerPowerUp & BulletModifier.explodeOnDeath) != 0)
                    {
                        GameObject explosion = ExplosionManager.instance.GetBigExplosion();
                        explosion.transform.position = gameObject.transform.position;
                        explosion.SetActive(true);
                        explosion.GetComponent<Explosion>().InitExplode(1f,_proj.owner);
                    }

                    if (_proj.owner.GetComponent<ArmaPlayer>())
                    {
                        ((ArmaPlayer)_proj.owner).UpdateScore(scoreValue);                           
                        _proj.gameObject.SetActive(false);
                    }
                }
                //remove from list of spawned
                EnemyManager.instance.m_spawnedenemies.Remove(this.gameObject);
            }
        }

        /// <summary>
        /// See if the enemy has collided with the tower, if so, begin attacking the tower
        /// </summary>
        /// <param name="_col">Col.</param>
        void OnCollisionEnter2D(Collision2D _col)
        {
            //if the collided object has the tower script attached, it is the tower
            if (_col.gameObject.GetComponent<Tower>())
            {
                //switch to attack the tower
                m_state = ArmaEnemyStates.attackTower;
            }
        }
        #endregion

        #region 'Custom' callbacks
        public void Init()
        {
            moveSpeed = 6f;
            health = 1f;
        }
        /// <summary>
        /// Controlled update for objects, 
        /// Tick is called in the update 
        /// pending on the current game state
        /// </summary>
        protected void Tick()
        {
            switch (m_state)
            {
			case ArmaEnemyStates.followPath:
				if (pathIndex != int.MaxValue)
				{
					FollowPath ();

                    if (m_weapon)
                    {
                        Shoot(DetectPlayers());
                    }
				}
				else //there is no path, move towards the tower
				{
					//set activate timer (for hitting the ground)
					activateTimer = 3f;

					//generate an A* path to the tower
					GenFreePath ();

					//start following it
					FollowPath ();
				}
                break;
            case ArmaEnemyStates.sprintPath:
                if (pathIndex != int.MaxValue)
                {
                    FollowPath();
                }
                else //there is no path, move towards the tower
                {
                    //set activate timer (for hitting the ground)
                    activateTimer = 3f;

                    //generate an A* path to the tower
                    GenFreePath();

                    //start following it
                    FollowPath();
                }
                break;
            case ArmaEnemyStates.attackTower:
                AttackTower();
                break;
            case ArmaEnemyStates.idle:
                break;
            default:
                break;
            }
        }
        #endregion

        #region Enemy behaviours
        /// <summary>
        /// Called when enemy
        /// needs to sprint to
        /// the base
        /// </summary>
        public void Sprint()
        {
            if (m_weapon != null) { m_weapon.StopFire(); }
            m_state = ArmaEnemyStates.sprintPath;
            m_armaAnimator.SetTrigger("enterBall");
            StartCoroutine(SpeedUp());
        }

        /// <summary>
        /// Increases enemy speed using quartic function
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpeedUp()
        {
            float startSpeed = moveSpeed;
            float windUpTime = 2f;
            float maxSpeed = startSpeed * 3;

            //This is ugly, sorry.. didn't want to break anything
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

            float t = 0f;
            while (t < windUpTime)
            {
                ps.emissionRate = moveSpeed * 10;
                moveSpeed = QuartIn(t, startSpeed, maxSpeed, windUpTime);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// http://wpf-animation.googlecode.com/svn/trunk/src/WPF/Animation/PennerDoubleAnimation.cs
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public float QuartIn(float t, float b, float c, float d)
        {
            return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
        }

        /// <summary>
        /// Spawn is used by the enemy manager 
        /// to spawn in enemies with default settings
        /// </summary>
        public void Spawn()
        {
            type = ActorType.Enemy;//set the type
            //m_armaAnimator.SetTrigger("exitBall");
            m_override.SetCharacter(ActorName);

            //Random weapon
            float val = Random.value;
            val += 0.04f * WaveManager.instance.m_wave;
            if(val > 0.95)
            {
                EquipWeapon(m_flamethrower);
            }
            else if (val > 0.8)
            {
                EquipWeapon(m_laser);
            }
            else if( val > 0.6)
            {
                EquipWeapon(m_shotgun);
            }
            else
            {
                EquipWeapon(m_machinegun);
            }
            //End random weapon

            pathIndex = PathManager.instance.RandomPath();
            currentPathPosition = 0;
            transform.position = PathManager.instance.FirstNodeOnPath(pathIndex).transform.position;
            m_state = ArmaEnemyStates.followPath;
            moveSpeed = 12;
            health = 1.0f;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        /// <summary>
        /// Shoot is overridden by derived classes to allow 
        /// variant shooting styles
        /// </summary>
        public void Shoot(Vector3 _closestPlayerDir)
        {
            //only shoots if conditions are met
            if (_closestPlayerDir == Vector3.zero)
            {
                return;
            }
            //check weapon
            m_weapon.Shoot(transform.position, _closestPlayerDir, 0, this, Color.red, ActorType.Enemy);
        }

        /// <summary>
        /// Detect players within a certain proximity
        /// if players are found, shoot (this will
        /// only be called by enemies with shooting
        /// capabilities)
        /// </summary>
        public Vector3 DetectPlayers()
        {
            int highestPrio = -1;
            float smallestDistOverScore = float.PositiveInfinity;
            for (int i = 0; i < ArmaPlayerManager.instance.m_spawnedPlayers.Count; i++)
            {
                float tempDist = Vector3.Distance(transform.position, ArmaPlayerManager.instance.m_spawnedPlayers[i].transform.position),
                tempDisOvScore = tempDist / ArmaPlayerManager.instance.m_spawnedPlayers[i].chevron_score;

                if (tempDisOvScore < smallestDistOverScore || tempDisOvScore == float.PositiveInfinity)
                {
                    smallestDistOverScore = tempDisOvScore;
                    highestPrio = i;
                }

            }
            //return the direction of the closest player if within proximity, otherwise (0, 0, 0)
            Vector3 dir = Vector3.zero;

            if (highestPrio != -1)
            {
                //if it found a player within proximity, return the direction to it from this enemy
                if (Vector3.Distance(transform.position, ArmaPlayerManager.instance.m_spawnedPlayers[highestPrio].transform.position) <= shootProximity)
                {
                    dir = (ArmaPlayerManager.instance.m_spawnedPlayers[highestPrio].transform.position - transform.position).normalized;
                }
                else //else just do the closest instead
                {
                    return GetClosestPlayer();
                }
            }

            //Destroy (closestPlayer);

            return dir;
        }

		public Vector3 GetClosestPlayer()
		{
			//GameObject closestPlayer = new GameObject ();
			int closestPlayer = -1;
			float smallestDist = float.PositiveInfinity;
			for (int i = 0; i < ArmaPlayerManager.instance.m_spawnedPlayers.Count; i++)
			{
				if ((i == 0))
				{
					closestPlayer = i;
					smallestDist = Vector3.Distance(transform.position, ArmaPlayerManager.instance.m_spawnedPlayers[i].transform.position);
				}

				if (Vector3.Distance(transform.position, ArmaPlayerManager.instance.m_spawnedPlayers[i].transform.position) < smallestDist)
				{
					smallestDist = Vector3.Distance(transform.position, ArmaPlayerManager.instance.m_spawnedPlayers[i].transform.position);
					closestPlayer = i;
				}
			}
			//return the direction of the closestplayer if within proximity, otherwise (0, 0, 0)
			Vector3 dir = Vector3.zero;

			//if it found a player within proximity, return the direction to it from this enemy
			if (smallestDist <= shootProximity && closestPlayer != -1)
			{
				dir = (ArmaPlayerManager.instance.m_spawnedPlayers[closestPlayer].transform.position - transform.position).normalized;
			}

			//Destroy (closestPlayer);
			return dir;
		}

        /// <summary>
        /// Follows the path it is currently on
        /// </summary>
        private void FollowPath()
		{
			//if still floating to the ground, just count the timer down whilst moving down towards the drop point
			if (activateTimer > 0f)
			{
				activateTimer -= Time.deltaTime;
				return;
			}

			Vector3 offset = new Vector3 (0, 0.15f, 0);

            GameObject nextNode = PathManager.instance.NextNodeOnPath(pathIndex, currentPathPosition);

            //try to move to the current m_pathPos
            Vector3 movement = (((Vector3)nextNode.transform.position + offset) - transform.position).normalized * ((moveSpeed / 10));
            transform.position += movement * Time.deltaTime;
            
			//Update the current angle of the player

            if (GetClosestPlayer() == Vector3.zero)
            {
                angle = Mathf.Atan2(movement.normalized.x, -movement.normalized.y) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Mathf.Atan2(GetClosestPlayer().normalized.x, -GetClosestPlayer().normalized.y) * Mathf.Rad2Deg;
            }

            ChangeAnimatorAngle(m_armaAnimator, "angle", angle);

			//transform.LookAt(((Vector3)m_currentPath [m_pathPos + 1].m_worldTileObject.transform.position + offset));
			

            m_armaAnimator.SetBool("walking", true);

			if (Vector3.Distance(transform.position, nextNode.transform.position + offset) <= 0.0625f)
            {
                //if the current m_pathPos is 1 less than the number of tiles in the path, it is on the last tile
                if (currentPathPosition > PathManager.instance.NodeCountInPath(pathIndex) -2)
                {
                    m_state = ArmaEnemyStates.attackTower;
                    return;
                }
                else
                {
                    currentPathPosition++;
                }
            }
        }

		/// <summary>
		/// Generate an A* path from the drop point to the tower. 
		/// </summary>
		protected void GenFreePath ()
		{
			Vector3 offset = new Vector3 (0, 0.15f, 0);

			//set the enemy to float to the drop point
			Tweener m_obj = this.gameObject.transform.DOMove(dropPoint + offset, activateTimer, false);
			m_obj.SetEase (Ease.OutCubic);

			//generate a free path
			//m_currentPath = WorldGenerator.instance.CreatePath(dropPoint + offset, dropPoint + offset);
		}

        /// <summary>
        /// Called by the enemy manager when "spawned"
        /// </summary>
        /// <param name="_path">the path for the enemy to follow to the tower</param>
        public void SetPath(List<WorldTile> _path)
        {
            //m_currentPath = _path;
            //m_pathPos = 0;
        }

        /// <summary>
        /// Attack the tower (done once at the end of the path)
        /// </summary>
        private void AttackTower()
        {
            //wait for attack countdown
            health = 0;
            GameObject explosion = ExplosionManager.instance.GetExplosion();
            explosion.transform.position = gameObject.transform.position;
            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().InitExplode(1f);
            SoundManager.instance.PlayClip(EnemyManager.instance.ExplosionSpawn,false, 0.5f);
            CameraShakeHandler.Instance.Shake();

            Tower.instance.TakeDamage(50.0f);
            TakeDamage(1);
        }
        #endregion

        #region Actor Behaviours
        public override void TakeDamage(float damage)
        {
            if (exploding)
            {
                GameObject explosion = ExplosionManager.instance.GetBigExplosion();
                explosion.transform.position = gameObject.transform.position;
                explosion.SetActive(true);
                explosion.GetComponent<Explosion>().InitExplode(1f);
            }
            base.TakeDamage(damage);
            if(health<=0)
            {
                if (m_weapon != null) { m_weapon.StopFire(); }
                EnemyManager.instance.m_spawnedenemies.Remove(this.gameObject);
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}