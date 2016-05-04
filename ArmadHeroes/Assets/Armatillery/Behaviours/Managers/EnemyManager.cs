/// <summary>
/// Manages all Enemies
/// Created and implemented by Sam Endean - 17/01/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ArmadHeroes;

namespace Armatillery
{
    public enum EnemyManagerStates { Setup, Spawn };
    public class EnemyManager : MonoBehaviour
    {
        #region Singleton
        private static EnemyManager m_instance = null;
        public static EnemyManager instance { get { return m_instance; } }
        #endregion

        #region Public Members
        [HideInInspector]
        public bool FirstRound = true;
        [HideInInspector]
        public int poolAmount = 50;//amount of enemies to pool
        //[HideInInspector]
        public float spawnInterval = 8f; //the minimum spawn interval for each
        [HideInInspector]
        public List<GameObject> m_spawnedenemies = new List<GameObject>(); //list of enemies spawned in game
        public List<GameObject> m_enemies = new List<GameObject>(); //list of pooled enemies
        public List<GameObject> dropShips; // pooled list of floating enemies that can be spawned away from spawner;
        [HideInInspector]
        public GameObject EnemyActor,
          dropshipsHolder;
        public AudioClip ExplosionSpawn;
        #endregion

        #region Private Members
        private List<float> m_spawnerTimers; //this holds the timers for all spawner separately for malleability
        private List<Vector2> m_spawnPoints = new List<Vector2>();
        private float startingInterval; //the minimum spawn interval for each
        private bool StartSpawn = false;
        private int poolHead = 0;
        private List<GameObject> m_spawnPool = new List<GameObject>(); // List of enemies to spawn
        private float m_timeLastSpawnedEnemies = 0f; //Time last spawned an enemy
        private float m_timeBetweenSpawns = 0.5f; //How long between enemies
        #endregion

