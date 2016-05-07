/// <summary>
/// Weapon.cs
/// Base class for integration weapons 
/// Created by Chris, M., David, G & Daniel, W ~ 20/04/2016
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Astrodillos;
using Armatillery;

namespace ArmadHeroes
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Public Members
        public List<Projectile> m_projectiles = new List<Projectile>();
        public bool infiniteAmmo = false;
        protected int poolHead = 0;

        public AudioClip ShotSound;
        public float mRecoilForce;
        public float mCurrentCoolDown;
        public GameObject m_projectile;//prefab/GO bullet for gun to pool and fire
        public bool shells = false;
		public bool firesBullets = true; //used to determine whether to add to projectiles fired stat
    

        #endregion

        #region Protected Members
        protected Actor player;
        protected float mDamage;
        protected float mSpreadAngle;
		protected bool mRandomSpread = true;
        //protected float mCoolDown;
        public float mCoolDown;
        protected float mProjectileSpeed;
        protected int mMagazineCapacity;
        protected int mCurrentRounds;
		protected int maxRounds = 20;
        protected int mNumberOfBounces;
		protected int mLayer;
		protected float mVibrationStrength = 1.0f;
		protected float mVibrationTime = 0.1f;
        #endregion

        #region Private Members
        #endregion

        #region BulletCasingParticleSystem Members
        public int controllerID = 0;
        public int playerId = 0;
        public ParticleSystem bulletCasingParticleSystem,muzzleFlashParticleSystem;
        public GlobalPlayerManager globalPlayerManager;
        private Vector2 aimVector;
        public int mOrientation;

        private ArmadHeroes.Controller controller
        {
            get { return ControllerManager.instance.GetController(controllerID); }
            set { }
        }
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            globalPlayerManager = GlobalPlayerManager.instance;
        }
        void Start()
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
           // bulletCasingParticleSystem = gameObject.GetComponent<ParticleSystem>();
			mLayer = gameObject.layer;

            if (globalPlayerManager != null)
            {
                controllerID = globalPlayerManager.GetPlayerData(playerId).controllerIndex;
            }

        }
        void Update()
        {
            Tick();
            ProcessBulletCasingDirection();
        }
        #endregion

        /// <summary>
        /// Called Every Update
        /// updates counters ect
        /// </summary>
        public virtual void Tick()
        {
            //update 
            mCurrentCoolDown -= mCurrentCoolDown <= 0.0f ? 0.0f : 1.0f * Time.deltaTime;
        }
      
        /// <summary>
        /// Can be override by
        /// other weapons
        /// </summary>
        /// <param name="_spawnPos">Bullet Start</param>
        /// <param name="_shootDir">Direction of travel</param>
        /// <param name="_shooter">Player ID who fired it</param>
        /// <param name="_owner">Reference to Actor who fired it</param>
        /// <param name="_type">Actor type</param>
        /// <param name="_modifiers">Power-ups for projectile</param>
		public virtual void Shoot(Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner,Color colour, ActorType _type = ActorType.Player, BulletModifier _modifiers = BulletModifier.vanilla, float height = 0)
        {
            if (infiniteAmmo)
            {
                mCurrentRounds = 15;
            }
            if (mCurrentRounds > 0)
            {
                if (CheckCoolDown())
                {

                    TriggerFireSound();
                    UseAmmo();
					FireProjectile (_spawnPos,_shootDir,_shooter,_owner,_type,_modifiers,height);


                    //Vibrate controller
                    if (_owner.gameObject.GetComponent<PlayerActor>())
                    {
                        _owner.gameObject.GetComponent<PlayerActor>().controller.StartVibration(mVibrationStrength, mVibrationTime);
                    }

                    //Only emit when on ground
                    if (bulletCasingParticleSystem != null && height == 0)
                    {
                        bulletCasingParticleSystem.Emit(1);
                    }
                    if (muzzleFlashParticleSystem != null)
                    {
                        muzzleFlashParticleSystem.Emit(1);
                    }
                }
            }
        }

		protected void FireProjectile(Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner, ActorType _type = ActorType.Player, BulletModifier _modifiers = BulletModifier.vanilla, float height = 0){

			//grab bullet from pool and ready it for firing
			m_projectiles[poolHead].transform.position = _spawnPos;// + (_shootDir.normalized); - Can spawn directly from emit transform instead
			m_projectiles [poolHead].transform.localEulerAngles = new Vector3(0,0, Mathf.Atan2 (_shootDir.y, _shootDir.x) * Mathf.Rad2Deg);
			Projectile _curBullet = m_projectiles[poolHead].GetComponent<Projectile>();
			//set bullet direction m_projectiles[poolHead].GetComponent<Projectile>()
			_curBullet.direction = _shootDir.normalized;
			if (mRandomSpread) {
				_curBullet.direction = GetSpreadAngle(_curBullet.direction);
			}

			//hand projectile props
			_curBullet.damage = mDamage;
			_curBullet.moveSpeed = mProjectileSpeed;
			_curBullet.m_mods = _modifiers;
			_curBullet.owner = _owner;
			_curBullet.type = _type;
			_curBullet.gameObject.layer = mLayer;
            //Enable the bullet.
            _curBullet.gameObject.SetActive(true);
            //THEN fire the bullet. If the bullet is not enabled before it is fired, then a force cannot be applied to the bullet when it is fired.
            _curBullet.Fire(_shooter, height);
            		
			//increment pool head
			poolHead = poolHead == m_projectiles.Count - 1 ? poolHead = 0 : ++poolHead;
			mCurrentCoolDown = mCoolDown;
		}

        #region Helper Functions
        /// <summary>
        /// Calcs shot spread for weapons
        /// </summary>
        /// <param name="aimAngle"></param>
        /// <returns></returns>
        protected Vector3 GetSpreadAngle(Vector3 shootDir)
        {
            return (shootDir + new Vector3(Random.Range(-mSpreadAngle, mSpreadAngle), Random.Range(-mSpreadAngle, mSpreadAngle), 0f)).normalized;
        }

		/// <summary>
		/// Returns the number of rounds
		/// </summary>
		public int GetCurrentAmmo()
		{
			return mCurrentRounds;
		}

        /// <summary>
        /// Check validity of weapon cool down
        /// </summary>
        /// <returns></returns>
        public bool CheckCoolDown()
        {
            return mCurrentCoolDown <= 0.0f;
        }

		/// <summary>
		/// Set the collision layer for weapons/projectiles
		/// </summary>
		/// <param name="layerName">Layer name.</param>
		public void SetLayer(int layer)
		{
			mLayer = layer;
			gameObject.layer = mLayer;
		}

        /// <summary>
        /// Using List of cached sounds ]
        /// trigger one
        /// </summary>
        protected void TriggerFireSound()
        {
            SoundManager.instance.PlayClip(ShotSound, false, 0.25f);
        }
        //Stops Firing (laser & flame specific)
        public virtual void StopFire(){}

        /// <summary>
        /// Pool bullets for gun
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected virtual void ObjectPoolBullets()
        {
            //on awake object pool bullets for player
            for (int i = 0; i < 150; i++)
            {
                GameObject _bullet = GameObject.Instantiate(m_projectile);
                //assign name
                _bullet.name = "bullet " + i;
                //set state
                _bullet.SetActive(false);
                //hide in hierarchy
                //_bullet.transform.parent = this.transform;
                _bullet.hideFlags = HideFlags.HideInHierarchy;
                //tag
                _bullet.tag = "Bullet";
                //add to pool
                m_projectiles.Add(_bullet.GetComponent<Projectile>());
            }
        }

        /// <summary>
        /// Sets up the weapons characteristics
        /// </summary>
        /// <param name="_damage"></param>
        /// <param name="_spread"></param>
        /// <param name="_coolDown"></param>
        /// <param name="_speed"></param>
        /// <param name="_recoil"></param>
        protected void SetWeaponCharacteristics(float _damage, float _spread, float _coolDown, float _speed, float _recoil, int _magCap)
        {
            mDamage = _damage;
            mSpreadAngle = _spread;
            mCoolDown = _coolDown;
            mProjectileSpeed = _speed;
            mRecoilForce = _recoil;
            mMagazineCapacity = _magCap;
        }

        protected virtual void UseAmmo()
        {
            mCurrentRounds--;
        }

		public virtual void Reload()
		{
			mCurrentRounds = maxRounds;
		}

        public virtual void ProcessBulletCasingDirection()
        {
			if (bulletCasingParticleSystem == null) {
				return;
			}
            aimVector.x = controller.aimX.GetValue();
            aimVector.y = controller.aimY.GetValue();

            Vector2 inputAimDirection = new Vector2(aimVector.x, aimVector.y);


            if (inputAimDirection.magnitude > 0.4)
            {
                //Converts the angle between 0 and 360 degrees to an integer between 0 and 8 which corresponds to a sprite facing that angle
                int tempSpriteNumber = Mathf.RoundToInt(GetAngleWithSign(inputAimDirection));

                mOrientation = (tempSpriteNumber > 360 - (45 / 2) ? tempSpriteNumber - 360 - (45 / 2) : tempSpriteNumber) / 45;



                switch (mOrientation)
                {
                    case 0:

                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                       
                        break;
                    case 1:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 330);
                        break;
                    case 2:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 3:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 210);

                        break;
                    case 4:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;

                    case 5:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 150);
                        break;
                    case 6:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 7:
                        bulletCasingParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 30);
                        break;
                }

           
            }

        }

        public virtual float GetAngleWithSign(Vector2 dir)
        {
            float sAngle = Mathf.Sign(Vector3.Cross(dir, Vector3.up).z) * Mathf.RoundToInt(Vector2.Angle(dir, Vector2.up));
            return sAngle < 0 ? 360 + sAngle : sAngle;
        }
        #endregion
    }
}
