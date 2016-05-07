using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;
using ArmadHeroes;

namespace ShellShock
{
    public class PlayerLogic : MonoBehaviour
    {
       // public NewWeapon mEquippedWeapon;
       
        public RewiredController rewiredController;
        //  public Slider               HPSlider;
        //  public Slider               ballModeSlider;
        public float mHealth;
       public  float mMaxHealth;
        int mSheildDamageReduction;
        public int playerScore;
        public int killingBulletID;
        public int mPlayerNumber;
        float mMaxSheildTime;
        float mRemainingSheidTime;
        public float timeToRespawn = 3f;
        // Vector2                     mReticlePosition;
        public bool isPlayerDead = false;
        public Text playerScoreText;
        public GameObject playerScoreTextObject;
        public float weaponsChanged;
        public bool acc_playerKilled = false;
        public bool canTravel = true;
        public float travelTimer = 3;
        public SiloManager siloManager;
        public SiloScript siloScript;
        public int siloHitId;
        public AudioSource siloEnterSound;

        public Image healthBar;

        public GameObject laserBeam;


        public AudioSource ricochetBallSound;


        private float lateStartTimer = 0f;
        private float lateStartThreshold = 0.1f;
        private bool callLateStart = false;

        public int PlayerNumber
        {
            get
            {
                return mPlayerNumber;
            }
        }

        //public Vector2 ReticlePosition
        //{
        //    get
        //    {
        //        return mReticlePosition;
        //    }
        //}
        void LateStart()
        {
            InitialisePlayerProperties();
        }
        void ProcessLateStart()
        {
            if (!callLateStart)
            {
                lateStartTimer += Time.deltaTime;
                if (lateStartTimer >= lateStartThreshold)
                {
                    lateStartTimer = 0;
                    callLateStart = true;
                    LateStart();
                }
            }
        }

        void Awake()
        {
            // HPSlider = transform.Find("Canvas").transform.Find("PlayerHUD").transform.Find("PlayerHP1").GetComponent<Slider>();
            //ballModeSlider = transform.Find("Canvas").transform.Find("PlayerHUD").transform.Find("PlayerAmmoCountSlider").GetComponent<Slider>();
            //playerScoreText = GameObject.FindWithTag(("ShellShock/P" + (mPlayerNumber) + "SCORE")).GetComponent<Text>();

            rewiredController = gameObject.GetComponent<RewiredController>();
            healthBar = transform.Find("Canvas").transform.Find("PlayerHUD").transform.Find("HealthBar").GetComponent<Image>();
        }
        void Start()
        {
            GetComponent<ShellShock.SiloManager>();

            // Previous way to find the text score in the scene

            /* playerScoreTextObject = GameObject.Find("UI Layer").transform.Find("P" + (gameObject.GetComponent<PlayerActor>().playerNumber + 1) + "Score").gameObject;
             playerScoreText = playerScoreTextObject.GetComponent<Text>();
             playerScoreText.enabled = true;*/
            //playerScoreText = GameObject.Find("UI Layer").transform.Find("P" + (gameObject.GetComponent<RewiredController>().controllerID + 1) + "Score").GetComponent<Text>();



            // mReticlePosition = mEquippedWeapon.transform.position;
            // mEquippedWeapon.transform.position = transform.position;
            //  mEquippedWeapon.transform.parent = transform;

            //ballModeSlider.maxValue = 1;
            // ballModeSlider.value = 1;



            //mMaxHealth = 100;
            mMaxHealth = gameObject.GetComponent<RewiredController>().health;
            mHealth = mMaxHealth;
            playerScore = 0;
            weaponsChanged = 0;

            mPlayerNumber = rewiredController.playerNumber + 1;
            //playerScoreText.text = playerScore.ToString();
            siloEnterSound = GetComponent<AudioSource>();
            ricochetBallSound = GameObject.FindGameObjectWithTag("ShellShock/RICOCHETBALL").GetComponent<AudioSource>();
           
        }

