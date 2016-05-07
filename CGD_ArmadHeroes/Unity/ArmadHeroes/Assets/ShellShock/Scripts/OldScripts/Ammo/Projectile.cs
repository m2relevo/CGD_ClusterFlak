using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class Projectile : MonoBehaviour
    {
        #region Protected Member Variables
        protected int mDamage;

        protected int mOwnerID;

        //Maximum number of ricochets allowed
        protected int mMaxRicochets;

        //Current number of ricochets
        protected int mRicochets;

        //Projectiles RigidBody2D
        Rigidbody2D mRigidBody2D;

        public GameObject explosion;
        Object thisExplosion;

        #endregion

        public GameObject accManReference;

        protected bool isActive;

        private int mCurrentBounces;
        private int mMaximumBounces = 3;

        //Time in seconds since this projectile was created
        private float mTimeAlive;

        public bool Active
        {
            get
            {
                return isActive;
            }
        }
        public float TimeAlive
        {
            get
            {
                return mTimeAlive;
            }
        }
        public int Damage
        {
            get
            {
                return mDamage;
            }
        }
        public int OwnerID
        {
            get
            {
                return mOwnerID;
            }
        }

        public void MakeInactive()
        {
            mTimeAlive = 0;
            isActive = false;
            gameObject.SetActive(false);
        }

        public void MakeActive()
        {
            gameObject.SetActive(true);
            isActive = true;

        }

        void Awake()
        {
            //Get RigidBody2D to apply physics
            mRigidBody2D = GetComponent<Rigidbody2D>();
        }

        public void Fire(Vector2 position, float speed, float angle, int damage, int ownerID, int numBounces)
        {
            gameObject.name = "Player_" + ownerID + "'s Projectile";

           // accManReference.GetComponent<ShellShock.StatTracker>().shotFired = true;

            mOwnerID = ownerID;

            mDamage = damage;

            //Get RigidBody2D to apply physics
            mRigidBody2D = GetComponent<Rigidbody2D>();

            //Sets initial position to the exit position of the weapon
            transform.position = position;

            //Converts angle to Vector2
            mRigidBody2D.velocity = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * speed;
            //SetRotation of projectile to direction it was fired

            UpdateRotation();

            mMaximumBounces = numBounces;

			//accManReference.GetComponent<ShellShock.StatTracker>().shotFired = false;

            Destroy(gameObject, 5f);	
        }

        void Update()
        {
            if (mCurrentBounces == mMaximumBounces)
            {
                // ContactPoint2D contact = bulletCollider.contacts[0];
                thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

                Destroy(thisExplosion, 0.2f);
                Destroy(gameObject);
            }

            mTimeAlive += Time.deltaTime;
            UpdateRotation();
        }
        void UpdateRotation()
        {
            Vector3 dir = mRigidBody2D.velocity.normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }

        void OnCollisionEnter2D(Collision2D bulletCollider)
        {
            mCurrentBounces++;

            if (bulletCollider.gameObject.GetComponent<RewiredController>().isBallin == false)
            {
                thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

                Destroy(thisExplosion, 0.2f);
                Destroy(gameObject);
            }
        }
    }
}
