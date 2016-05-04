using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ArmadHeroes;

//Used Actor_Armad as a reference guide

namespace ArmadHeroes
{
    public class CF_Actor : Actor
    {
        #region PlayerActor Ripped Public Members
        public float accolade_unique = 0;
        public float accolade_timesShot = 0;
        public float accolade_shotsFired = 0;
        public float accolade_distance = 0;
        public int chevron_score = 0;
        public int ControllerID { get { return m_controllerID; } }
        #endregion
        
        //Controller
        public ArmadHeroes.Controller controller
        {
            private set { }
            get { return ArmadHeroes.ControllerManager.instance.GetController(m_controllerID); }
        }

        protected int m_controllerID;
        protected bool walking = false;
        protected float footstepCooldown;
        public ParticleSystem leftFootprint, rightFootprint;
        public AudioClip footstepSfx;

        #region CF Variables
        //Local Movement Speed
        public float speed = 5;

        //Local Other bullet Stuff
        public GameObject StandardBullet;
        public GameObject ProjectileSpawnLoc;

        //AMMO and HP values
        private float MaxHP;
        private float CurrentHP;
        private float MaxAMMO;
        private float CurrentAMMO;

        //Element Elements
        public Slider HP_Slider;
        public Image HP_FillImage;
        public Color HP_FullHealthColor = Color.green;
        public Color HP_ZeroHealthColor = Color.red;

        public Slider AMMO_Slider;
        public Image AMMO_FillImage;
        public Color AMMO_FullHealthColor = Color.green;
        public Color AMMO_ZeroHealthColor = Color.red;



        //Modes
        bool IsAimOn = false;

        //junk
        private int height = 0;
        private float Anglex;
        private float Angley;

        #endregion

        //Player ID
        // protected int playerID = 0;
        protected int controllerID;

        void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {
            MaxHP = health;
            CurrentHP = MaxHP;
            SetHealthUI();


            EquipWeapon(m_machinegun);
        }

        // Update is called once per frame
        void Update()
        {
            #region Movement Update
            //Movement and aim by left stick and aim by right stick
            float Movex = controller.moveX.GetValue();
            float Movey = controller.moveY.GetValue();

            Vector2 direction = new Vector2(Movex, Movey).normalized;

            Vector2 stickAngle = new Vector2(controller.moveX.GetValue(), controller.moveY.GetValue());//stick angle method from ArmaPlayer.CS/Armatillery

            shootDir = stickAngle;

            if (IsAimOn == false)
            {
                Move(direction);
            }

            #endregion

            #region Firing Button events
            if (controller.shootButton.IsDown() && stickAngle != Vector2.zero)//fire gun button...duh. It says shootButton. Vector2.Zero taken from Player.CS/ZonePatrol
            {
                Vector3 Projectilespawn = ProjectileSpawnLoc.transform.position;
                m_weapon.Shoot(Projectilespawn, shootDir, playerID, this, Color.blue, ActorType.Player, BulletModifier.vanilla, height);
                accolade_shotsFired++;
                Debug.Log("Weapon = " + m_weapon + "Fire Triggered");
                Debug.Log("Shots fired accolade: " + accolade_shotsFired);
            }
            #endregion

            #region Animator stuff
            Anglex = Movex;
            Angley = Movey;


            float angle = Mathf.Atan2(Anglex, -Angley) * Mathf.Rad2Deg;

            if (direction != Vector2.zero)//Vector2.Zero taken from Player.CS/ZonePatrol
            {
                m_armaAnimator.SetBool("walking", true);
                spriteRenderer.gameObject.transform.localScale = getScale(angle);
                m_armaAnimator.SetFloat("angle", Mathf.Abs(angle));

            }

            if (direction == Vector2.zero || IsAimOn == true)//Vector2.Zero taken from Player.CS/ZonePatrol
            {
                m_armaAnimator.SetBool("walking", false);
            }
            #endregion

            #region Other Button Press Update
            if (controller.anchorButton.IsDown())//'Aim' Mode
            {
                IsAimOn = true;
            }

            if (controller.anchorButton.JustReleased())
            {
                IsAimOn = false;
            }

            if (controller.pauseButton.JustPressed())//'pause' event
            {
                if (ArmadHeroes_Pause.instance != null)
                {
                    if (m_weapon != null) { m_weapon.StopFire(); }
                    ArmadHeroes_Pause.instance.Pause(m_controllerID);
                }
            }

            #endregion



            switch (GameManager.instance.state)//states Ripped out of Updated Player Actor cus I dont know
            {
                case GameStates.game:
                    Tick();
                    break;
                case GameStates.pause:
                    break;
                case GameStates.gameover:
                    break;
                default:
                    break;
            }
        }

        #region Movement Code
        void Move(Vector2 direction)
        {
            Vector2 pos = transform.position;

            pos += direction * moveSpeed * Time.deltaTime;
            transform.position = pos;
            if (direction != Vector2.zero)
            {
                accolade_distance++;
            }
        }
        #endregion

        #region Other Button Events
        protected virtual void Pause()
        {
            if (controller.pauseButton.JustPressed())
            {
                if (ArmadHeroes_Pause.instance != null)
                {
                    if (m_weapon != null) { m_weapon.StopFire(); }
                    ArmadHeroes_Pause.instance.Pause(m_controllerID);
                }
            }
        }
        #endregion

        #region OnTriggerEnter2D Events
        //Enemy Bullet Collision
        void OnTriggerEnter2D(Collider2D Enemy)
        {
            //enemy bullet
            if (Enemy.gameObject.tag == "ClusterFlak/EnemyBulletMG")
            {
                CurrentHP -= 5;
                SetHealthUI();
                accolade_timesShot++;
                Death();
            }

            if (Enemy.gameObject.tag == "ClusterFlak/EnemyBulletRIF")
            {

                CurrentHP -= 3;
                SetHealthUI();
                accolade_timesShot++;
                Death();
            }

            if (Enemy.gameObject.tag == "ClusterFlak/EnemyBulletMIS")
            {

                CurrentHP -= 10;
                SetHealthUI();
                accolade_timesShot++;
                Death();
            }


        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "ClusterFlak/WeaponPickup")
            {
                int RNG = Random.Range(0, 3);

                if (RNG == 0)
                {
                    EquipWeapon(m_machinegun);
                    Debug.Log("Weapon Pickup = " + m_weapon);
                }

                if (RNG == 1)
                {
                    EquipWeapon(m_shotgun);
                    Debug.Log("Weapon Pickup = " + m_weapon);
                }

                if (RNG == 2)
                {
                    EquipWeapon(m_sniper);
                    Debug.Log("Weapon Pickup = " + m_weapon);
                }
            }
        }
        #endregion

        protected virtual void Tick()
        {
            if (m_weapon)
            {
                m_weapon.Tick();
            }
        }//ipped out of player actor to update this class

        //UI Elements
        private void SetHealthUI()
        {
            HP_Slider.value = CurrentHP;

            HP_FillImage.color = Color.Lerp(HP_ZeroHealthColor, HP_FullHealthColor, CurrentHP / MaxHP);
        }

        void Death()
        {
            if (CurrentHP <= 0)
            {
                Destroy(gameObject);
            }
        }

        //for those flpping sprites
        private Vector3 getScale(float angle)
        {
            if (angle > 0f && angle < 180f)
            {
                return new Vector3(1f, 1f, 1f);
            }
            else
            {
                return new Vector3(-1f, 1f, 1f);
            }
        }
    }
}