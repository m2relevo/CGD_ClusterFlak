using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ShellShock
{
    public class Player : MonoBehaviour
    {
        public Weapon mEquippedWeapon;
        Weapon[] weaponList;
        public int mPlayerNumber;


        public int PlayerNumber
        {
            get
            {
                return mPlayerNumber;
            }
        }

        public Slider HPSlider;
        public Slider ammoSlider;

        int mHealth;
        int mMaxHealth;

        int mSheildDamageReduction;

        public float waitTime;
        float mMaxSheildTime;
        float mRemainingSheidTime;
        Vector2 mReticlePosition;
        public ParticleSystem minigunParticleSystem;
        public GameObject playerHUD;

        public int playerScore;

        public bool isPlayerDead = false;
        public float timeToRespawn = 3f;
        public float timeSpentDead = 0f;
        public int killingBulletID;
        public Text playerScoreText;

        public Vector2 ReticlePosition
        {
            get
            {
                return mReticlePosition;
            }
        }

        void Awake()
        {

        }

        void Start()
        {
            mReticlePosition = mEquippedWeapon.transform.position;
            mEquippedWeapon.transform.position = transform.position;
            mEquippedWeapon.transform.parent = transform;
            ammoSlider.maxValue = 1;
            ammoSlider.value = 1;

            mMaxHealth = 100;
            mHealth = mMaxHealth;
            playerScore = 0;
            playerScoreText.text = playerScore.ToString();
        }

        void Update()
        {
            playerScoreText.text = playerScore.ToString();

            //if L3 is held, the HUD will appear
            if (Input.GetButton("Player_" + mPlayerNumber + "_Back"))
            {
                playerHUD.SetActive(true);
            }
            else
            {
                playerHUD.SetActive(false);
            }

            //waitTime -= Time.deltaTime;
            if (mEquippedWeapon)
            {
                //if the fire button is pressed, fire the gun.
                if (Input.GetButton("Player_" + mPlayerNumber + "_Fire1") && !isPlayerDead)
                {
                    //emit the bullet casing particle system
                    minigunParticleSystem.Emit(1);

                    if (!mEquippedWeapon.ShootNew(GetComponent<ShellShock.Aiming>().CorrectedAimDirection, GetComponent<ShellShock.Aiming>().ShootOrigin, mPlayerNumber))
                    {
                        mEquippedWeapon = ShellShock.WeaponManager.Instance.GetWeapon("Pistol").GetComponent<Weapon>();
                        ammoSlider.value = (float)mEquippedWeapon.RemainingAmmo / (float)mEquippedWeapon.MaximumAmmo;
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Projectile")
            {
                mHealth -= coll.gameObject.GetComponent<ShellShock.Projectile>().Damage;
                HPSlider.value = (float)mHealth / (float)mMaxHealth;
                if (mHealth <= mMaxHealth / 2)
                {
                    if (mHealth <= mMaxHealth / 4)
                    {
                        HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                    }
                    else {
                        HPSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.yellow;//TODO:Make this not super hacky (possibly with a script on the slider that holds references)
                    }
                }
                //if the player's health is 0, the player game object will be disabled.
                if (HPSlider.value == 0)
                {
                    ShellShock.RespawnManager.Instance.Kill(gameObject, timeToRespawn);
                    Debug.Log("Player_" + mPlayerNumber + "  has died");
                    killingBulletID = coll.gameObject.GetComponent<ShellShock.Projectile>().OwnerID;
                    IncreaseKillersScore();
                    ResetPlayerProperties();
                }

                Debug.Log("Player_" + mPlayerNumber + " took damage from Player_" + coll.gameObject.GetComponent<ShellShock.Projectile>().OwnerID + "'s projectile");
                Destroy(coll.gameObject);
            }
        }

        public void IncreaseKillersScore()
        {
            Player[] players = FindObjectsOfType(typeof(Player)) as Player[];
            foreach (Player player in players)
            {
                if (player.mPlayerNumber == killingBulletID && killingBulletID != mPlayerNumber)
                {
                    player.playerScore++;
                }
                if (player.mPlayerNumber == killingBulletID && killingBulletID == mPlayerNumber)
                {
                    playerScore--;
                }
            }
        }

        public void Spawn(Vector2 spawnPos)
        {
            gameObject.transform.position = spawnPos;
        }

        public void ResetPlayerProperties()
        {
            //reset player's health
            mHealth = mMaxHealth;
            HPSlider.value = 1;
            ammoSlider.value = 1;
            //equip the player with a fully loaded pistol.
            mEquippedWeapon = WeaponManager.Instance.GetWeapon("Pistol").GetComponent<Weapon>();
            mEquippedWeapon.RemainingAmmo = mEquippedWeapon.MaximumAmmo;

        }

        public void StartRespawnTimer()
        {
            isPlayerDead = true;
        }

        public void ChangeWeapon(int weaponNumber)
        {
            Destroy(mEquippedWeapon.gameObject);
            mEquippedWeapon = ShellShock.WeaponManager.Instance.GetWeapon(weaponNumber).GetComponent<Weapon>();
            mEquippedWeapon.transform.position = transform.position;
            mEquippedWeapon.transform.parent = gameObject.transform;
        }
    }
}
