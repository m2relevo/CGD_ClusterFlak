using UnityEngine;
using System.Collections.Generic;

namespace DilloDash
{

    public class WeaponSpawner : MonoBehaviour
    {
        // the prefabs that will be instantiated
        [SerializeField] GameObject[] weaponsToSpawn;
        [HideInInspector] public GameObject weaponSpawned;

        bool randomWeapon = false;
        [SerializeField] bool instantSpawn = true;
        [SerializeField] bool repeatSpawn = true;
        [SerializeField] float spawnDelay = 10.0f;
        [SerializeField] private int weight_Laser = 15, weight_Rocket = 35, weight_Mine = 35, weight_Boost = 15;


        // Use this for initialization
        void Start()
        {
            randomWeapon = weaponsToSpawn.Length > 1 ? true : false;

            // if instant spawn
            if (instantSpawn)
                spawnWeapon();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void spawnWeapon()
        {
            // if multiple weapon options
            if (randomWeapon)
                // spawn a random one
                weaponSpawned = Instantiate(weaponsToSpawn[Random.Range(0, weaponsToSpawn.Length)]);
            else
                // spawn the first one
                weaponSpawned = Instantiate(weaponsToSpawn[0]);

            // move the weapon to the gameobject location
            weaponSpawned.transform.position = gameObject.transform.position;
            weaponSpawned.transform.parent = gameObject.transform;

            PowerUps power = weaponSpawned.AddComponent<PowerUps>();
            power.weight_Boost = weight_Boost;
            power.weight_Laser = weight_Laser;
            power.weight_Mine = weight_Mine;
            power.weight_Rocket = weight_Rocket;
            power.init(); // randomly chooses which powerup
            power.mySpawner = this;

            
        }

        public virtual void weaponCollected()
        {
            // if the spawner keeps spawning weapons
            if (repeatSpawn)
            {
                Invoke("spawnWeapon", spawnDelay);
                Destroy(weaponSpawned);
            }
            // else destroy the spawner
            else
                Destroy(gameObject);
        }
    }
}