using UnityEngine;
using System.Collections;
using ArmadHeroes;


namespace DilloDash
{
    public class DilloDashAerialPlayer : PlayerActor
    {       
        private bool isPaused = true;

        private Timer waitTimer = null;   //Timer that is used to give a delay before the player comes onto screen

        private Rigidbody2D myRigidbody = null;
        [SerializeField] private SpriteRenderer shadow;

        private int orientation = 0;

        private float rotateHold = 0.25f;
        private float rotateTime = 0.25f; 

        private Vector3 target = Vector3.zero;   //Where the player is heading to (auto movement)
        [SerializeField] private float autoSpeed = 5.0f;   //General catch up speed modifier for player
        [SerializeField] private float autoMinSpeedBoundary = 10.0f;   //min speed bopundary of catch speed before it snaps to 0
        [SerializeField] private float autoMaxSpeedBoundary = 50.0f;   //max speed boundary of catch speed
        [SerializeField] private float avoidSpeed = 25.0f;   //The rate at which the player will try to avoid other aerial players

        [SerializeField] private float turnPower = 5.0f;   //The rate at which the player can turn
        [SerializeField] private float accelPower = 50.0f;   //The rate at which the player can accelerate
        private Timer accelTimer = null;   //Timer that is used influence the turn power based on the current velocity
        [SerializeField] private float airResistance = 2.0f;   //General drag value

        //[SerializeField] private GameObject projectilePrefab = null;
        //[SerializeField] private float fireRate = 5.0f;
        //private Timer fireTimer = null;

        //Start the wait timer before the aerial player comes onto screen
        void Awake()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer.enabled = false;
            shadow.enabled = false;

            waitTimer = new Timer();
            waitTimer.SetupTimer(null, 0.5f, true);
            waitTimer.AddFunctionCall(BeginFollow);
            waitTimer.StartTimer();

            accelTimer = new Timer();
            accelTimer.SetupTimer(null, 1, false);
        }

        //Assign controller/player info to the script
        public void Init(int _playerNum, int _controllerNum)
        {
            playerID = _playerNum;
            m_controllerID = _controllerNum;
        }

