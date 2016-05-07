using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace Armatillery
{
    public class powerUpManager : MonoBehaviour
    {
        #region Singleton
        private static powerUpManager m_instance;
        public static powerUpManager instance
        {
            get { return m_instance; }
        }
        #endregion

        #region Public Members
        public AudioClip heliSound;
        public GameObject helicopter;
        public List<GameObject> objectsToSpawn = new List<GameObject>();
        public List<GameObject> m_powerUps = new List<GameObject>();
        public List<float> cooldowns = new List<float>();
        public bool inWave = false;
        public bool countDown;
        public GameObject dropBoundsObject;
        public List<Bounds> dropBounds = new List<Bounds>();
        #endregion

        #region Private Members
        Vector3 spawnPos, heliTarget;
        bool delivering;
        float newVol = 0;
        AudioSource sound;
        int totalDrops,maxDrops;
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            //set up singleton
            m_instance = this;
            Collider2D[] temp = dropBoundsObject.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D c in temp)
            {
                dropBounds.Add(c.bounds);
            }

            var temp2 = dropBoundsObject.GetComponentsInChildren<Collider>();

            foreach(Collider c in temp2)
            {
                dropBounds.Add(c.bounds);
            }
        }

        void Update()
        {
            switch (GameManager.instance.state)
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
        #endregion

        public void Init()
        {
            begin();

            //Instantiate objects for each player
            for (int i = 0; i < ArmaPlayerManager.instance.m_spawnedPlayers.Count; i++)
            {
                float f = 0;
                cooldowns.Add(f);
                foreach (GameObject g in m_powerUps)
                {
                    GameObject newPowerUp = GameObject.Instantiate(g);
                    newPowerUp.SetActive(false);
                    objectsToSpawn.Add(newPowerUp);
                }
            }

            heliTarget = helicopter.transform.position;
            maxDrops = ArmaPlayerManager.instance.m_spawnedPlayers.Count;
            totalDrops = objectsToSpawn.Count;
        }

        void Tick()
        {
            if (helicopter.transform.position != heliTarget)
            {
                moveHeli();
            }
            else
            {
                if (sound)
                    sound.volume = Mathf.Lerp(sound.volume, 0, Time.deltaTime);
            }

            if (inWave)
            {
                //Countdown cooldowns
                for (int i = 0; i < cooldowns.Count; i++)
                {
                    if (cooldowns[i] > 0)
                        cooldowns[i] -= Time.deltaTime;

                    if (cooldowns[i] < 0)
                        cooldowns[i] = 0;
                }

                Random.seed = System.DateTime.Now.Millisecond;
                switch (GameManager.instance.state)
                {
                    case GameStates.game:
                        if (totalDrops - objectsToSpawn.Count < maxDrops)
                        {
                            for (int i = 0; i < ArmaPlayerManager.instance.m_spawnedPlayers.Count; i++)
                            {
                                if (cooldowns[i] == 0)
                                {
                                    if (helicopter.transform.position == heliTarget && objectsToSpawn.Count > 0)
                                    {
                                        begin();
                                        cooldowns[i] = Random.Range(10, 16);
                                    }
                                }
                            }
                        }
                        break;
                    case GameStates.pause:
                        break;
                    case GameStates.gameover:
                        break;
                    default:
                        break;
                }
            }
        }

        public void disablePowerup(GameObject powerUp)
        {
            powerUp.GetComponent<Animator>().Stop();
            powerUp.SetActive(false);
            objectsToSpawn.Add(powerUp);
        }

        //Set helicopter Y pos to be a set amount above the powerup drop location
        //When 
        void begin()
        {
            //Do some sound
            if (sound)
            {
                sound.Stop();
            }
            sound = SoundManager.instance.PlayClip(heliSound);
            sound.volume = 0;
            sound.loop = true;

            //Create a spawn position which the powerup will be dropped.
            //Old method
            /*
            float angle = Random.value * Mathf.PI * 2;
            float x = Mathf.Cos(angle * 5) * Random.Range(3, 12f);
            float y = Mathf.Sin(angle * 5) * Random.Range(1.75f, 4.5f);
            */

            //New method!
            Bounds b = dropBounds[Random.Range(0, dropBounds.Count - 1)];

            float y = Random.Range(b.min.y, b.max.y);
            float x = Random.Range(b.min.x, b.max.x);

            spawnPos = new Vector3(x, y,0);
            
            //Position the heli above the spawn pos, so the box can float down
            helicopter.transform.position = new Vector3(helicopter.transform.position.x, y + 5, 0);

            //Point the helicopter the right direction
            if (helicopter.transform.position.x > 0)
            {
                heliTarget = new Vector3(-21, y + 5f,0);
                helicopter.GetComponent<SpriteRenderer>().flipX = true;
                helicopter.GetComponentsInChildren<SpriteRenderer>()[1].flipX = true;
            }
            else
            {
                heliTarget = new Vector3(21, y + 5f,0);
                helicopter.GetComponent<SpriteRenderer>().flipX = false;
                helicopter.GetComponentsInChildren<SpriteRenderer>()[1].flipX = false;
            }
            
            delivering = true;
            newVol = 1;
        }

        void moveHeli()
        {
            if (!sound)
                sound = SoundManager.instance.PlayClip(heliSound);

            sound.volume = Mathf.Lerp(sound.volume, newVol, Time.deltaTime * 0.75f);

            if (newVol - sound.volume < 0.15f)
                newVol = 0;

            if (helicopter.GetComponent<SpriteRenderer>().flipX)
            {
                helicopter.transform.Translate(-Time.deltaTime * 10, 0, 0);
                if (helicopter.transform.position.x < heliTarget.x)
                    helicopter.transform.position = heliTarget;
            }
            else
            {
                helicopter.transform.Translate(Time.deltaTime * 10, 0, 0);
                if (helicopter.transform.position.x > heliTarget.x)
                    helicopter.transform.position = heliTarget;
            }
            
            if (delivering)
            {
                if (Vector3.Distance(helicopter.transform.position, spawnPos) < 5.25f)
                {
                    dropPowerUp();
                    delivering = false;
                }
            }
        }

        void dropPowerUp()
        {
            Random.seed = System.DateTime.Now.Millisecond/52;
            int spawnMe = Random.Range(0, objectsToSpawn.Count - 1);
            objectsToSpawn[spawnMe].transform.position = spawnPos;
            objectsToSpawn[spawnMe].GetComponent<Animator>().Play("powerupDrop");
            objectsToSpawn[spawnMe].SetActive(true);
            objectsToSpawn[spawnMe].GetComponentInChildren<powerup>().Init();
            objectsToSpawn.Remove(objectsToSpawn[spawnMe]);
        }
    }
}
