using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public abstract class BaseActor : MonoBehaviour
    {
        protected int mHealth;
        protected int mMaxHealth;

        public float mMovementSpeed;

        public int mOrientation;

        public Animator mAnimator;

        protected Rigidbody2D mRigidBody;

        private int mID = 1;

        private Vector2 mMoveVector, mAimVector;

        private float[] mOrientationAngles = { 0, 60, 90, 120, 180, 240, 270, 300 };

        //bool showHUD = false;
     //   bool isAlive = true;

        protected void Start()
        {
            mRigidBody = GetComponent<Rigidbody2D>();
        }

        protected void Update()
        {
            ProcessInput();
            Move();
            Aim();
        }

        public ArmadHeroes.Controller mController
        {
            private set { }
            get { return ArmadHeroes.ControllerManager.instance.GetController(mID); }
        }

        void ProcessInput()
        {
            mMoveVector = new Vector2(mController.moveX.GetValue(), mController.moveY.GetValue());
            mAimVector = new Vector2(mController.aimX.GetValue(), mController.aimY.GetValue());
            //showHUD = mController.hudButton.IsDown();
        }

        void Move()
        {
            if (mMoveVector.x != 0.0f || mMoveVector.y != 0.0f)
            {
                Debug.Log("Move");
                mRigidBody.AddForce(mMovementSpeed * mMoveVector);
                mAnimator.SetBool("walking", true);
            }
            else
            {
                mAnimator.SetBool("walking", false);
            }
        }

        void Aim()
        {
            if (mAimVector.magnitude > 0.4)
            {
                UpdateOrientation(mAimVector);
            }
            else
            {
                UpdateOrientation(mMoveVector);
            }
        }

        void UpdateOrientation(Vector2 dir)
        {
            mOrientation = GetOrientationFromaAngle(SignedAngleFromVector(dir));

            mAnimator.SetInteger("Orientation", mOrientation);

            if (mOrientation >= 4 && mOrientation <= 7)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        protected void TakeDamage(int damage, int id)
        {
            mHealth -= damage;
            if (mHealth <= 0)
            {
                Kill(id);
            }
            else
            {
                //TODO: Update health bar
                //TODO: Play Hit sound
                //TODO: Visual of being hit
            }
        }

        protected void Kill(int killer)
        {
            ShellShock.RespawnManager.Instance.Kill(gameObject, 3);

        //    isAlive = false;
            //TODO: Update killers score
        }

        protected void Respawn(Vector2 pos)
        {
            //TODO: Respawn animation / Visual cue
            gameObject.transform.position = pos;
          //  isAlive = true;
        }

        #region Helper Functions

        public float SignedAngleFromVector(Vector2 dir)
        {
            float sAngle = Mathf.Sign(Vector3.Cross(dir, Vector3.up).z) * Mathf.RoundToInt(Vector2.Angle(dir, Vector2.up));
            return sAngle < 0 ? 360 + sAngle : sAngle;
        }

        int GetOrientationFromaAngle(float angle)
        {
            for (int i = 0; i < 8; i++)
            {
                if (angle > mOrientationAngles[i] && angle > mOrientationAngles[i + 1 < 8 ? 0 : i + 1])
                {
                    return i;
                }
            }
            Debug.LogError("Angle not in range");
            return 0;
        }

        #endregion
    }
}
