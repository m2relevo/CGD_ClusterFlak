using UnityEngine;
using System.Collections;
namespace DilloDash
{
    public struct PowerStruct
    {
        public Powerup myPowerup;
        public string powerName;
    }
    public enum Powerup {BoostPowerup, RocketPowerup, MinePowerup, LaserPowerup, nothing }
    public class PowerUps : MonoBehaviour
    {
        private Timer timer = null;
        //private Powerup myPowerup;
        private PowerStruct m_PowerStruct;

        public WeaponSpawner mySpawner;

        public bool isPowerupThere = true;
        public bool powerupUsed = false;
        [SerializeField] private float respawnTime = 2.0f;
        public int weight_Laser , weight_Rocket , weight_Mine , weight_Boost ;
        int power;

        void Awake()
        {
            timer = new Timer();
            timer.QuickTimer(respawnTime, ResetPower);
        }

        void Update()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                timer.Update();
            }
        }

        public void PowerPickedUp()
        {
            isPowerupThere = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            timer.BeginTimer();
        }

        public void ResetPower()
        {
            init();
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        public void init()
        {
            // allow the powerup to be picked up
            isPowerupThere = true;

            // delay here

            int temp = Random.Range(0, weight_Boost + weight_Rocket + weight_Mine + weight_Laser);

            if (temp < weight_Boost)
                power = 0;
            else if (temp < weight_Boost + weight_Rocket)
                power = 1;
            else if (temp < weight_Boost + weight_Rocket + weight_Mine)
                power = 2;
            else
                power = 3;

            // choose the effect
            switch (power)
            {
                case 0:
                    m_PowerStruct.myPowerup = Powerup.BoostPowerup;
                    m_PowerStruct.powerName = "BOOST";
                    break;
                case 1:
                    m_PowerStruct.myPowerup = Powerup.RocketPowerup;
                    m_PowerStruct.powerName = "RPG";
                    break;
                case 2:
                    m_PowerStruct.myPowerup = Powerup.MinePowerup;
                    m_PowerStruct.powerName = "MINE";
                    break;
                case 3:
                    m_PowerStruct.myPowerup = Powerup.LaserPowerup;
                    m_PowerStruct.powerName = "LASER";
                    break;
                
            }
            
            if (levelGeneratorDD.instance.powerupDebug)
                m_PowerStruct.myPowerup = levelGeneratorDD.instance.powerupOverride;
        }

		//get Powerup id
       public Powerup GetPowerUpId()
        {
            return m_PowerStruct.myPowerup;
        }

		//set Powerup id
		public void SetPowerUpId(Powerup _myPowerup)
		{
            m_PowerStruct.myPowerup = _myPowerup;
		}

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.tag == "Player")
            {
                DilloDashPlayer player = coll.gameObject.GetComponent<DilloDashPlayer>();
                if (player.myPowerUp == Powerup.nothing)
                {
                    player.setPowerup(m_PowerStruct.myPowerup);
                    coll.gameObject.GetComponentInChildren<ArmadHeroes.ArmaCanvas>().ActivateWeaponPickup(m_PowerStruct.powerName);
                }
                mySpawner.weaponCollected();
            } 
        }
    }
}