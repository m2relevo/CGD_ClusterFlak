using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace DilloDash
{
    public class DilloDashPlayer : PlayerActor
    {        
        private bool isPaused = true;
        private bool isDead = false;

        private Rigidbody2D myRigidbody;
        private TrailComponent myTrail = null;
        [SerializeField] private ArmaCanvas HUD = null;
        [SerializeField] private SpriteRenderer shadow = null;

        private Vector3 landingTarget;
        [SerializeField] private SpriteRenderer parachute = null;

        #region Aerial Player Variables
        [SerializeField] private GameObject AerialPlayerPrefab = null;
        private DilloDashAerialPlayer aerialPlayer = null;
        #endregion

        #region Movement Variables

        #region Braking
        public float friction = 0.5f;               // Set drag while accelerating
        public float frictionModifier = 2;          // Multiplie acceleration drag for when not accelerating
        public float brakingFrictionModifier = 2;   // Friction modifier fore under braaking
        #endregion

        #region Turning
        public float turnPower = 2.5f;      // Turning speed
        public float turnPowerVehicle = 0f; // Turning speed
        [SerializeField] private float turnPowerPowerup = 0f; // Turning speed
        private float turnPowerPriv;                // Uses publicly set variables to determine final value
        public float brakingTurnModifier = 1.5f;    // Turning power multiplier for under braking
        #endregion

        #region Forward Movment
        public float maxPower = 50;                 // Accleration power
        public float maxPowerVehicle = 0f; //Accleration power
        [SerializeField] private float maxPowerPowerup = 0f; //Accleration power
        private float privPower;

        public float maxSpeed = 20;                 // Max speed (usually wont reach this)
        public float maxSpeedVehicle = 0f; //Max speed (usually wont reach this)
        [SerializeField] private float maxSpeedPowerup = 0f; //Max speed (usually wont reach this)

        public float boostModifier = 300.0f;        // Boost force
        [SerializeField] private float boostModifierPowerup = 0.0f;
        public float boostCoolDown = 5.0f;          // publicly set variable for length of boost cooldown
        private float boostCountdown = 0.0f;        // Private cooldown timer that increments
        private float boostPrivCoolDown = 0.0f;

        private float privSpeedMod = 1.0f;          // Private variable that is applied
        public AnimationCurve powerCurve = new AnimationCurve();
        public Vector2 currSpeed;
        #endregion
        

        #region Stun Variables
        
        private float stunLength = 1.0f;
        //[SerializeField] private float impactForce = 10.0f;
        [SerializeField] private int stunSpins = 1;
        
        private float stunnedTime;
        private float storeRBDrag;
        private bool stunned;
        public bool testStun = false;       

        #endregion

        #endregion

        #region Tile Modifiers
        public float speedTileModifier = 2.0f;      // Power multiplier for on speed tiles
        public float slowTileModifier = 0.5f;       // Power multiplier for on sand tiles
        public float slipFriction = 0.0f;
        public float slipSpeedMod = 0.05f;
        private float privSlipFriction;
        #endregion
       
        #region TrackNode Variables
        private TrackNode prevNode;
        private int lap = 1;
        private float percent = 0;
        #endregion

        #region Sprite Variables
        [SerializeField] private float rollSpeed = 35.0f;
        [SerializeField] private float animSpeed = 0.1f;
        //public Sprite[] directions = null;
        public enum Directions { N = 0, NE, E, SE, S, SW, W, NW }
        public enum State { Run, Roll, Walk, Idle }
        public int storedAngle;
        int orientation;
        public int checkAngle;
        //float euler = 22.5f;

        AnimationClip myAnimation;
        public Directions myDirection;
        public State myState;
        State prevState;

        private float overrideBallTime = 0.0f;
        [SerializeField] private float overrideBallCooldown = 0.2f;
        #endregion

        #region PowerUp Variables
        [SerializeField]
        //private float powerupCoolDown = 5.0f; //powerup cooldown
        //private float powerupCountdown = 0.0f;
       
        private float laserCountdown = 0.0f;
        [SerializeField] private float laserCoolDown = 3.0f;
        [SerializeField] private float laserTime = 5.0f;

        private bool laserCharged = false;
        private bool laserCharging = false;
        private bool laseractivate = false;

        private bool boostPowerUp;
        public float boostPowerCoolDown = 0.5f;
        public float boostPowerTime = 5.0f;
        private float bPowerCounter;

        public Powerup myPowerUp;
        public Transform rear;
        public Transform front;        
        public GameObject myChargeBall = null;
        GameObject laserBall;
        public GameObject myLazerBeam = null;
        
        [SerializeField] private bool enableDDLegacyLaser = false;

        #endregion

        #region Accolade Trackers

      
        private Vector3 lastPos;


        #endregion

        #region MonoBehaviour Functions
        // Use this for initialization
        void Awake()
        {
           
            myRigidbody = GetComponent<Rigidbody2D>();
            myTrail = GetComponent<TrailComponent>();
            //powerCurve.
            //LaserSetup();
            //sets inventory size
            resetPowerup();
            storeRBDrag = myRigidbody.angularDrag;
            lastPos = transform.position;
        }

        //Physics functions called in here
        void FixedUpdate()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {     
                if (!isPaused)
                {
                    HandleInput();
                    
                    if (prevNode)
                    {
                        percent = prevNode.PercentAroundTrack(GetFront());
                    }
                }
                CorrectSpriteRotation();
            }
            SetAnimationDirection();            
        }
        #endregion
        void LateUpdate()
        {
            //Fix the stun glitch in late update
            if (stunned)
            {
                CorrectSpriteRotation();
            }
        }

        //Assign controller/player info to the script
        public void Init(int _playerNum, int _controllerNum, string _name)
        {
            playerID = _playerNum;
            m_controllerID = _controllerNum;
            //controller = ControllerManager.instance.GetController(controllerIndex);
            ActorName = _name;
            m_override.SetCharacter(ActorName);
        }

        public void SendData()
        {
            GlobalPlayerManager.instance.SetDebriefStats(playerID, chevron_score, accolade_timesShot, accolade_distance, accolade_distance, accolade_shotsFired, accolade_unique);
        }

        //Sets if paused
        public void SetPaused(bool _b) { isPaused = _b; }

        //Return if the player is dead
        public bool GetIsDead() { return isDead; }

        public void SetLandingTarget(Vector3 _landingTarget)
        {
            transform.position = _landingTarget + (Vector3.up * 75.0f);
            landingTarget = _landingTarget;
            parachute.enabled = true;
            shadow.gameObject.transform.position = _landingTarget + (Vector3.up * -0.35f * transform.localScale.y);
        }

        public void UpdateLandingTarget(float _time, float _duration)
        {
            transform.position = Vector3.Lerp(landingTarget + (Vector3.up * 75.0f),landingTarget, (_duration - _time) / _duration);
            if (_time == 0.0f)
            {
                bodyCollider.enabled = true;
                parachute.enabled = false;
            }
            shadow.gameObject.transform.position = landingTarget + (Vector3.up * -0.35f * transform.localScale.y);
        }


        //public int GetPlayerNumber() { return playerNumber; }

        //When the player dies
        public void Kill()
        {
            isDead = true;
            isPaused = true;
            spriteRenderer.enabled = false;
            shadow.enabled = false;
            bodyCollider.enabled = false;
            HUD.Deactivate();
            InstantiateAerialPlayer();
        }

        public int GetScore()
        {
            return chevron_score;
        }

        public void Win()
        {
            ++chevron_score;
            bodyCollider.enabled = false;
            ArmadHeroes.CanvasManager.instance.setPlayerValue(playerID, chevron_score);
        }

        #region Aerial Player Functions
        void InstantiateAerialPlayer()
        { 
            if (!aerialPlayer)
            {
                GameObject _aerialPlayerObject = (GameObject)Instantiate(AerialPlayerPrefab,transform.position, Quaternion.identity);
                aerialPlayer = _aerialPlayerObject.GetComponent<DilloDashAerialPlayer>();
                aerialPlayer.Init(playerID, m_controllerID);
            }
        }

        void RemoveAerialPlayer()
        {
            if (aerialPlayer)
            {
                Destroy(aerialPlayer.gameObject);
                aerialPlayer = null;
            }
        }

        public void UpdateAerialPlayer(Vector3 _target)
        {
            aerialPlayer.SetTargetPosition(_target);
        }

        public DilloDashAerialPlayer GetAerialPlayer() { return aerialPlayer; }
        
        #endregion       

        #region Set Tile Modifiers
        //Sets the private speed mod for when on fast tiles
        public void setSpeedTileMod() 
        {
            privSpeedMod = speedTileModifier;
            
        }

        //Sets the private speed mod for when on slow tiles
        public void setSlowTileMod()
        {
            privSpeedMod = slowTileModifier;
            
        }        

        //Sets the mods for when on slip tiles
        public void setSlipTileOn()
        {
            if (bodyCollider.enabled == true)
            {
                privSlipFriction = slipFriction;
                privSpeedMod = slipSpeedMod;
                if (!stunned)
                {
                    Vector3 temp = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                    InitiateStun(transform.position + temp, false);
                }
            }
            
        }

        //Set currTile to Track
        public void setTrackTile()
        {
            //Here if needed
        }
        #endregion

        #region Movment Functions
        //Calls each aspect of movement
        void HandleInput()
        {
            if (testStun == true)
            {
                InitiateStun(transform.position);
                testStun = false;
            }
            if(!stunned)
            {
                ACCELERATE(controller.accelerateButton);
                ROTATE(controller.moveX);
                BRAKE(controller.decelerateButton);
                BOOST(controller.boostButton);
                POWERUP(controller.shootButton);
            }
            else
            {
                stunnedTime += Time.fixedDeltaTime;
                if (stunnedTime >= stunLength)
                {
                    stunned = false;
                    myRigidbody.freezeRotation = true;
                    myRigidbody.angularVelocity = 0;
                    myRigidbody.angularDrag = storeRBDrag;
                }
                HUD.UpdateSliderTwo((stunnedTime / stunLength) * 100);
            }
            if (boostPowerUp)
            {
                bPowerCounter += (bPowerCounter < boostPowerTime) ? Time.fixedDeltaTime : 0.0f;
                if (bPowerCounter >= boostPowerTime)
                {
                    boostPowerUp = false;
                }
            }

            accolade_distance += (transform.position.toCart() - lastPos.toCart()).magnitude;

            lastPos = transform.position;
            //InitiatStun(Vector3.zero);
            ResetVariables();
        }       

        //Deals with moving the player forward
        void ACCELERATE(ArmadHeroes.ControlInput _input)
        {
            if (_input.IsDown())
            {
                privPower = EvaluatePowerCurve();
                myRigidbody.AddForce(ExtensionMethods.toDirectionalIso((transform.up) * _input.GetValue() * privPower * privSpeedMod));

                myRigidbody.drag = friction * privSlipFriction;

            }
            else
            {
                myRigidbody.drag = friction * frictionModifier * privSlipFriction;
            }


            currSpeed = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y);
            currSpeed = currSpeed.toCart();
            if (currSpeed.magnitude > maxSpeed + maxSpeedVehicle + maxSpeedPowerup)
            {
                currSpeed = currSpeed.normalized;
                currSpeed *= maxSpeed + maxSpeedVehicle + maxSpeedPowerup;
            }
        }

        //See if the player is braking
        void BRAKE(ArmadHeroes.ControlInput _input)
        {            
            if (_input.IsDown())
            {
                myRigidbody.drag = friction * brakingFrictionModifier * privSlipFriction;
                turnPowerPriv = (turnPower + turnPowerVehicle + turnPowerPowerup) * brakingTurnModifier;
            }
            else
            {
                turnPowerPriv = turnPower + turnPowerVehicle + turnPowerPowerup;
            }
        }

        //reset some variable once theyve been used this frame
        void ResetVariables()
        {
            privSpeedMod = 1.0f;
            privSlipFriction = 1.0f;
        }

        //Deals with rotating the player
        void ROTATE(ArmadHeroes.ControlInput _input)
        {
            //STILL NEED TO GET APPROPRIATE ROTATION SPEED FOR ISO
            float leftStickX = _input.GetValue();
            Vector3 rot = (transform.forward * (-leftStickX * turnPowerPriv));

            transform.Rotate(rot);

            //Method for converting the new rotation to iso.  
            float zRot = transform.rotation.eulerAngles.z;
            zRot = zRot * Mathf.Deg2Rad;
            Vector2 testRot = new Vector2((float)Mathf.Cos(zRot), (float)Mathf.Sin(zRot));
            testRot.toIso();
            zRot = (float)Mathf.Atan2(testRot.x, -testRot.y);
            zRot = zRot * Mathf.Rad2Deg;
            rot.z = zRot;
            //This is Obsolette and actually doesn't do anything anymore
            // transform.rotation.SetEulerRotation(rot);

        }

        //Deals with boosting the  player
        void BOOST(ControlInput _input)
        {

            boostPrivCoolDown = boostPowerUp ? boostPowerCoolDown : boostCoolDown;
            if (boostCountdown >= boostPrivCoolDown)
            {
                if (_input.JustPressed())
                {
                    myRigidbody.AddForce(transform.up * (boostModifier + boostModifierPowerup) * 10);
                    boostCountdown = 0.0f;
                }
            }
            else
            {
                boostCountdown += Time.fixedDeltaTime;
                HUD.UpdateSliderOne((boostCountdown / boostPrivCoolDown) * 100);
            }
        }

        public void InitiateStun(Vector3 _location, bool _shot = false, float _stunLength = 1, float _force = 500)
        {

            if(!stunned)
            {
                stunLength = _stunLength;
                Vector2 dpos = transform.position - _location;
                dpos = dpos.normalized * _force;
                stunned = true;
                stunnedTime = 0.0f;
                myRigidbody.angularDrag = 0.0f;
                myRigidbody.drag = friction * 0.1f;
                myRigidbody.freezeRotation = false;
                myRigidbody.angularVelocity = (360* stunSpins) / stunLength;
                myRigidbody.AddForce(dpos);
                accolade_unique++;                          

            }
            if (_shot)
                    accolade_timesShot++;
        }

        //Return a value on the power curve based on speed
        float EvaluatePowerCurve()
        {
 
            return ((maxPower + maxPowerVehicle + maxPowerPowerup) * powerCurve.Evaluate((currSpeed.magnitude / (maxSpeed + maxSpeedVehicle + maxSpeedPowerup))));

        }
        #region Set Sprites
        void SpeedTrail()
        {
            //myShadow = 
            // Destroy(Instantiate(mySpriteRenderer))
        }
        void UpdateAngle()
        {
            checkAngle = (int)transform.eulerAngles.z;
        }
        void SelectStatesMove()
        {
            if (myRigidbody.velocity.magnitude <= 0.1f)
            {
                m_armaAnimator.speed = 1.0f;
                SetState(State.Idle);
            }
            else if ((myRigidbody.velocity.magnitude > 0.1f) && (myRigidbody.velocity.magnitude < rollSpeed))
            {
                m_armaAnimator.speed = animSpeed * myRigidbody.velocity.magnitude;
                SetState(State.Run);
            }

            else if ((myRigidbody.velocity.magnitude >= rollSpeed))
            {
                m_armaAnimator.speed = animSpeed * myRigidbody.velocity.magnitude;
                //If just used powerup/weapon force out of ball
                if (overrideBallTime > 0.0f)
                {
                    SetState(State.Run);
                }
                else
                {
                    SetState(State.Roll);
                }
            }
        }
        void UseState(State _myState)
        {
            switch (_myState)
            {
                case State.Idle:
                    UseDirectionIdle();
                    break;
                case State.Run:
                    UseDirectionRun();
                    break;
                case State.Roll:
                    UseDirectionBall();
                    break;
            }
        }
        void SetDirection(Directions _Direction)
        {
            myDirection = _Direction;
        }
        void SetState(State _State)
        {
            myState = _State;
        }
        void UseDirectionIdle()
        {
            switch (myDirection)
            {
                case Directions.N:
                    m_armaAnimator.SetTrigger("Nidle");
                    break;
                case Directions.NE:
                    m_armaAnimator.SetTrigger("NEidle");
                    break;
                case Directions.E:
                    m_armaAnimator.SetTrigger("Eidle");
                    break;
                case Directions.SE:
                    m_armaAnimator.SetTrigger("SEidle");
                    break;
                case Directions.S:
                    m_armaAnimator.SetTrigger("Sidle");
                    break;
                case Directions.SW:
                    m_armaAnimator.SetTrigger("SEidle");
                    break;
                case Directions.W:
                    m_armaAnimator.SetTrigger("Eidle");
                    break;
                case Directions.NW:
                    m_armaAnimator.SetTrigger("NEidle");
                    break;
            }
        }
        void UseDirectionRun()
        {
            switch (myDirection)
            {
                case Directions.N:
                    m_armaAnimator.SetTrigger("Nrun");
                    break;
                case Directions.NE:
                    m_armaAnimator.SetTrigger("NErun");
                    break;
                case Directions.E:
                    m_armaAnimator.SetTrigger("Erun");
                    break;
                case Directions.SE:
                    m_armaAnimator.SetTrigger("SErun");
                    break;
                case Directions.S:
                    m_armaAnimator.SetTrigger("Srun");
                    break;
                case Directions.SW:
                    m_armaAnimator.SetTrigger("SErun");
                    break;
                case Directions.W:
                    m_armaAnimator.SetTrigger("Erun");
                    break;
                case Directions.NW:
                    m_armaAnimator.SetTrigger("NErun");
                    break;
            }
        }
        void UseDirectionBall()
        {
            switch (myDirection)
            {
                case Directions.N:
                    m_armaAnimator.SetTrigger("Nspec");
                    break;
                case Directions.NE:
                    m_armaAnimator.SetTrigger("NEspec");
                    break;
                case Directions.E:
                    m_armaAnimator.SetTrigger("Espec");
                    break;
                case Directions.SE:
                    m_armaAnimator.SetTrigger("SEspec");
                    break;
                case Directions.S:
                    m_armaAnimator.SetTrigger("Sspec");
                    break;
                case Directions.SW:
                    m_armaAnimator.SetTrigger("SEspec");
                    break;
                case Directions.W:
                    m_armaAnimator.SetTrigger("Espec");
                    break;
                case Directions.NW:
                    m_armaAnimator.SetTrigger("NEspec");
                    break;
            }
        }
        void SetAnimationDirection()
        {
            orientation = (int)Mathf.Round(transform.eulerAngles.z / 45);
            myDirection = (Directions)orientation;
            myDirection -= orientation == 8 ? 8 : 0;
            if (orientation > 3)
            {
                orientation = 8 - orientation;
                if (orientation % 4 != 0)
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                if (orientation % 4 != 0)
                {
                    spriteRenderer.flipX = true;
                }
            }
            SelectStatesMove();
            UseState(myState);
            CallFootsteps(myState);
            
        }
                //This rotates the child object sprite so that it doesn't rotate with the player
         void CorrectSpriteRotation()
        {
            Vector3 playerRotation = transform.rotation.eulerAngles;
            spriteRenderer.gameObject.transform.localEulerAngles = new Vector3(playerRotation.x * -1.0f, playerRotation.y * -1.0f, playerRotation.z * -1.0f);
        }
        #endregion
        void CallFootsteps(State _state)
        {
            switch (_state)
            {
                case State.Idle:
                    walking = false;
                    break;
                case State.Walk:
                    walking = true;
                    break;
                case State.Run:
                    walking = true;
                    break;
                case State.Roll:
                    walking = false;
                    break;
            }

            currentAngle = transform.eulerAngles.z;
            Steps();             

        }

        #endregion

        #region Powerup Functions
        // called when a powerup is used
        public void resetPowerup()
        {
            myPowerUp = Powerup.nothing;
            
            GetComponentInChildren<ArmaCanvas>().DeactivateWeaponPickup();
        }

        void nullifyPowerup()
        {
            turnPowerPowerup = 0.0f;
            maxPowerPowerup = 0.0f;
            maxSpeedPowerup = 0.0f;
            boostModifierPowerup = 0.0f;
            //powerupCountdown = 0.0f;
            //powerupCoolDown = 0.0f;
        }
        // called when a powerup is picked up
        public void setPowerup(Powerup power)
        {
            myPowerUp = power;
        }       

        void StartLaser()
        {
            Destroy(laserBall);
            GameObject laserBeam = (GameObject)Instantiate(myLazerBeam, front.transform.position, Quaternion.identity);
            laserBeam.transform.parent = transform;
            laserBeam.transform.rotation = transform.rotation;
            Destroy(laserBeam, laserTime);
            laserCharged = false;
            laserCharging = false;
            laserCountdown = 0f;
            laseractivate = false;

            resetPowerup();

        }

        void applyPowerUp(float _turnPowerPowerup, float _maxPowerPowerup, float _maxSpeedPowerup, float _boostModifierPowerup, float _powerupCoolDown)
        {
            turnPowerPowerup = _turnPowerPowerup;
            maxPowerPowerup = _maxPowerPowerup;
            boostModifierPowerup = _boostModifierPowerup;
            maxSpeedPowerup = _maxSpeedPowerup;
            //powerupCoolDown = _powerupCoolDown;
        } 

        //to use the powerup

        void POWERUP(ArmadHeroes.ControlInput _input)
        {
            overrideBallTime -= overrideBallTime < 0.0f ? 0.0f : Time.deltaTime;
            switch (myPowerUp)
            {
                case Powerup.BoostPowerup:
                    if (_input.JustPressed())
                    {
                        bPowerCounter = 0.0f;
                        boostPowerUp = true;
                        resetPowerup();
                    }         
                    break;

                case Powerup.RocketPowerup:                    
                    if (_input.JustPressed())
                    {
                        m_RPG.Shoot(transform.position, Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.up, playerID, this, Color.white);
                        overrideBallTime = overrideBallCooldown;
                        accolade_shotsFired++;
                        resetPowerup();
                    }
                    break;

                case Powerup.MinePowerup:                    
                    if (_input.JustPressed())
                    {
                        m_weapon.Shoot(transform.position, Vector3.zero, playerID, this, Color.white);
                        overrideBallTime = overrideBallCooldown;
                        accolade_shotsFired++;
                        resetPowerup();
                    }
                    break;

                case Powerup.LaserPowerup:
                    if (!enableDDLegacyLaser)
                    {
                        if (_input.JustPressed())
                        {
                            m_laser.Shoot(transform.position, Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.up, playerID, this, Color.white);
                            overrideBallTime = overrideBallCooldown;
                            resetPowerup();
                        }
                        break;
                    }
                    else
                    {
                        if (_input.JustPressed() && !laseractivate)
                        {
                            laseractivate = true;
                            accolade_shotsFired++;
                        }
                        if (laseractivate)
                        {
                            overrideBallTime = overrideBallCooldown;
                            if (!laserCharged)
                            {
                                if (!laserBall)
                                {
                                    laserBall = (GameObject)Instantiate(myChargeBall, front.transform.position, Quaternion.identity);
                                    laserBall.transform.parent = transform;

                                }
                                laserCharging = true;
                            }
                            else
                            {
                                StartLaser();
                            }
                            if (laserCharging)
                            {
                                if (laserCountdown >= laserCoolDown)
                                {
                                    laserCharged = true;
                                }
                                else
                                {
                                    //add time to countdown
                                    laserCountdown += Time.fixedDeltaTime;
                                }
                            }
                        }
                        else
                        {
                            laserCountdown = 0;
                            laserCharging = false;
                            Destroy(laserBall);
                        }
                        break;
                    }
            }    
        }
        #endregion

        #region Track Node Functions

        //Get the track percent that represents the player
        public float GetTrackProgress()
        {
            if (isDead)
            {
                return 0;
            }
            return (lap * 100.0f) + percent;
        }

        //Resets the player when the current game run finishes
        public void Reset(TrackNode _node)
        {
            if (myTrail != null)
                myTrail.unparentLastTrails();

            myRigidbody.velocity = Vector2.zero;
            myRigidbody.angularVelocity = 0.0f;
            prevNode = _node;
            lap = 1;
            percent = GameStateDD.Singleton().maxPlayers - playerNumber;
            isDead = false;
            isPaused = true;
            spriteRenderer.enabled = true;
            shadow.enabled = true;
            HUD.UpdateSliderOne(100);
            HUD.UpdateSliderTwo(100);
            stunned = false;
            stunnedTime = stunLength;
            boostCountdown = 0; //OR = boostcooldown to refill
            RemoveAerialPlayer();
            resetPowerup();
        }

        public TrackNode GetPrevNode() { return prevNode; }

        public void SetPrevNode(TrackNode _node) { prevNode = _node; }

        public void UpdateTrackPercent()
        {
            prevNode.PercentAroundTrack(transform.position);
        }

        //The front most point of the player
        Vector3 GetFront()
        {
            return transform.position + (Quaternion.Euler(0, 0, transform.eulerAngles.z) * (Vector3.up * (transform.localScale.y / 2)));
        }
        #endregion

        #region Collision Functions
        void OnTriggerEnter2D(Collider2D _other)
        {
            if (!isPaused)
            {
                if (_other.tag == "DilloDash/TrackNode")
                {
                    //Increases the lap only if the player is behind the start line
                    prevNode = _other.gameObject.GetComponent<TrackNode>();
                    if (_other.gameObject.GetComponent<TrackNode>() == GameControllerDD.Singleton().GetStartLine())
                    {
                        //Determine if before the node
                        Vector3 nodeForward = Quaternion.Euler(0, 0, prevNode.transform.eulerAngles.z) * (Vector3.down * (transform.localScale.y / 2));
                        Vector3 fromNode = transform.position - prevNode.transform.position;
                        float dot = Vector3.Dot(nodeForward, fromNode);
                        if (dot > 0.0f)
                        {
                            ++lap;
                        }
                    }
                }
                //adds powerup to inventory if tag is powerup then destroys it
                if (_other.tag == "Powerup")
                {
                    Destroy(_other.gameObject);
                }
                if (_other.tag == "Projectile")
                {

                }
            }
        }

        void OnTriggerExit2D(Collider2D _other)
        {
            if (_other.tag == "DilloDash/TrackNode")
            {
                //Decrease the lap only if the player is behind the start line   
                Vector3 nodeForward = Quaternion.Euler(0, 0, prevNode.transform.eulerAngles.z) * (Vector3.up * (transform.localScale.y / 2));
                Vector3 fromNode = transform.position - prevNode.transform.position;
                float dot = Vector3.Dot(nodeForward, fromNode);
                if (dot < 0.0f)
                {
                    prevNode = prevNode.GetClosestPreviousNode(GetFront());
                    if (_other.gameObject.GetComponent<TrackNode>() == GameControllerDD.Singleton().GetStartLine())
                    {
                        --lap;
                        if (lap < 0)
                        {
                            lap = 0;
                        }
                    }
                }
            }
        }
        #endregion      
    }
}
