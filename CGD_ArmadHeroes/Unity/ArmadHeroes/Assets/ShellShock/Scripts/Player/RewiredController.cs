using UnityEngine;
using System.Collections;
using Rewired;
using UnityEngine.UI;
using ArmadHeroes;

namespace ShellShock
{
    public class RewiredController : PlayerActor
    {
        // public int playerId = 0; // The Rewired player id of this character
        //public int controllerID = 0;
        //private Rewired.Player player; // The Rewired Player
        private Vector3 moveVector;
        private Vector2 aimVector;
        private bool fire;
        public bool hasFired;
        Rigidbody2D mRigidBody2D;
        private float mMovementSpeed = 400f;
        int mOrientation;
        int mFiringOrientation;
        public GlobalPlayerManager globalPlayerManager;
        public CharacterAnimationOverride characterAnimationOverride;
        public Animator playerAnimator;
        public Vector2[] mShootOffsets;

        #region Aiming Variables
        private SpriteRenderer playerRenderer;
        private SpriteRenderer reticleRenderer;
        private Vector2 mDefaultPosition = new Vector2(0.0f, 0.0f);
        //  float mReticuleDistFromOrigin = 2f; //Range the reticle can move from the origin(player)
        Vector2 mCorrectedAimDirection = new Vector2(0.0f, 0.0f);
        Vector2 mShootOrigin = new Vector2();
        private GameObject mReticle;
        #endregion
        #region Player/Firing Variables
       // public NewWeapon mEquippedWeapon;
        public ArmadHeroes.Weapon mSecondWeapon;
        // public Slider ammoSlider;
        int mSheildDamageReduction;
        public float waitTime;
        float mMaxSheildTime;
        float mRemainingSheidTime;
        Vector2 mReticlePosition;
        private ParticleSystem minigunParticleSystem;
        public GameObject playerHUD;
        public int playerScore;
        private bool isPlayerDead = false;
        private int killingBulletID;
        private float[] mOrientationAngles = { 0, 60, 90, 120, 180, 240, 270, 300 };
        public ArmadHeroes.Weapon[] weaponList;
        public ArmadHeroes.Weapon[] weaponListDef;
        public bool debugFirstWeaponEnabled = true;
        public bool debugSecondWeaponEnabled = false;
        int randomWeapon;
        public bool disablePlayerUpdate = false;

        Vector2 mLaserDirection;

        public LaserSight mLaserSight;

        // public int playerID = 0;


        //private ArmadHeroes.Controller controller
        //{
        //    get { return ControllerManager.instance.GetController(controllerID); }
        //    set { }
        //}