        void FixedUpdate()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                waitTimer.Update();
                accelTimer.Update();
                if (!isPaused)
                {
                    //Move to target and then apply player input
                    AIFollow();
                    HandleInput();     
                }
                CorrectSpriteRotation();
                SetAnimationDirection();
            }
        }


        public void SetTargetPosition(Vector3 _target)
        {
            target = _target;
        }

        //The logic that makes the aerial player go to the target
        void AIFollow()
        {
            Vector3 _direction = target - transform.position;
            _direction.z = 0;
            //The further away from the target the faster the catch speed at an exponential 
            float _catchSpeed = (Vector3.SqrMagnitude(_direction) / 10000) * Time.deltaTime * autoSpeed;
            //Limit the exponential catch up speed
            if (_catchSpeed < autoMinSpeedBoundary) { _catchSpeed = 0; }
            else if (_catchSpeed > autoMaxSpeedBoundary) { _catchSpeed = autoMaxSpeedBoundary; }
            //Apply catch up speed
            myRigidbody.AddForce(ExtensionMethods.toDirectionalIso(_direction *_catchSpeed));
        }

        void HandleInput()
        {
            ACCELERATE(controller.accelerateButton);
            ROTATE(controller.moveX);
            FIRE(controller.shootButton);
        }

        void ACCELERATE(ControlInput _input)
        {
            if (_input.IsDown())
            {
                //Apply acceleration
                myRigidbody.AddForce(ExtensionMethods.toDirectionalIso((transform.up) * _input.GetValue() * Time.deltaTime * accelPower));
            }
            else
            {
                //The faster the player the greater the air resistance
                float airResistanceModifier = Vector3.SqrMagnitude(myRigidbody.velocity);
                if (airResistanceModifier < 1) { airResistanceModifier = 1; }
                else if (airResistanceModifier > 2) { airResistanceModifier = 2; }
                myRigidbody.drag = airResistance * airResistanceModifier;
            }

            //When accelerating timer outputs 0 when not accelerating then outputs the timer variable
            if (_input.JustPressed())
            {
                accelTimer.InitialiseTimer();
            }
            else if(_input.JustReleased())
            {
                accelTimer.StartTimer();
            }

        }

        void ROTATE(ControlInput _input)
        {
            //keep rotating if held for a length of time
            if (_input.IsDown())
            {
                rotateTime -= rotateTime < 0.0f ? 0.0f : Time.deltaTime;
            }
            else
            {
                rotateTime = rotateHold;
            }
            //If rotate held then use rotation parameters
            if (rotateTime <= 0.0f)
            {
                //Limit the turn power based on the accelation timer
                float _turnLimiter = accelTimer.GetTime() + 1;
                Vector3 rot = (transform.forward * ((-_input.GetValue() * turnPower * Time.deltaTime) * _turnLimiter * 3));
                transform.Rotate(rot);
            }
            //Else do quick turns
            else
            {
                if (_input.JustPressed() && _input.GetValue() > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 45);
                }
                else if (_input.JustPressed() && _input.GetValue() < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 45);
                }
            }

            
         
        }

        void FIRE(ControlInput _input)
        {
            if (_input.JustPressed())
            {
                m_weapon.Shoot(transform.position, Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.up, playerNumber ,this, Color.white);
            }
        }

        //Is called when the aerial player activates
        void BeginFollow()
        {
            isPaused = false;

            //Get random value within square range
            float a = 0;
            float b = 0;
            float c = 0;
            bool loop = true;
            while (loop)
            {
                a = Random.Range(-1.0f, 1.0f);
                b = Random.Range(-1.0f, 1.0f);
                c = Mathf.Abs(a) + Mathf.Abs(b);
                if (c > 1)
                {
                    loop = false;
                }
            }

            //Use viewport to world to make thge point outside the camera
            Vector3 _pos = GameControllerDD.Singleton().GetCamera().ViewportToWorldPoint(new Vector3(a + 0.5f, b + 0.5f, 0));
            _pos.z = 0;
            transform.position = _pos;
            spriteRenderer.enabled = true;
            shadow.enabled = true;
        }

        #region Set Sprites
        //While full animation isn't possible this will change the texture to match the rotation
        void SetAnimationDirection()
        {
            orientation = (int)Mathf.Round(transform.eulerAngles.z / 45);
            Vector3 _scale = spriteRenderer.gameObject.transform.localScale;
            if (orientation > 3)
            {
                orientation = 8 - orientation;
                if(orientation % 4 != 0)
                {
                    _scale.x = 1.0f;
                }
            }
            else
            {
                if (orientation % 4 != 0)
                {
                    _scale.x = -1.0f;
                }
            }
            spriteRenderer.gameObject.transform.localScale = _scale;
            m_armaAnimator.SetInteger("Orientation", orientation);
        }

        //This rotates the child object sprite so that it doesn't rotate with the player
        void CorrectSpriteRotation()
        {
            Vector3 playerRotation = transform.rotation.eulerAngles;
            spriteRenderer.gameObject.transform.localEulerAngles = new Vector3(playerRotation.x * -1.0f, playerRotation.y * -1.0f, playerRotation.z * -1.0f);
        }
        #endregion

        void OnTriggerStay2D(Collider2D _other)
        {
            //When in range of any other player move away
            if (!isPaused)
            {
                if (_other.tag == "DilloDash/AerialPlayer")
                {
                    //The closer to the player the greater the avoid speed by exponential amount
                    Vector3 _direction = transform.position - _other.gameObject.transform.position;
                    Vector3 _avoid = Vector3.ClampMagnitude(_direction * 1000, 1);
                    _avoid /= Vector3.SqrMagnitude(_direction);
                    _avoid *= Time.deltaTime * avoidSpeed;
                    _avoid.z = 0;
                    myRigidbody.AddForce(ExtensionMethods.toDirectionalIso(_avoid));
                }
            }
        }
    }
}