        #region Enemy Manager State
        private EnemyManagerStates m_state;
        public EnemyManagerStates state { get { return m_state; } set { m_state = value; ChangeState(value); } }
        private void ChangeState(EnemyManagerStates _state)
        {
            Debug.Log("EnemyManagerState: " + m_state);
            switch (_state)
            {
                case EnemyManagerStates.Setup:
                    OnEnterSetUp();
                    break;
                case EnemyManagerStates.Spawn:
                    StartCoroutine(IStartCurrentWave());
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Called when set-up state is set
        /// </summary>
        private void OnEnterSetUp()
        {
            //if not on first round decrement spawn interval
            if (!FirstRound) { spawnInterval = spawnInterval > 1 ? spawnInterval - 1 : 1; }
            //stop all spawning
            StartSpawn = false;
            //increase enemy speed - bring the pace
            for (int i = 0; i < m_spawnedenemies.Count; i++)
            {
                if (m_spawnedenemies[i].activeSelf)
                {
                    m_spawnedenemies[i].GetComponent<ArmaEnemy>().Sprint();
                }
            }
        }
        #endregion
        
        #region Unity Callbacks
        void Awake()
        {
            //set up singleton
            m_instance = this;
         
            m_enemies = new List<GameObject>();
            m_enemies.Clear();
            m_spawnerTimers = new List<float>();
            m_spawnerTimers.Clear();
            startingInterval = spawnInterval;
        }

        void Start()
        {
            for (int i = 0; i < poolAmount; i++)
            {
                //init new enemy
                GameObject enemy = Instantiate(EnemyActor);
                enemy.SetActive(false);//make sure they are turned off
                enemy.name = "Enemy " + i;//give name
                enemy.GetComponent<Actor>().EquipWeapon(enemy.GetComponent<Actor>().m_machinegun);//change to random
                //enemy.GetComponent<ArmaEnemy>().m_weapon.Init(ActorType.Enemy);
                enemy.transform.parent = this.gameObject.transform;//set parent to manager
                enemy.GetComponent<Actor>().health = 1.0f;//set health

                m_enemies.Add(enemy);//add to list of pooled enemies
            }
            Init();
        }

        public void Init()
        {
            spawnInterval = startingInterval;
            //set initial state to setup which waits for world gen to feed it enemies 
            state = EnemyManagerStates.Setup;
            //make sure lists are fresh and empty
            if(m_spawnedenemies.Count > 0)
            {
                for (int i = 0; i < m_spawnedenemies.Count; i++)
                {
                    m_spawnedenemies[i].SetActive(false);
                }
            }

            for (int i = 0; i < PathManager.instance.m_paths.Count; i++)
            {
                m_spawnerTimers.Add(spawnInterval);
            }

            m_spawnedenemies = new List<GameObject>();
            m_spawnedenemies.Clear();
            //ensure we're on the first round for first run
            FirstRound = true;
        }

        /// <summary>
        /// Standard update, operates in accordance to the Game's overall state
        /// </summary>
        private void Update()
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

        #region EnemyManager Behaviours
        private void Tick()
        {
            switch (m_state)
            {
                case EnemyManagerStates.Setup:
                    StartCurrentWave();
                    break;
                case EnemyManagerStates.Spawn:
                    if (StartSpawn)
                    {
                        CheckTimers();
                        //RandomiseDropShip();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Is called once worldGen is completed, this makes worldGen feed
        /// it's list of spawnpoints to the enemy manager, reducing the
        /// longterm cost of calling the value remotely.
        /// </summary>
        /// <param name="_spawnPoints"> the spawn point locations from worldGen </param>
        public void SetSpawnPoints(List<List<WorldTile>> _paths)
        {
            for (int i = 0; i < _paths.Count; i++)
            {
                m_spawnPoints.Add(_paths[i][0].m_worldTileObject.transform.position + new Vector3(0f, 0.15f, 0f));

                m_spawnerTimers.Add(0.0f);
            }
        }

        /// <summary>
        /// Called within Tick when Manager state 
        /// is in Set-Up. Ensure that a new round won't start until 
        /// all enemies are dead
        /// </summary>
        private void StartCurrentWave()
        {
            if (m_spawnedenemies.Count > 0 || FirstRound)
            {
                return;
            }    
            WaveManager.instance.WaveStart();            
        }

        /// <summary>
        /// Count down to init spawn after a player 
        /// has grabbed a gun
        /// </summary>
        /// <returns></returns>
        private IEnumerator IStartCurrentWave()
        {
            powerUpManager.instance.countDown = false;
            //check for first round ~ only tween defend prompt on first round
            if (FirstRound)
            {
                CanvasController.instance.TweenTextIn(CanvasController.instance.DefendPrompt);
            }

            yield return new WaitForSeconds(WaveManager.instance.m_WaveBetweenTime);

            powerUpManager.instance.countDown = true;

            StartSpawn = true;
            CameraShakeHandler.Instance.Shake();
            ArmaPlayerManager.instance.VibratePlayers(4);
            SoundManager.instance.PlayClip(ExplosionSpawn);

            //On first round tween defend text out
            if (FirstRound)
            {
                CanvasController.instance.TweenTextOut(CanvasController.instance.DefendPrompt);
                FirstRound = false;
            }
            //On a new round reset all spawn times
            for (int i = 0; i < m_spawnerTimers.Count; i++)
            {
                m_spawnerTimers[i] = 0.0f;
            }

        }

        /// <summary>
        /// Requests the spawn.
        /// </summary>
        /// <param name="_position">the start position, where the enemies start floating to the ground.</param>
        /// <param name="_dropPos">the final position of the drop</param>
        /// <param name="_paraDrop">If set to <c>true</c> para drop.</param>
        public void RequestSpawn(Vector3 _position, Vector3 _dropPos, bool _paraDrop)
        {
            //should we drop a para
            if (_paraDrop)
            {
                //go through all enemies and find one that is not currently spawned
                for (int i = 0; i < m_enemies.Count; i++)
                {
                    //if the current enemy cant be found in spawned list
                    if (!m_spawnedenemies.Contains(m_enemies[i]))
                    {
                        //move position, feed it the path, and set it active
                        m_enemies[i].transform.position = _position;

                        //Change weapon to requested type
                        EquipWeightedWeapon(m_enemies[i]);

                        m_enemies[i].GetComponent<ArmaEnemy>().dropPoint = _dropPos;

                        m_enemies[i].GetComponent<Actor>().health = 1.0f;
                        m_enemies[i].GetComponent<SpriteRenderer>().color = Color.white;
                        //add to the spawned list
                        m_spawnedenemies.Add(m_enemies[i]);
                        m_enemies[i].SetActive(true);
                        return;
                    }
                }
            }
        }

        private void EquipWeightedWeapon(GameObject enemy)
        {
            if(Random.Range(0f, WaveManager.instance.m_wave) < 2f)
            {
                //enemy.GetComponent<Actor>().EquipWeapon<ArmaMachineGun>();
                enemy.GetComponent<Actor>().EquipWeapon(enemy.GetComponent<Actor>().m_machinegun);
            }
            else if(Random.Range(10f, WaveManager.instance.m_wave) < 4f)
            {
                //enemy.GetComponent<Actor>().EquipWeapon<ArmaLaser>();
                enemy.GetComponent<Actor>().EquipWeapon(enemy.GetComponent<Actor>().m_machinegun);
            }
            else
            {
                //enemy.GetComponent<Actor>().EquipWeapon<ArmaShotgun>();
                enemy.GetComponent<Actor>().EquipWeapon(enemy.GetComponent<Actor>().m_shotgun);
            }
        }

        private void RandomiseDropShip()
        {
            int randVal = Random.Range(0, 100);

            if (randVal == 50)
            {
                //go through all dropships
                for (int i = 0; i < dropShips.Count; i++)
                {
                    //if the current dropship is not active
                    if (!dropShips[i].activeSelf)
                    {
                        dropShips[i].SetActive(true);
                        //initialise the dropship
                        dropShips[i].GetComponent<ArmaDropShip>().Init();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// spawn enemies when spawninterval at a location hits 0
        /// </summary>
        private void CheckTimers()
        {
            //go through list of floats and find one that has hit 0
            for (int i = 0; i < m_spawnerTimers.Count; i++)
            {
                if (m_spawnerTimers[i] <= 0f)
                {
                    //feed the index of the spawner and ATTEMPT to spawn from that location 
                    //SpawnEnemy(i);
                    AddEnemyToSpawnPool(i);
                }
                else
                {
                    m_spawnerTimers[i] -= Time.deltaTime;
                }
            }

            //If there are enemies to be spawned
            if (m_spawnPool.Count > 0)
            {
                //If we have waited at least the required time, spawn the next enemy
                if (Time.time - m_timeLastSpawnedEnemies > m_timeBetweenSpawns)
                {
                    m_timeLastSpawnedEnemies = Time.time;
                    SpawnNextEnemy();
                }
            }
        }

        /// <summary>
        /// Attempts to spawn 
        /// an enemy 
        /// </summary>
        /// <param name="_spawnerIndex"></param>
        private void SpawnEnemy(int _spawnerIndex)
        {
            GameObject _enemy = GetEnemy();
            //spawn selected enemy
            _enemy.GetComponent<ArmaEnemy>().Spawn();
            //Debug.Log(EnemyManager.instance.m_spawnedenemies.Count);
            _enemy.SetActive(true);
            //reset timer
            m_spawnerTimers[_spawnerIndex] = spawnInterval;
        }

        private void SpawnNextEnemy()
        {
            //pop off list
            GameObject _enemy = m_spawnPool[0];
            m_spawnedenemies.Add(_enemy);
            m_spawnPool.RemoveAt(0);
            //spawn selected enemy
            _enemy.GetComponent<ArmaEnemy>().Spawn();
            _enemy.SetActive(true);
        }

        private void AddEnemyToSpawnPool(int _spawnerIndex)
        {
            GameObject _enemy = GetEnemy();
            m_spawnPool.Add(_enemy);
            m_spawnerTimers[_spawnerIndex] = spawnInterval;
        }
        
        private GameObject GetEnemy()
        {
            //check the pool head
            poolHead = poolHead == m_enemies.Count ? 0 : poolHead;
            //attempt to return top of list
            if(!m_spawnedenemies.Contains(m_enemies[poolHead]))
            {
                return m_enemies[poolHead++];
            }
            //safe guard ~ iterate until one is found 
            else
            {
                //go through all enemies and find one that is not currently spawned
                for (int i = 0; i < m_enemies.Count; i++)
                {
                    //if the current enemy cant be found in spawned list
                    if (!m_spawnedenemies.Contains(m_enemies[i]))
                    {
                        return m_enemies[i];
                    }
                }
            }
            return m_enemies[poolHead];//worst case
        }

        /// <summary>
        /// Pauses all enemy animators 
        /// </summary>
        public void Pause()
        {
            for (int i = 0; i < m_spawnedenemies.Count; i++)
            {
                m_spawnedenemies[i].GetComponent<Actor>().m_armaAnimator.enabled = GameManager.instance.state == GameStates.pause ? false : true;
            }
        }
        #endregion
    }
}