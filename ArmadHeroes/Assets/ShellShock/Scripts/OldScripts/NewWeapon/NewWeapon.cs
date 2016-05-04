using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShellShock
{
    public enum WeaponState { CHARGING, RECOVERING, RELOADING }

    public enum FireMode { FULL_AUTO, SEMI_AUTO }

    public abstract class NewWeapon : MonoBehaviour
    {
        #region Protected Member Variables

        //Current state of the weapon
        [SerializeField]
        protected WeaponState mState;

        //Damage dealt per projectile
        protected int mDamage;

        //Time since gun started reloading
        protected float mTimeReloading;

        //Total time needed to reload
        protected float mTimeToReload;

        //Angle when weapon is most accurate
        protected float mSpreadAngle;

        //Fire rate in RPS (60 RPS is max as game runs at 60 fps)
        protected float mFireRate;

        //Time to charge up the weapon
        protected float mChargeTime;

        //Speed of projectile upon leaving the weapon
        protected float mProjectileSpeed;

        //Time taken in seconds until the weapon can be charged again
        protected float mRecoveryTime;

        protected float mTimeRecovering;

        //Time spent shooting the gun (ie how long the trigger has been held)
        protected float mTimeCharging;

        //Total number of projectiles that can be held
        protected int mTotalRounds;

        //Number of rounds held
        protected int mCurrentRounds;

        //Capacity of 'magazine'
        protected int mMagazineCapacity;

        //Number of rounds currently loaded
        protected int mRoundsInMagazine;

        //Recoil is applied in opposite direction to muzzle velocity
        protected float mRecoilForce;

        //Player that owns this weapon
        private RewiredController mOwner;

        //Maximum number of projectiles active
        protected int mMaxNumActiveProjectiles;

        public GameObject mProjectile;

        //Projectile pool
        public List<Projectile> mProjectilePool;

        //Positions in which the projectiles should spawn ie where the end of the barrel is in each of the player orientations (IN MODEL SPACE (ie Relative to the player))
        public Vector2[] mProjectileSpawnPositions;

        //Particle system for weapon charging
        public ParticleSystem mChargeEffect;

        //Particle system for weapon firing
        public ParticleSystem mFireEffect;

        //Particle system for weapon recoiling
        public ParticleSystem mRecoilEffect;

        protected int mNumberOfBounces;

        protected bool isReady = true;

        protected FireMode mFireMode;

       public bool infiniteAmmo;

        float[] mIsoAngles = { 0, 60, 90, 120, 180, 240, 270, 300 };

        public bool hasLaserSight;

        #region Getters & Setters
        public bool InfiniteAmmo
        {
            get
            {
                return infiniteAmmo;
            }
        }
        public int RemainingAmmo
        {
            get
            {
                return mRoundsInMagazine;
            }
        }
        public int MaximumAmmo
        {
            get
            {
                return mTotalRounds;
            }
        }
        public RewiredController Owner
        {
            get
            {
                return mOwner;
            }

            set
            {
                mOwner = value;
            }
        }

        #endregion

        #endregion


        #region Debug

        //Constructor for weapon
        void Init()
        {

        }

        //Shows cone of fire
        [SerializeField]
        bool showSpread;

        #endregion

        protected virtual void Start()
        {
            mDamage = 50;
            mTimeReloading = 0f;
            mTimeToReload = 2f;
            mSpreadAngle = 10f;
            mFireRate = 0.1f;
            mChargeTime = 0f;
            mProjectileSpeed = 100f;
            mRecoveryTime = 0f;
            mTimeRecovering = 0f;
            mTimeCharging = 0f;
            mTotalRounds = 100;
            mCurrentRounds = 100;
            mMagazineCapacity = 10;
            mRoundsInMagazine = 10;
            mRecoilForce = 0f;
            mNumberOfBounces = 1;
            mMaxNumActiveProjectiles = 30;

            if (hasLaserSight)
            {
                gameObject.AddComponent<LaserSight>();
            }
        }


        #region ProjectilePool
        //protected void InitialiseProjectilePool()
        //{
        //    mProjectilePool = new List<Projectile>();

        //    for (int i = 0; i < mMaxNumActiveProjectiles; i++)
        //    {
        //        GameObject p = Instantiate(mProjectile);
        //        mProjectilePool.Add(p.GetComponent<Projectile>());
        //        p.GetComponent<Projectile>().MakeInactive();
        //        mProjectilePool[i].transform.parent = gameObject.transform;
        //    }
        //}

        ////When this weapon is picked up it is parented to the player
        //public void Pickup(PlayerLogic owner)
        //{
        //    mOwner = owner;
        //    transform.parent = owner.gameObject.transform;
        //}
        //int GetNumActiveProjectiles()
        //{
        //    int num = 0;
        //    for (int i = 0; i < mProjectilePool.Count; i++)
        //    {
        //        if (mProjectilePool[i].Active)
        //        {
        //            num++;
        //        }
        //    }
        //    return num;
        //}

        ////Returns a list of inactive projectiles
        //List<NewProjectile> GetInactiveProjectiles()
        //{
        //    List<NewProjectile> inactiveProjectiles = new List<NewProjectile>();
        //    for (int i = 0; i < mProjectilePool.Count; i++)
        //    {
        //        if (!mProjectilePool[i].Active)
        //        {
        //            inactiveProjectiles.Add(mProjectilePool[i]);
        //        }
        //    }
        //    return inactiveProjectiles;
        //}

        ////Returns first available projectile. Else finds oldest projectile and uses that.
        //NewProjectile GetNextAvailableProjectile()
        //{
        //    NewProjectile oldestProjectile;
        //    oldestProjectile = mProjectilePool[0];

        //    for (int i = 0; i < mProjectilePool.Count; i++)
        //    {
        //        if (mProjectilePool[i].TimeAlive > oldestProjectile.TimeAlive) oldestProjectile = mProjectilePool[i];
        //        if (!mProjectilePool[i].Active)
        //        {
        //            return mProjectilePool[i];
        //        }
        //    }
        //    //IF no inactive projectile is in the pool set the oldest active projectile to be inactive
        //    oldestProjectile.MakeInactive();
        //    return oldestProjectile;
        //}
        #endregion

        float GetSpreadAngle(float aimAngle)
        {
            return aimAngle + (Random.Range(-mSpreadAngle / 2.0f, mSpreadAngle / 2.0f));
        }

        //Takes in the raw spawn point and returns a position relative to the player
        Vector2 GetRelativeProjectileSpawnPoint(Vector2 posOnWeapon)
        {
            return (Vector2)mOwner.transform.position + posOnWeapon;
        }

        public void Charge(int orientation)
        {
            //Debug.Log("ad");
            if (isReady)
            {
                if (mTimeCharging > mChargeTime)
                {
                    if (mFireMode != FireMode.FULL_AUTO)
                    {
                        StopCharging();
                    }
                    Shoot(orientation);
                    StartCoroutine(Recover());
                }
                else
                {
                    mTimeCharging += Time.deltaTime;
                }
            }
        }

        public void StopCharging()
        {
            mTimeCharging = 0f;
            isReady = false;
        }

        public void ResetAmmo()
        {
            mRoundsInMagazine = mMagazineCapacity;
            mCurrentRounds = MaximumAmmo;
        }

        IEnumerator Recover()
        {
            isReady = false;

            yield return new WaitForSeconds(mRecoveryTime);

            isReady = true;
        }

        IEnumerator Reload()
        {
            isReady = false;

            yield return new WaitForSeconds(mTimeToReload);

            Refresh();
            isReady = true;
        }

        //Refreshes ammo to full magazine
        void Refresh()
        {
            //Remove one magazines worth of ammo
            mCurrentRounds -= mMagazineCapacity;
            //Replenish magazine ammo to full;
            mRoundsInMagazine = mMagazineCapacity;
        }

        void Update()
        {
            //Debug.DrawRay(GetRelativeProjectileSpawnPoint(mProjectileSpawnPositions[mOwner.GetComponent<NewAiming>().Orientation]), new Vector2(Mathf.Cos(GetSpreadAngle(mOwner.GetComponent<NewAiming>().AimAngle) * Mathf.Deg2Rad), Mathf.Sin(GetSpreadAngle(mOwner.GetComponent<NewAiming>().AimAngle) * Mathf.Deg2Rad)));
        }

        public void UpdateLaserSight(int orientation)
        {
            gameObject.GetComponent<LaserSight>().UpdateOrientation(mIsoAngles[orientation]);
            gameObject.GetComponent<LaserSight>().UpdatePosition(GetRelativeProjectileSpawnPoint(mProjectileSpawnPositions[orientation]));
        }

        void Shoot(int orientation)
        {
            //NewProjectile projectileToBeFired = GetNextAvailableProjectile();

            //projectileToBeFired.gameObject.name = "SHOOTING";

            //projectileToBeFired.MakeActive();
            //Instantiate(projectileToBeFired);
            //TODO: Understand what this does
            //projectileToBeFired.Fire(GetRelativeProjectileSpawnPoint(mProjectileSpawnPositions[mOwner.GetComponent<NewAiming>().Orientation]), mProjectileSpeed, GetSpreadAngle(mOwner.GetComponent<NewAiming>().AimAngle), mDamage, mOwner.PlayerNumber);
            GameObject m = Instantiate(mProjectile);
            m.transform.parent = gameObject.transform;

           // m.GetComponent<Projectile>().Fire(GetRelativeProjectileSpawnPoint(mProjectileSpawnPositions[orientation]), mProjectileSpeed, GetSpreadAngle(mIsoAngles[orientation]), mDamage, mOwner.playerId, mNumberOfBounces);

            //Debug.Log(mProjectileSpeed);

            if (!infiniteAmmo)
            {
                mCurrentRounds--;
                mRoundsInMagazine--;

                if (mRoundsInMagazine <= 0)
                {
                    //StartCoroutine(Reload());
                }
                if (mCurrentRounds <= 0)
                {
                    //Drop();
                }
            }
        }

        void Drop()
        {
            //mOwner.DropWeapon();
        }
    }
}
