/// <summary>
/// ArmaPlayerManager manages all ArmaPlayers
/// Created and implemented by Daniel Weston - 02/01/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace Armatillery
{
    public class ArmaPlayerManager : MonoBehaviour
    {
        #region Singleton
        private static ArmaPlayerManager m_instance = null;
        public static ArmaPlayerManager instance { get { return m_instance; } }
        #endregion

        #region Public Members
        [HideInInspector]
        public List<ArmaPlayer> m_spawnedPlayers = new List<ArmaPlayer>(); //list of players spawned in game
        public List<ArmaPlayer> m_players = new List<ArmaPlayer>(); //list of pooled players
        public int SpawnedPlayersCount { get { return m_spawnedPlayers.Count; } }
        [HideInInspector]
        public int ConnectedPlayers = 0;//this is the amount of connected players by the amount of connected controllers - not the amount of spawned players
        public int ArmedPlayers
        {
            get { return m_ArmedPlayers; }
            set
            {
                m_ArmedPlayers = value;
                if (m_ArmedPlayers == SpawnedPlayersCount)
                {
                    CanvasController.instance.TweenTextOut(CanvasController.instance.PickUpPrompt);
                    WaveManager.instance.WaveStart();
                    
                }
            }
        } //Counter of armed players
        #endregion

        #region Private Members
        private int m_ArmedPlayers = 0;
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            //set up singleton
            m_instance = this;
        }

        void Start()
        {
            Init();
        }
        #endregion

        #region ArmaPlayerManager Behaviours
        public void VibratePlayers(float _strength = 1.0f, float _time = 0.1f)
        {
            foreach (ArmaPlayer _player in m_spawnedPlayers)
            {
                _player.controller.StartVibration(_strength, _time);
            }
        }

        /// <summary>
        /// Initializer for 
        /// PlayerManager
        /// </summary>
        public void Init()
        {
            m_ArmedPlayers = 0;
            CanvasManager.instance.init();
            ControllerSetUp();
            InitPlayers();
        }

        /// <summary>
        /// Used to remove players from 
        /// list of spawned players, ensures 
        /// that when no players are left 
        /// set game to gameover
        /// </summary>
        /// <param name="_player"></param>
        public void RemovePlayer(ArmaPlayer _player)
        {
            _player.gameObject.SetActive(false);
            m_spawnedPlayers.Remove(_player);
            if (m_spawnedPlayers.Count == 0)
            {
                StopAllCoroutines();
                GameManager.instance.state = GameStates.gameover;
            }
        }


        public void ReviveAllPlayers()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].spawnedAtStart)
                {
                    bool isAlive = false;
                    for (int x = 0; x < m_spawnedPlayers.Count; x++)
                    {
                        if (m_spawnedPlayers[x] == m_players[i])
                        {
                            isAlive = true;
                            break;
                        }
                    }
                    if (!isAlive)
                    {
                        m_players[i].gameObject.SetActive(true);
                        m_players[i].Respawn();
                        m_spawnedPlayers.Add(m_players[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the connected joystick amount 
        /// and calls controller setup, using joypads
        /// or keyboard pending on connected devices
        /// </summary>
        public void InitPlayers()
        {
            foreach (ArmaPlayer player in m_spawnedPlayers)
            {
                player.Reset();
            }
        }

        /// <summary>
        /// Iterates over the number of connected devices (*2)(padsplitting)
        /// and sets up players with correct player and controller id's
        /// </summary>
        /// <param name="m_type"></param>
        private void ControllerSetUp()
        {
            if (GlobalPlayerManager.instance)
            {
                for (int i = 0; i < GlobalPlayerManager.instance.playerData.Length; i++)
                {
                    if (GlobalPlayerManager.instance.playerData[i].activePlayer)
                    {
                        SetupPlayer(i,
                        GlobalPlayerManager.instance.playerData[i].controllerIndex,
                        CharacterProfiles.instance.GetProfile(GlobalPlayerManager.instance.playerData[i].character).characterName);
                    }
                }//end for
            }//end if
            if(m_spawnedPlayers.Count <= 0)
            {
                for (int i = 0; i < ControllerManager.instance.controllerCount; i++)
                {
                    SetupPlayer(i,i, "sarge");
                }//end for
            }//end else
            powerUpManager.instance.Init();
        }

        /// <summary>
        /// Sets up player Actor
        /// </summary>
        /// <param name="_playerID"></param>
        /// <param name="_controllerID"></param>
        /// <param name="name"></param>
        private void SetupPlayer(int _playerID, int _controllerID, string _name)
        {
            //turn on a new player
            ArmaPlayer _player =  m_players[_playerID].GetComponent<ArmaPlayer>();
            _player.Init(_controllerID, _playerID, _name);
            m_spawnedPlayers.Add(m_players[_playerID]);
        }
        #endregion
    }
}