        void Update()
        {
            ProcessLateStart();
            //playerScoreText.text = playerScore.ToString();
            //Debug.Log(GetComponent<RewiredController>().health);
            mHealth = GetComponent<RewiredController>().health;
           

            healthBar.fillAmount = (float)mHealth / (float)mMaxHealth;

            if (canTravel == false)
            {
                TravelCooldown();
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (GetComponent<RewiredController>().isBallin)
            {
                ricochetBallSound.Play();
            }
           // Debug.Log("COLLISION");
            ///*&& gameObject.GetComponent<RewiredController>().isBallin == false*/
            if (GetComponent<RewiredController>().isBallin == false)
            {
                if (coll.gameObject.tag == "Bullet" || coll.gameObject.GetComponent<Weapon_Flamethrower>())
                {
                   
                    gameObject.GetComponent<RewiredController>().accolade_timesShot++;

                    GetComponent<RewiredController>().health -= coll.gameObject.GetComponent<ArmadHeroes.Projectile>().damage;
                    //HPSlider.value = (float)mHealth / (float)mMaxHealth;
                    if (mHealth <= mMaxHealth / 2)
                    {
                        if (mHealth <= mMaxHealth / 4)
                        {
                            healthBar.color = Color.red;
                            // HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                        }
                        else
                        {
                            healthBar.color = Color.yellow;
                            // HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.yellow;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                        }
                    }
                    //if the player's health is 0, the player game object will be disabled.
                    if (/*HPSlider.value == 0 ||*/ /*healthBar.fillAmount == 0*/GetComponent<RewiredController>().health <= 0)
                    {
                        timeToRespawn = 3f;
                        //Debug.Log("Player_" + mPlayerNumber + "  has died");
                        killingBulletID = coll.gameObject.GetComponent<ArmadHeroes.Projectile>().callerID;
                        // CanvasManager.instance.setPlayerValue(coll.gameObject.GetComponent<ArmadHeroes.Projectile>().callerID, 1);
                        ChangeKillersScore(killingBulletID, gameObject.GetComponent<RewiredController>().playerNumber);
                        ShellShock.RespawnManager.Instance.Kill(gameObject, timeToRespawn);
                        ResetPlayerProperties();
                    }

                    //Debug.Log("Player_" + mPlayerNumber + " took damage from Player_" + coll.gameObject.GetComponent<ShellShock.Projectile>().OwnerID + "'s projectile");
                    //Destroy(coll.gameObject);
                }
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.gameObject.tag == "ShellShock/Silos" && canTravel == true)
            {
                //if (!transform.Find("WeaponHolster").transform.Find("Laser").transform.Find("LaserBeam(Sprite)").gameObject.activeInHierarchy /*|| transform.Find("WeaponHolster").transform.Find("Laser").transform.GetChild(0).gameObject == null*/)
                if(!laserBeam.activeInHierarchy)
                {
                    //Debug.Log("SiloHit");
                    if (other.GetComponent<SiloScript>().isOpen)
                    {
                        siloEnterSound.Play();
                        transform.position = other.gameObject.GetComponent<SiloScript>().GetPairedSiloPos();
                    }

                    //siloHitId = siloScript.siloId;
                    //siloManager.GetOpenSilos(siloHitId);
                }
            }
            if(other.gameObject.tag == "ShellShock/LASERBEAM")
            {
              //  Debug.Log("LASERED");
                gameObject.GetComponent<RewiredController>().accolade_timesShot++;

                GetComponent<RewiredController>().health -= other.gameObject.GetComponent<ArmadHeroes.Projectile>().damage;
                //HPSlider.value = (float)mHealth / (float)mMaxHealth;
                if (mHealth <= mMaxHealth / 2)
                {
                    if (mHealth <= mMaxHealth / 4)
                    {
                        healthBar.color = Color.red;
                        // HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                    }
                    else
                    {
                        healthBar.color = Color.yellow;
                        // HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.yellow;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                    }
                }
                //if the player's health is 0, the player game object will be disabled.
                if (/*HPSlider.value == 0 ||*/ /*healthBar.fillAmount == 0*/GetComponent<RewiredController>().health <= 0)
                {
                    timeToRespawn = 3f;
                    //Debug.Log("Player_" + mPlayerNumber + "  has died");
                    killingBulletID = other.gameObject.GetComponent<ArmadHeroes.Projectile>().callerID;
                    // CanvasManager.instance.setPlayerValue(coll.gameObject.GetComponent<ArmadHeroes.Projectile>().callerID, 1);
                    ChangeKillersScore(killingBulletID, gameObject.GetComponent<RewiredController>().playerNumber);
                    ShellShock.RespawnManager.Instance.Kill(gameObject, timeToRespawn);
                    ResetPlayerProperties();
                }

            }
        }

        public void ChangeKillersScore(int callerID, int playerID)
        {
            if( callerID != playerID)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length; i++)
                {
                    if(players[i].GetComponent<RewiredController>().playerNumber == callerID)
                    {
                        players[i].GetComponent<RewiredController>().chevron_score++;
                        CanvasManager.instance.setPlayerValue(callerID, players[i].GetComponent<RewiredController>().chevron_score);
                    }
                }
                
            }
            else
            {
                // CanvasManager.instance.setPlayerValue(callerID, -1);
                timeToRespawn = 8f;
            }
        

            //PlayerLogic[] players = FindObjectsOfType(typeof(PlayerLogic)) as PlayerLogic[];

            //foreach (PlayerLogic player in players)
            //{
            //    if (player.mPlayerNumber == killingBulletID && killingBulletID != mPlayerNumber)
            //    {
            //        player.playerScore++;
            //        acc_playerKilled = true;
            //    }
            //    else
            //    {
            //        //Debug.Log(player.mPlayerNumber +""+ mPlayerNumber +""+ killingBulletID); 
            //    }

            //    if (player.mPlayerNumber == killingBulletID && killingBulletID == mPlayerNumber)
            //    {
            //        playerScore--;
            //        acc_playerKilled = true;
            //    }
            //}
        }

        public void Spawn(Vector2 spawnPos)
        {
            gameObject.transform.position = spawnPos;
            acc_playerKilled = false;
        }

        public void InitialisePlayerProperties()
        {
            GetComponent<RewiredController>().health = mMaxHealth;
            healthBar.color = new Color(0, 246, 0);
           
            weaponsChanged = 0;
            //equip the player with a fully loaded pistol.
            //  mEquippedWeapon = WeaponManager.Instance.GetWeapon("Assault Rifle").GetComponent<NewWeapon>();
            // mEquippedWeapon.ResetAmmo();

            mHealth = GetComponent<RewiredController>().health;
            healthBar.fillAmount = 1;
            
            for (int i = 0; i < GetComponent<RewiredController>().playerHUD.transform.childCount; i++)
            {
                GetComponent<RewiredController>().playerHUD.transform.GetChild(i).gameObject.SetActive(true);
            }
            GetComponent<RewiredController>().ballModeTick = GetComponent<RewiredController>().ballModeBarMax;
            GetComponent<RewiredController>().ballModeBar.fillAmount = 1;
        }

        public void ResetPlayerProperties()
        {
            //reset player's health
            GetComponent<RewiredController>().health = mMaxHealth;
            healthBar.color = new Color(0, 246, 0);
            //  HPSlider.value = 1;
            // ballModeSlider.value = 1;
            weaponsChanged = 0;
            //equip the player with a fully loaded pistol.
            //  mEquippedWeapon = WeaponManager.Instance.GetWeapon("Assault Rifle").GetComponent<NewWeapon>();
            // mEquippedWeapon.ResetAmmo();
            GetComponent<RewiredController>().RunOutOfAmmo();
            GetComponent<RewiredController>().ballModeTick = GetComponent<RewiredController>().ballModeBarMax;
            GetComponent<RewiredController>().ballModeBar.fillAmount = GetComponent<RewiredController>().ballModeTick;
            mHealth = GetComponent<RewiredController>().health;
            for(int i = 0; i < GetComponent<RewiredController>().playerHUD.transform.childCount; i++)
            {
                GetComponent<RewiredController>().playerHUD.transform.GetChild(i).gameObject.SetActive(true);
            }
           // GetComponent<RewiredController>().playerHUD.SetActive(true);
            //gameObject.transform.Find("BallMode").GetComponent<AudioSource>().volume = 0;
        }

        public void StartRespawnTimer()
        {
            isPlayerDead = true;
        }

        //public void ChangeWeapon(int weaponNumber)
        //{

        //    Destroy(mEquippedWeapon.gameObject);
        //    mEquippedWeapon = ShellShock.WeaponManager.Instance.GetWeapon(weaponNumber).GetComponent<NewWeapon>();
        //    mEquippedWeapon.transform.position = transform.position;
        //    mEquippedWeapon.transform.parent = gameObject.transform;
        //    weaponsChanged++;
        //}

        public void TravelCooldown()
        {
            travelTimer -= Time.deltaTime;
            if (travelTimer <= 0)
            {
                canTravel = true;
                travelTimer = 3;
            }
        }

        public void Teleport(Vector3 telePos)
        {
            this.transform.position = telePos;
            canTravel = false;
        }
    }
}