        public Vector2 ReticlePosition
        {
            get
            {
                return mReticlePosition;
            }
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
        #endregion
        #region Ball Variables
        public GameObject ballObj;
        public bool isBallin = false; //curled up in a ball
        public int weaponNumber;
        private Collider2D playerCollider;
        //private float timeSpentBallin = 0.0f;
        //private float timeSpentNotBallin = 5.0f;

        public float ballModeTick = 120;
        private float ballModeTickThreshold = 0;
        private bool ballModeStopped = false;
        private float ballStoppedTimer = 0;
        private float ballStoppedTimerThreshold = 10;
        public float ballModeBarMax = 120;
        private float autoRechargeSpeed = 0.1f;
        private float drainSpeed = 2f;


        public Image ballModeBar;

        #endregion

        void Awake()
        {
            // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
            globalPlayerManager = GlobalPlayerManager.instance;
            characterAnimationOverride = gameObject.GetComponent<CharacterAnimationOverride>();
            //   player = ReInput.players.GetPlayer(controllerID);
            mRigidBody2D = gameObject.GetComponent<Rigidbody2D>();
            playerCollider = gameObject.GetComponent<Collider2D>();
            playerAnimator = gameObject.GetComponent<Animator>();
        }
        public void Init(int playerNumber, int controllerNumber)
        {
           // Debug.Log("PLAYERNUMBER:" + playerNumber + ",controller NUMBER:" + controllerNumber);
            playerID = playerNumber;
            m_controllerID = controllerNumber;
            string _name = CharacterProfiles.instance.TypeToString(GlobalPlayerManager.instance.GetPlayerData(playerID).character);
            m_override.SetCharacter(_name);
            //characterAnimationOverride.SetCharacter(ArmadHeroes.CharacterProfiles.instance.TypeToString(globalPlayerManager.GetPlayerData(playerID).character));

        }

        void Start()
        {
            //if there isn't a controller to work with that player, then they will disappear.
            if (globalPlayerManager != null)
            {
                // Debug.Log("TEST");
                // Debug.Log(playerNumber);
                //  Debug.Log(!globalPlayerManager.GetPlayerData(playerNumber).activePlayer);
                //if (!globalPlayerManager.GetPlayerData(playerNumber).activePlayer)
                //{
                //    //Debug.Log(playerNumber);
                //    //disabling the players
                //    // Debug.Log("CALLED");
                //    gameObject.SetActive(false);

                //    return;
                //}

                //obtaining the controller ID.
               // m_controllerID = globalPlayerManager.GetPlayerData(playerNumber).controllerIndex;
              //  characterName = ArmadHeroes.CharacterProfiles.instance.TypeToString(name);
                //Changes the character's sprite based on the character they chose in the CharacterSelect screen.
                //characterAnimationOverride.SetCharacter(globalPlayerManager.GetPlayerData(m_controllerID).character.ToString());
                //characterAnimationOverride.SetCharacter( ArmadHeroes.CharacterProfiles.instance.TypeToString(globalPlayerManager.GetPlayerData(m_controllerID).character));

               // Debug.Log(globalPlayerManager.GetPlayerData(m_controllerID).character.ToString());
            }



            #region AimingStart
            playerRenderer = GetComponent<SpriteRenderer>();
            playerRenderer.enabled = true;
            mReticle = transform.Find("Reticle").gameObject;
            reticleRenderer = transform.Find("Reticle").GetComponent<SpriteRenderer>();
            reticleRenderer.enabled = false;

            minigunParticleSystem = transform.Find("MiniGun Particle System").GetComponent<ParticleSystem>();
            playerHUD = transform.Find("Canvas").transform.Find("PlayerHUD").gameObject;
            //ammoSlider = transform.Find("Canvas").transform.Find("PlayerHUD").transform.Find("PlayerAmmoCountSlider").GetComponent<Slider>();
            ballModeBar = transform.Find("Canvas").transform.Find("PlayerHUD").transform.Find("BallModeBar").GetComponent<Image>();


            //Setting the sprite of the Player Icon automatically.
            playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("_Integration/Sprites/P" + (m_controllerID + 1).ToString());

            //Setting the color of the Player Icon sprite automatically.
            switch (m_controllerID)
            {
                case 0:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 171, 255);
                break;
                case 1:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                break;
                case 2:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(235, 239, 0);
                break;
                case 3:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 55);
                break;
                case 4:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 171, 255);
                break;
                case 5:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                break;
                case 6:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(235, 239, 0);
                break;
                case 7:
                playerHUD.transform.Find("PlayerIcon").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 55);
                break;
            }


          //  mEquippedWeapon.Owner = this;
            //if (WeaponManager.Instance)
            //{
            //    mEquippedWeapon = WeaponManager.Instance.GetWeapon(0).GetComponent<NewAssaultRifle>();

            //}

            //if (!WeaponManager.Instance.GetWeapon(0).GetComponent<NewWeapon>())
            //{
            //    Debug.LogError("Weapon is null");
            //}

            #endregion
            #region PlayerStart
           // mEquippedWeapon.transform.position = transform.position;
           // mEquippedWeapon.transform.parent = transform;
            mReticlePosition = transform.position;
            weaponList = new ArmadHeroes.Weapon[transform.Find("WeaponHolster").childCount];
            for (int i = 0; i < transform.Find("WeaponHolster").childCount; i++)
            {
                weaponList[i] = transform.Find("WeaponHolster").GetChild(i).GetComponent<ArmadHeroes.Weapon>();
            }
            weaponListDef = new ArmadHeroes.Weapon[transform.Find("WeaponHolsterDef").childCount];
            for (int i = 0; i < transform.Find("WeaponHolsterDef").childCount; i++)
            {
                weaponListDef[i] = transform.Find("WeaponHolsterDef").GetChild(i).GetComponent<ArmadHeroes.Weapon>();
            }
            playerScore = 0;
            //  playerScoreText = GameObject.FindWithTag(("ShellShock/P" + (controllerID+1) + "SCORE")).GetComponent<Text>();
            //  playerScoreText.text = playerScore.ToString();
            #endregion
            #region BallStart
            ballObj = transform.Find("BallMode").gameObject;
            #endregion
            //need to load this into character animation override.
            //globalPlayerManager.GetCharacterName(controllerID);
            // gameObject.transform.localScale = new Vector3(5, 5, 5);



            // Debug.Log(globalPlayerManager.GetPlayerData(controllerID).character.ToString());
        }

        public void SetPlayerID(int ID)
        {
            playerID = ID;
        }

        protected override void Update()
        {
            if (!disablePlayerUpdate)
            {
                GetInput();
                ProcessMovement();
                ProcessAiming();
                ProcessFiring();
                ProcessHUD();
                base.Update();
                ProcessBall();
            }
        }


        private void GetInput()
        {
            // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
            // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

            //moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
            //moveVector.y = player.GetAxis("Move Vertical");
            //aimVector.x = player.GetAxis("Aim Horizontal");
            //aimVector.y = player.GetAxis("Aim Vertical");

            moveVector.x = controller.moveX.GetValue();   // get input by name or action id
            moveVector.y = controller.moveY.GetValue();
            aimVector.x = controller.aimX.GetValue();
            aimVector.y = controller.aimY.GetValue();

        }

        private void ProcessMovement()
        {
            // Process movement
            if (moveVector.x != 0.0f || moveVector.y != 0.0f)
            {
                mRigidBody2D.AddForce(mMovementSpeed * new Vector2(moveVector.x, moveVector.y));
                accolade_distance += new Vector2(moveVector.x, moveVector.y).magnitude;
                playerAnimator.SetBool("walking", true);
            }
            else
            {
                playerAnimator.SetBool("walking", false);
            }
        }
        private void ProcessAiming()
        {
            Vector2 inputAimDirection = new Vector2(aimVector.x, aimVector.y);

            if (inputAimDirection.magnitude > 0.4 && !isBallin)
            {
                float aimAngle = GetAngleWithSign(inputAimDirection);
                //Converts the angle between 0 and 360 degrees to an integer between 0 and 8 which corresponds to a sprite facing that angle
                int tempSpriteNumber = Mathf.RoundToInt(GetAngleWithSign(inputAimDirection));

                mOrientation = (tempSpriteNumber > 360 - (45 / 2) ? tempSpriteNumber - 360 - (45 / 2) : tempSpriteNumber) / 45;
                //mFiringOrientation = (tempSpriteNumber > 360 - (45 / 2) ? tempSpriteNumber - 360 - (45 / 2) : tempSpriteNumber) / 45;
                aimAngle = (mOrientation * 45);


                reticleRenderer.enabled = true;
                mLaserDirection = new Vector2((Mathf.Sin(mOrientationAngles[mOrientation] * Mathf.Deg2Rad)), (Mathf.Cos(mOrientationAngles[mOrientation] * Mathf.Deg2Rad)));

                mCorrectedAimDirection = new Vector2((Mathf.Sin(aimAngle * Mathf.Deg2Rad)), (Mathf.Cos(aimAngle * Mathf.Deg2Rad)));
                mShootOrigin = mDefaultPosition + (Vector2)transform.position + (mCorrectedAimDirection);
                mReticle.transform.position = mShootOrigin;
                if (mOrientation > 4)
                {
                    mOrientation = 8 - mOrientation;
                }

                switch (mOrientation)
                {
                    case 0:
                    playerAnimator.SetFloat("angle", 180);
                    break;
                    case 1:
                    playerAnimator.SetFloat("angle", 135);
                    break;
                    case 2:
                    playerAnimator.SetFloat("angle", 90);
                    break;
                    case 3:
                    playerAnimator.SetFloat("angle", 30);
                    break;
                    case 4:
                    playerAnimator.SetFloat("angle", 0);
                    break;
                }
                //..playerAnimator.SetFloat("angle", aimAngle);

                //if im facing left x scale should be -1, right should be 1.
                if (aimAngle > 180)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    //   gameObject.transform.localScale = new Vector3(2, 2, 2);
                }
                else if (aimAngle < 180)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    // gameObject.transform.localScale = new Vector3(2, 2, 2);
                }

                //playerRenderer.sprite = SpriteManager.instance.ChangeSprite(mOrientation);


                //playerRenderer.sprite = SpriteManager.instance.ChangeSprite(mOrientation);
            }
            else
            {
                reticleRenderer.enabled = false;
            }
        }

        public float GetAngleWithSign(Vector2 dir)
        {
            float sAngle = Mathf.Sign(Vector3.Cross(dir, Vector3.up).z) * Mathf.RoundToInt(Vector2.Angle(dir, Vector2.up));
            return sAngle < 0 ? 360 + sAngle : sAngle;
        }

        public void ReticleRender()
        {
            if (isBallin == true && controller.boostButton.JustPressed())
            {
                playerAnimator.SetBool("enterBall", true);
                playerAnimator.SetBool("exitBall", false);

                //playerRenderer.enabled = false;
            }

            if (isBallin == false)
            {
                //playerRenderer.enabled = true;
            }
        }

        private void ProcessFiring()
        {

            //if (mEquippedWeapon && debugFirstWeaponEnabled)
            //{
            //    //if the fire button is pressed, fire the gun.
            //    //if (player.GetButton("Fire") && !isPlayerDead && !isBallin)
            //    if (controller.shootButton.IsDown() && !isPlayerDead && !isBallin)
            //    {
            //        //emit the bullet casing particle system
            //        minigunParticleSystem.Emit(1);

            //        mEquippedWeapon.Charge(mFiringOrientation);

            //        //if (!mEquippedWeapon.ShootNew(CorrectedAimDirection, ShootOrigin, controllerID + 1 /*mPlayerNumber*/))
            //        {
            //            //  mEquippedWeapon = ShellShock.WeaponManager.Instance.GetWeapon("Pistol").GetComponent<Weapon>();
            //            // ammoSlider.value = (float)mEquippedWeapon.RemainingAmmo / (float)mEquippedWeapon.MaximumAmmo;
            //        }
            //    }
               
            //    //if (mEquippedWeapon.hasLaserSight)
            //    //{
            //    //    mEquippedWeapon.UpdateLaserSight(mFiringOrientation);
            //    //}
            //}

            if (mSecondWeapon && debugSecondWeaponEnabled)
            {
                //if the fire button is pressed, fire the gun.
                //if (player.GetButton("Fire") && !isPlayerDead && !isBallin)

                //if (mSecondWeapon.hasLaserSight)
                //{
                //    if (isBallin)
                //    {
                //        mLaserSight.enabled = false;
                //    }
                //    else
                //    {
                //        mLaserSight.enabled = true;
                //    }

                //    mLaserSight.UpdateOrientation(mLaserDirection);
                //    mLaserSight.UpdatePosition(transform.position + (Vector3)mLaserDirection.normalized);
                //}

                if (controller.shootButton.IsDown() && !isPlayerDead && !isBallin)
                {
                    //emit the bullet casing particle system
                    minigunParticleSystem.Emit(1);

                    //Please for the love of god don't try to understand this horrible mess 
                    if (mSecondWeapon.firesBullets && mSecondWeapon.CheckCoolDown())
                    {
                        accolade_shotsFired++;
                    }
                    mSecondWeapon.Shoot(transform.position + (((Vector3)mShootOffsets[(gameObject.GetComponent<SpriteRenderer>().flipX == true ? 8 - mOrientation : mOrientation)] * 2) * 1.2f), new Vector2(Mathf.Sin(mOrientationAngles[mOrientation] * Mathf.Deg2Rad) * (gameObject.GetComponent<SpriteRenderer>().flipX == true ? -1 : 1), Mathf.Cos(mOrientationAngles[mOrientation] * Mathf.Deg2Rad)), playerNumber, this, Color.white, ActorType.Player, BulletModifier.vanilla);

                    if(mSecondWeapon.GetCurrentAmmo() == 0)
                    {
                        RunOutOfAmmo();
                    }
                    //Debug.Log(transform.position + ((Vector3)mShootOffsets[(gameObject.GetComponent<SpriteRenderer>().flipX == true ? 8 - mOrientation : mOrientation)] * 2));
                    //if (!mEquippedWeapon.ShootNew(CorrectedAimDirection, ShootOrigin, controllerID + 1 /*mPlayerNumber*/))
                    
                        //  mEquippedWeapon = ShellShock.WeaponManager.Instance.GetWeapon("Pistol").GetComponent<Weapon>();
                        //ammoSlider.value = (float)mSecondWeapon. / (float)mEquippedWeapon.MaximumAmmo;
                    
                }
                else if (controller.shootButton.JustReleased())
                {
                    mSecondWeapon.StopFire();
                    mLaserSight.enabled = false;
                }
                if (mSecondWeapon == mSecondWeapon.GetComponent<Weapon_Sniper>())
                {
                    mLaserSight.GetComponent<LineRenderer>().enabled = true;
                    if (isBallin)
                    {
                        mLaserSight.enabled = false;
                    }
                    else
                    {
                        mLaserSight.enabled = true;
                    }
                    mLaserSight.UpdateOrientation(mLaserDirection);
                    mLaserSight.UpdatePosition(transform.position + (Vector3)mLaserDirection.normalized);
                }
                if (mSecondWeapon != mSecondWeapon.GetComponent<Weapon_Sniper>())
                {
                    mLaserSight.enabled = false;
                    mLaserSight.GetComponent<LineRenderer>().enabled = false;
                }

            }
        }

        private void ProcessHUD()
        {
            //if L3 is held, the HUD will appear
            //if (player.GetButton("HUD"))
            if (controller.hudButton.IsDown())
            {
                playerHUD.SetActive(true);
            }
            else
            {
                playerHUD.SetActive(false);
            }
            ballModeBar.fillAmount = ballModeTick / ballModeBarMax;
        }
        //Handles Ball Mode- where the player can transform into a stationary ball and deflect bullets.
        private void ProcessBall()
        {
            if (ballModeTick <= ballModeTickThreshold)
            {
                ballModeStopped = true;
                ballStoppedTimer = 0;
            }
            if (controller.boostButton.IsDown())
            {
                //show the ball mode bar when the player holds the ball button down.
                int children = playerHUD.transform.childCount;
                for (int i = 0; i < children; i++)
                {
                    playerHUD.transform.GetChild(i).gameObject.SetActive(false);
                }
                playerHUD.transform.Find("BallModeBar").gameObject.SetActive(true);
                playerHUD.transform.Find("BallModeBarBackground").gameObject.SetActive(true);
                playerHUD.SetActive(true);
            }
            else
            {
                //is ball mode button is not held down, then re-enable all HUD children for the HUD button.
                int children = playerHUD.transform.childCount;
                for (int i = 0; i < children; i++)
                {
                    playerHUD.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            //if the player holds the ball button down.
            if (controller.boostButton.IsDown() && !ballModeStopped/* && timeSpentBallin < 2.0f*/)
            {
                //starts a tick, which decrements only when ball mode is held down, meaning the player can use ball mode strategically, and multiple times before cooldown.
                accolade_unique += Time.deltaTime;
                ballModeTick -= drainSpeed;
                isBallin = true;
                PlayerRender();
                ReticleRender();
                ballObj.GetComponent<Ball>().BallRender();
            }
            else
            {
                if (isBallin)
                {
                    playerAnimator.SetBool("exitBall", true);
                    playerAnimator.SetBool("enterBall", false);
                }

                isBallin = false;
                PlayerRender();
                ReticleRender();
                ballObj.GetComponent<Ball>().BallRender();

                //the ball mode bar will fill even if it is not empty
                if (ballModeTick < ballModeBarMax)
                {
                    ballStoppedTimer += Time.deltaTime;
                    //ballModeTick += (ballStoppedTimer / ballModeTick) * ballModeBarMax;
                    ballModeTick += autoRechargeSpeed;
                }
            }
            //if ball mode has been disabled, start the timer to reactivate it.
            if (ballModeStopped)
            {
                ballStoppedTimer += Time.deltaTime;
                ballModeTick = (ballStoppedTimer / ballStoppedTimerThreshold) * ballModeBarMax;
            }
            if (ballStoppedTimer >= ballStoppedTimerThreshold)
            {
                //reactivate ball mode.
                ballModeStopped = false;
                ballStoppedTimer = 0;
            }
        }
        void PlayerRender()
        {
            if (isBallin == false)
            {
                playerCollider.enabled = true;
                //playerRenderer.enabled = true;
            }

            if (isBallin == true)
            {
                playerCollider.enabled = false;
                //playerRenderer.enabled = false;
            }
        }

        public void ChangeWeapon()
        {
           // randomWeapon = 5;
            randomWeapon = Random.Range(0, weaponList.Length);
            //Destroy(mSecondWeapon.gameObject);
            mSecondWeapon = weaponList[randomWeapon];
            weaponList = new ArmadHeroes.Weapon[transform.Find("WeaponHolster").childCount];
            for (int i = 0; i < transform.Find("WeaponHolster").childCount; i++)
            {
                weaponList[i] = transform.Find("WeaponHolster").GetChild(i).GetComponent<ArmadHeroes.Weapon>();
            }
            mSecondWeapon.transform.position = transform.position;
           // mSecondWeapon.transform.parent = gameObject.transform;
            mSecondWeapon.Reload();
            //mEquippedWeapon.transform.position = transform.position;
            //mEquippedWeapon.transform.parent = gameObject.transform;
        }

        public void RunOutOfAmmo()
        {
            // to refer when a player run out of ammo
            mSecondWeapon.StopFire();
            mSecondWeapon = weaponListDef[0];

        }
        public void SendData()
        {
            GlobalPlayerManager.instance.SetDebriefStats(playerID, chevron_score, accolade_timesShot, accolade_distance, accolade_distance, accolade_shotsFired, accolade_unique);
        }
    }
}
