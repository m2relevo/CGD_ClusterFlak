/// <summary>
/// Tower.cs
/// Created and Implement by Oliver Bourne 23/02/16
/// Updated by Daniel Weston 24/02/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public class Tower : MonoBehaviour
    {
        #region Singleton
        private static Tower m_instance;
        public static Tower instance { get { return m_instance; } }
        #endregion

        #region Public Members
        public SpriteRenderer m_flag; //The flag on the pole
        public float health = 100000f,//Base health
        maxHealth = 100000f,//Max health
        playerMaxHealth = 100.0f,
        healthRange = 3f,
        healthTransferRate = 1f;
        public Vector3 spawnOffsetPosition = Vector3.zero;
        #endregion

        #region Private Members
        private float m_flagTopY = 3.46f; // top of the pole
        private float m_flagBottomY = -4f; // bottom of the pole
        #endregion

        #region Unity Callbacks
        public void Awake()
        {
            m_instance = this;
            UpdateHealth();
        }

		private void Update ()
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

        public void OnTriggerEnter2D(Collider2D col)
        {
            //Check if col is a Projectile
            if (col.gameObject.GetComponent<ProjectileBase>())
            {
                //if so is it an enemy bullet? & the tower has health
                if (col.gameObject.GetComponent<ProjectileBase>().type == ArmadHeroes.ActorType.Enemy && health > 0)
                {
                    //deduct health
                    health -= 1;
                }
                else if (health <= 0)
                {
                    //target random player and destroy
                    ArmaPlayerManager.instance.RemovePlayer(ArmaPlayerManager.instance.m_spawnedPlayers[Random.Range(0, ArmaPlayerManager.instance.SpawnedPlayersCount)]);                        
                }
            }
        }
        #endregion

        #region Tower Behaviours
        private void Tick()
        {
            UpdateHealth();
            //detect any spawned player within "range"
            foreach (ArmaPlayer _playerObject in ArmaPlayerManager.instance.m_spawnedPlayers)
            {
                if (Vector3.Distance(_playerObject.gameObject.transform.position, transform.position) < healthRange)
                {
                    //give that player health if they are not currently maxed
					if (_playerObject.health < playerMaxHealth && health > 40.0f)
                    {
                        //transfer health equal to the health transfer rate
                        _playerObject.GiveHealth(healthTransferRate*Time.deltaTime*40);
                        health -= healthTransferRate*Time.deltaTime*40;
                        
                        //a sanity check to make sure health doesnt go over max
                        if (_playerObject.health > playerMaxHealth)
                        {
                            _playerObject.health = playerMaxHealth;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called during tick 
        /// to ensure UI elements are updated
        /// </summary>
        public void UpdateHealth()
        {
            m_flag.transform.localPosition = new Vector3(m_flag.transform.localPosition.x, Mathf.Lerp(m_flagBottomY, m_flagTopY, health / maxHealth), m_flag.transform.localPosition.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_damage"></param>
        public void TakeDamage(float _damage)
        {
            health -= _damage;
            ArmaPlayerManager.instance.VibratePlayers();
            if(health <= 0)
            {
                GameManager.instance.state = GameStates.gameover;
            }
        }
        #endregion
    }
}