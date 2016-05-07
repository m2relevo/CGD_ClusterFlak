using UnityEngine;
using System.Collections;
using Rewired;

namespace ShellShock
{
    public class Aiming : MonoBehaviour
    {
        //Side Note selling of Reticle is ok instead of Reticule which looks weird
        //it not like anyone reads these comments anyway

        public GameObject player;
        private PlayerActions _playerActions;
        private SpriteRenderer playerRenderer;
        public SpriteRenderer reticleRenderer;
        public Movement playerMovement;

        public Vector2 mDefaultPosition = new Vector2(0.0f, 0.0f);

        float mReticuleDistFromOrigin = 2f; //Range the reticle can move from the origin(player)

        Vector2 mCorrectedAimDirection = new Vector2(0.0f, 0.0f);
        Vector2 mShootOrigin = new Vector2();

        public GameObject mReticle;
        private Rewired.Player rewiredPlayer;
        private Vector2 aimVector;

        public int playerId = 0;



        void Start()
        {
            playerRenderer = GetComponent<SpriteRenderer>();
            _playerActions = player.GetComponent<ShellShock.PlayerActions>();
            playerRenderer.enabled = true;
            reticleRenderer = transform.Find("Reticle").GetComponent<SpriteRenderer>();
            reticleRenderer.enabled = false;
            playerMovement = GetComponent<Movement>();



        }

        public Vector2 CorrectedAimDirection
        {
            get
            {
                return -mCorrectedAimDirection.normalized;
            }

            set
            {
                mCorrectedAimDirection = value.normalized;
            }
        }
        public Vector2 ShootOrigin
        {
            get
            {
                return mShootOrigin;
            }

            set
            {
                mShootOrigin = value;
            }
        }

        void Awake()
        {
            rewiredPlayer = ReInput.players.GetPlayer(playerId);
        }

        void Update()
        {
            aimVector.x = rewiredPlayer.GetAxis("Aim Horizontal");
            aimVector.y = rewiredPlayer.GetAxis("Aim Vertical");
            //   Vector2 inputAimDirection = new Vector2(Input.GetAxis("Player_" + GetComponent<ShellShock.Player>().PlayerNumber + "_RJoystickX"), Input.GetAxis("Player_" + GetComponent<ShellShock.Player>().PlayerNumber + "_RJoystickY"));
            Vector2 inputAimDirection = new Vector2(aimVector.x, aimVector.y);
            
            if (inputAimDirection.magnitude > 0.6)
            {
                reticleRenderer.enabled = true;
                float signedAngle = GetAngleWithSign(inputAimDirection);

                //Basically converts the angle between 0 and 360 degrees to an integer between 0 and 8 which corresponds to a sprite facing that angle
                int spriteNumber = Mathf.RoundToInt(((signedAngle <= -157.5f ? 180 : signedAngle) + 180) / 45);

                //Corrected aim direction takes into account the weapon length to prevent the bullets colliding with the players own collider
                mCorrectedAimDirection = new Vector2((Mathf.Sin(spriteNumber * 45 * Mathf.Deg2Rad)), (Mathf.Cos(spriteNumber * 45 * Mathf.Deg2Rad))) * mReticuleDistFromOrigin;

                mShootOrigin = mDefaultPosition + (Vector2)transform.position - (mCorrectedAimDirection);

                mReticle.transform.position = mShootOrigin;
                
           
                playerRenderer.sprite = SpriteManager.instance.ChangeSprite(spriteNumber);
            }
            else
            {
                reticleRenderer.enabled = false;
            }

         
        }

        public float GetAngleWithSign(Vector2 dir)
        {
            //Get the angle in degrees between the aim direction and the up direction
            float angle = Vector2.Angle(dir, Vector2.up);
            //Gets the sign value for the degrees as Vector2.Angle will round 270 to 90 degrees
            //Then multiplies it by the angle so 270 degrees becomes -90 degrees not 90 degrees
            return Mathf.Sign(Vector3.Cross(dir, Vector3.up).z) * angle;
            //Calculates the numerator for the 8 sections using some hacky maths  
        }

        public void ReticleRender()
        {
            if (_playerActions.isBallin == true)
            {
                playerRenderer.enabled = false;
            }

            if (_playerActions.isBallin == false)
            {
                playerRenderer.enabled = true;
            }
        }
    }
}
