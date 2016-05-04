/// <summary>
/// ArmaClassManager manages all class drops pending on players
/// Created and implemented by Daniel Weston - 17/01/16
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace Armatillery
{
    public class ArmaWeaponManager : MonoBehaviour
    {
        #region Singleton
        private static ArmaWeaponManager m_instance;
        public static ArmaWeaponManager instance { get { return m_instance; } }
        #endregion  

        #region Public Members
        public List<GameObject> m_Weapons = new List<GameObject>();
        public Vector3 offset;
        [HideInInspector]
        public List<GameObject> m_SpawnedWeapons = new List<GameObject>();
        #endregion

        #region Private Members
        private bool m_classesSpawned = false;
        #endregion

        #region Unity Callbacks 
        void Awake()
        {
            m_instance = this;//set up singleton
        }
        
        void Update()
        {
            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    if (!m_classesSpawned)
                    {
                        Init();
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
        #endregion

        #region ArmaWeaponManager Behaviours
        /// <summary>
        /// Init Should be called after the players
        /// have spawned. Init will spawn X amount of
        /// classes; x being the number of spawned players
        /// </summary>
        void Init()
        {
            //this.transform.position = Vector2.zero;
            Vector3 _spawn = Vector3.zero;
            GameObject _class;
            int _classToSpawn = 0;
            ////until spawned classes matches spawned players

            for (int i = 0; i < ArmaPlayerManager.instance.m_spawnedPlayers.Count; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                float x = Mathf.Cos(angle * 5) * Random.Range(3, 5f);
                float y = Mathf.Sin(angle * 5) * Random.Range(1.75f, 3f);

                _spawn = new Vector3(x, y,0);
                _spawn += Tower.instance.transform.position + Tower.instance.spawnOffsetPosition;
                foreach (ArmaPlayer g in ArmaPlayerManager.instance.m_spawnedPlayers)
                {
                    if (Vector3.Distance(_spawn, g.gameObject.transform.position) < 1.5f)
                    {
                        _spawn += g.gameObject.transform.position.normalized*3;
                    }
                }

                //_spawn = new Vector2(Random.Range(-100 * 0.05f, 100 * 0.05f),Random.Range(-100 * 0.05f, 100 * 0.05f));//quick fix/create new pos for pickup


                //_spawn = new Vector2((_spawn.x * WorldGenerator.instance.m_tileHeight) - ((WorldGenerator.instance.m_mapWidth *
                //    WorldGenerator.instance.m_tileHeight) / 2f), (_spawn.y * WorldGenerator.instance.m_tileHeight) -
                //    ((WorldGenerator.instance.m_mapHeight * WorldGenerator.instance.m_tileHeight) / 2f));

                _classToSpawn = Random.Range(0, m_Weapons.Count);//get a random class
                _class = GameObject.Instantiate(m_Weapons[_classToSpawn], _spawn, Quaternion.identity) as GameObject;
                m_SpawnedWeapons.Add(_class);//spawn and add random class
                //m_SpawnedClasses[i].transform.parent = this.transform;//parent transform
            }

            m_classesSpawned = !m_classesSpawned;//toggle bool as not to Init every Update!
        }

        /// <summary>
        /// Reset needs to be called
        /// when the Game state changes from 
        /// GameOver back to game
        /// </summary>
        public void Reset()
        {
            foreach (GameObject weapon in m_SpawnedWeapons)
            {
                weapon.SetActive(true);
            }
        }
        #endregion
    }
}