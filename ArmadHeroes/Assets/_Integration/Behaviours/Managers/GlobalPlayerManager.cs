/// <summary>
/// Created by Julian Stopher and Alan Parsons
/// Edited by Shaun Landy
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArmadHeroes
{
    //Struct containing all details relating to a player
    [System.Serializable]
    public struct PlayerData
    {
        public CharacterType character;
        public int controllerIndex;
        public bool activePlayer;
        //May be good idea to assign player number here before pushed to peoples scenes
        //Any other information that has to stick across screens per player

		/// 
		/// Added by Peter Maloney 
		/// 
		//Score reprsents chevrons earnt
		public int overallScore;
        public int gameScore;
        //Array 
        public AccoladeValues[] accolades;
        
		//public KeyValuePair<string, string>[] accolades;
		///
		///

    }

    public class GlobalPlayerManager : MonoBehaviour
    {
        static private GlobalPlayerManager m_instance = null;
        static public GlobalPlayerManager instance { get { return m_instance; } }

        public PlayerData[] playerData = new PlayerData[8];

        public AccoladeEnum currentGameAccolade;
        public string  currentMission = string.Empty;

        public void SetGameAccolade(AccoladeEnum _accolade)
        {
            currentGameAccolade = _accolade;
        }

        public void SetCurrentMission(string _mission)
        {
            currentMission = _mission;
        }

        public void SetDebriefStats(int _playerID, int _score, float _sponge, float _camper, float _runner, float _rambo, float _unique)
        {
            playerData[_playerID].gameScore = _score;
            playerData[_playerID].accolades[(int)AccoladeEnum.BULLETSPONGE].lastValue = _sponge;
            playerData[_playerID].accolades[(int)AccoladeEnum.CAMPER].lastValue = _camper;
            playerData[_playerID].accolades[(int)AccoladeEnum.ROADRUNNER].lastValue = _runner;
            playerData[_playerID].accolades[(int)AccoladeEnum.RAMBO].lastValue = _rambo;
            playerData[_playerID].accolades[(int)currentGameAccolade].lastValue = _unique;
        }

        
        // Use this for initialization
        void Awake()
        {
            if (!m_instance)
            {
                m_instance = this;
            }
            for(int i = 0; i < 8; ++i)
            {
                playerData[i].accolades = new AccoladeValues[(int)AccoladeEnum.MAX_ACCOLADES];
                for (int j = 0; j < (int)AccoladeEnum.MAX_ACCOLADES; ++j)
                {
                    playerData[i].accolades[j].accoladesPositions = new List<Vector3>();
                }

            }
        }
        public void SetCharacter(int _player, int _character)
        {
            playerData[_player].character = (CharacterType)_character;
        }

        public CharacterType ChangeCharacter(int _player, int _change)
        {
            CharacterType _newCharacter = playerData[_player].character + _change;
            if(_newCharacter >= CharacterType.MAX_TYPES)
            {
                _newCharacter = 0;
            }
            else if(_newCharacter < 0)
            {
                _newCharacter = CharacterType.MAX_TYPES - 1;
            }
            playerData[_player].character = _newCharacter;
            return playerData[_player].character;
        }

        public void SetActive(int _player, bool _active)
        {
            playerData[_player].activePlayer = _active;
        }
        public void SetControllerIndex(int _player, int _index)
        {
            playerData[_player].controllerIndex = _index;
        }

        public PlayerData GetPlayerData(int _index)
        {
            return playerData[_index];
        }

		public void Reset()
        {
            currentMission = string.Empty;
            for(int i = 0; i < playerData.Length; ++i)
            {
                playerData[i].overallScore = 0;
                playerData[i].character = CharacterType.SARGE;
                for (int j = 0; j < playerData[i].accolades.Length; ++j)
                {
                    playerData[i].accolades[j].accoladesPositions.Clear();
                }
            }
        }
    }
}
