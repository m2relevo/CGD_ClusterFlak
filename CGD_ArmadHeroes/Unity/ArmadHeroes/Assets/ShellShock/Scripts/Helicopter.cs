using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShellShock
{
    public class Helicopter : MonoBehaviour
    {
        public static Helicopter Instance;

        public GameObject DropObject;

        Animator mAnimator;
        Rigidbody2D mRigidBody;
        SpriteRenderer mSpriteRenderer;

        public List<Transform> mDropPoints;
        public List<GameObject> mDeployedCrates;

        public float mStartDistance;
        public float mAccelleration;
        public float mMaxSpeed;
        public float mHeightOffset;
        public int mOrientation;

        Transform mSelectedDropPoint;

        Vector2 mHeading;
        bool started = false;
        public bool isCarrying;
        public bool isDebug;
        public AudioSource heliSound;
        public GameObject background;
        float minDistanceForSound = 20f;
        float maxDistanceForSound = 1f;

        void Awake()
        {
            if (Helicopter.Instance != null)
            {
                Debug.LogError("There should only be one helicopter!");
            }
            Instance = this;
            mDropPoints = new List<Transform>();
            mRigidBody = GetComponent<Rigidbody2D>();
            mSpriteRenderer = GetComponent<SpriteRenderer>();
            mAnimator = GetComponent<Animator>();
        }

        void Start()
        {
            heliSound = GetComponent<AudioSource>();
        }

        public void JoinDropPoints(Transform point)
        {
            mDropPoints.Add(point);
        }

        public void ClearDropPoints()
        {
            for (int i = 0; i < mDeployedCrates.Count; i++)
            {
                Destroy(mDeployedCrates[i].gameObject);
            }

            mDeployedCrates.Clear();
            mDropPoints.Clear();
        }

        void AdjustHeliVolume()
        {
            //20 is the fall off
            //1 is the closest pretty much.
            float distance = Vector3.Distance(background.transform.position, transform.position);
            heliSound.volume = 0;
            if (distance <= minDistanceForSound)
            {
                heliSound.volume = maxDistanceForSound / distance;
            }
        }

        void Update()
        {
            AdjustHeliVolume();
            if (started)
            {
                Move();
                if (mSelectedDropPoint == null && mDropPoints != null)
                {
                    SelectDropPoint();
                }
                else
                {
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
                }
            }
            else
            {
                started = true;
                Respawn();
            }
        }

        void SelectDropPoint()
        {
            if (mDropPoints != null)
            {
                mSelectedDropPoint = mDropPoints[Random.Range(0, mDropPoints.Count - 1)];
                mHeading = (mSelectedDropPoint.position + new Vector3(0, mHeightOffset, 0)) - transform.position;
            }
        }

        void Move()
        {
            mRigidBody.AddForce(mHeading.normalized * mAccelleration * Time.deltaTime, ForceMode2D.Force);
            LimitSpeed();

            if (isDebug)
            {
                Debug.DrawLine(transform.position, mSelectedDropPoint.position + new Vector3(0, mHeightOffset, 0), Color.yellow);
                Debug.DrawLine(transform.position, mHeading);
                Debug.DrawLine(transform.position, transform.position + (Vector3)mRigidBody.velocity, Color.red);
            }
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
            if (Vector3.Distance(transform.position, mSelectedDropPoint.transform.position + new Vector3(0, mHeightOffset, 0)) < 0.7f)
            {
                mDeployedCrates.Add(Instantiate(DropObject, transform.position, Quaternion.identity) as GameObject);

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
            transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f).normalized * mStartDistance; //randomize the spawning position of the helicopter
            SelectDropPoint();
            mRigidBody.velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            isCarrying = true;
        }
    }
}
