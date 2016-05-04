/*
 * Helicopter stolen from ShellShock adapted by AstroChris
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;
namespace Astrodillos
{
    public class HeliIntegration : MonoBehaviour
    {
        public static HeliIntegration Instance;

        public WeaponPickUp DropObject;

        Animator mAnimator;
        Rigidbody2D mRigidBody;
		SpriteRenderer mSpriteRenderer, mShadowRenderer;


        public float mStartDistance;
        public float mAccelleration;
        public float mMaxSpeed;
        public float mHeightOffset;
        public int mOrientation;
        public AudioClip chopper;
        public GameObject shadow;
        AudioSource heliSound = null;
        Vector3 mSelectedDropPoint;

        Vector2 mHeading;
        bool started = false;
        public bool isCarrying;
		Vector3 startPos;
        
        void Awake()
        {
           
            Instance = this;
            mRigidBody = GetComponent<Rigidbody2D>();
            mSpriteRenderer = GetComponent<SpriteRenderer>();
			mShadowRenderer = shadow.GetComponent<SpriteRenderer>();
            mAnimator = GetComponent<Animator>();
			startPos = transform.position;

        }

		public void Reset(){
			transform.position = startPos;
			started = false;
			mRigidBody.velocity = Vector2.zero;
			if (heliSound != null) {
				heliSound.Stop ();
			}
		}


        void Update()
        {
            if (started)
            {
                Move();
                if (mRigidBody.velocity.magnitude > 0.4f)
                {
                    UpdateOrientation();
                }

                if (isCarrying)
                {
                    CheckDrop();
                }
                if (transform.position.magnitude > mStartDistance)
                {
                    Respawn();
                }
               
                //PlaySounds();
				if (heliSound != null && heliSound.isPlaying) {
					SoundManager.instance.SetPan(transform.position, heliSound);

					heliSound.volume = mSpriteRenderer.isVisible ? 0.1f : 1 / (Vector3.Distance (transform.position, Vector3.zero) * 5);
				}

				mShadowRenderer.sortingOrder = SpriteOrdering.GetOrder (shadow.transform.position.y);
                
            }
            else
            {
                started = true;
                Respawn();
            }
        }

        void SelectDropPoint()
        {
			mSelectedDropPoint = Gametype_Astrodillos.instance.GetDropPoint ();
            mHeading = (mSelectedDropPoint + new Vector3(0, mHeightOffset, 0)) - transform.position;
        }

        void Move()
        {
            mRigidBody.AddForce(mHeading.normalized * mAccelleration * Time.deltaTime, ForceMode2D.Force);
            LimitSpeed();
        }

        void UpdateOrientation()
        {
            float tempAngle = GetAngleWithSign(mRigidBody.velocity.normalized);

            mOrientation = Mathf.RoundToInt(tempAngle / 45f);

            mOrientation = mOrientation == 8 ? 0 : mOrientation;

            mSpriteRenderer.flipX = mOrientation > 4 ? true : false;
            if (mOrientation > 4)
            {
                mOrientation = 8 - mOrientation;
            }
            mAnimator.SetInteger("Orientation", mOrientation);
        }

        void CheckDrop()
        {
            if (Vector3.Distance(transform.position, mSelectedDropPoint+ new Vector3(0, mHeightOffset, 0)) < 0.7f)
            {
               WeaponPickUp weapon = WeaponPickUp.Instantiate(DropObject);
               weapon.transform.position = transform.position;
               weapon.SetDropPoint(shadow.transform.position);
                
                isCarrying = false;
                mHeading = (new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0f).normalized * mStartDistance) - transform.position;
            }
        }

        void LimitSpeed()
        {
            if (mRigidBody.velocity.magnitude > mMaxSpeed)
            {
                mRigidBody.velocity = mRigidBody.velocity.normalized * mMaxSpeed;
            }
        }

        public float GetAngleWithSign(Vector2 dir)
        {
            float sAngle = Mathf.Sign(Vector3.Cross(dir, Vector3.up).z) * Mathf.RoundToInt(Vector2.Angle(dir, Vector2.up));
            return sAngle < 0 ? 360 + sAngle : sAngle;
        }

        void Respawn()
        {
            transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f).normalized * mStartDistance; 
            SelectDropPoint();
            mRigidBody.velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            isCarrying = true;
			heliSound = SoundManager.instance.PlayClip(chopper, true, 0.1f);
        }
    }
}