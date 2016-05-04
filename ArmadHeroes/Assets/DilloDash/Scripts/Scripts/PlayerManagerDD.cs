using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace DilloDash
{
    public class PlayerManagerDD : MonoBehaviour
    {
        //singleton
        private static PlayerManagerDD singleton;
        public static PlayerManagerDD Singleton() { return singleton; }

        // player prefab to spawn
        public GameObject playerPrefab;
        public int alivePlayers;   //The amount of current alive players

        // lists
        private List<DilloDashPlayer> players = null;

        // Starting node (for spawning)
        private TrackNode leadTrackNode = null;

        #region monobehaviour

        void Awake()
        {
            //initialise
            singleton = this;
            players = new List<DilloDashPlayer>();
            alivePlayers = 0;
        }

        // Use this for initialization
        void Start()
        {
            // define list
        }

        // Update is called once per frame
        void Update()
        {
            // locate the player's tile and apply appropriate effect
            locatePlayerOnMap();
        }
        #endregion

        #region publicAPI

        // update the players checkpoint progress
        public void UpdatePlayers()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].UpdateTrackPercent();
            }
        }

        // sort the list of players in order of progress
        public void SortPlayersByPosition()
        {
            int _numPlayers = players.Count;
            //Bubble sort players 
            for (int i = 0; i < _numPlayers - 1; ++i)
            {
                for (int j = 0; j < _numPlayers - 1 - i; ++j)
                {
                    float left = players[j].GetTrackProgress();
                    float right = players[j + 1].GetTrackProgress();

                    if (left < right)
                    {
                        DilloDashPlayer temp = players[j];
                        players[j] = players[j + 1];
                        players[j + 1] = temp;
                    }
                }
            }
            leadTrackNode = players[0].GetPrevNode();
        }

        public Vector3[] GetAllPlayersPositions()
        {
            //Get the positions of all alive players
            Vector3[] positions = new Vector3[alivePlayers];
            for (int i = 0; i < alivePlayers; ++i)
            {
                positions[i] = players[i].transform.position;
            }
            return positions;
        }

        public void CheckPlayersOnScreen(Camera _cam)
        {
            //For every player other than first place
            for (int i = alivePlayers - 1; i != 0; --i)
            {
                if (!players[i].GetIsDead())
                {
                    //Get position from world space on viewport
                    Vector3 screenPoint = _cam.WorldToViewportPoint(players[i].transform.position);
                    bool onScreen = false;
                    if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                    {
                        onScreen = true;
                    }
                    if (!onScreen)
                    {
                        KillPlayer(i);
                    }
                }
            }
        }

        public void KillPlayer(int _player)
        {
            players[_player].Kill();
            --alivePlayers;
            if (alivePlayers == 1)
            {
                //Give that player a winning point
            }
        }

        public void MoveCameraToStart(Camera _cam, TrackNode _node = null)
        {
            if(_node != null)
            {
                _cam.transform.position = _node.spawnPivot.transform.position + (Vector3.back * 10.0f);
            }
            else
            {
                _cam.transform.position = leadTrackNode.GetLastSpawnableNode().spawnPivot.transform.position + (Vector3.back * 10.0f);
            }
        }

        //Will position all players along a tracknode
        public void PositionPlayers(TrackNode _node)
        {
            if (!_node)
            {
                _node = leadTrackNode.GetLastSpawnableNode();
            }
            Vector3 _pivot = _node.GetSpawnPivot();
            //Calculations that appropriately position players
            int rows = Mathf.CeilToInt((float)players.Count / 4.0f);
            Vector3 _right; 

            for (int i = 0; i < players.Count; ++i)
            {
                _right = (_node.GetRightPoint() - _node.GetLeftPoint()).normalized;
                if (rows == 1)
                {
                    int _columns = ((players.Count - 1) % 4) + 1;
                    float _column = -((float)(_columns - 1) / 2.0f) + (i % 4);
                    _right *= _column * 6.0f;
                }
                else
                {
                    Vector3 nodeForward = Vector3.Cross(_node.GetRightPoint() - _node.GetLeftPoint(), Vector3.forward).normalized;
                    if (i < 4)
                    {
                        int _columns = 4;
                        float _column = -((float)(_columns - 1) / 2.0f) + (i % 4);
                        _right *= _column * 6.0f;
                        _right += nodeForward * 3.0f;
                    }
                    else
                    {
                        int _columns = ((players.Count - 1) % 4) + 1;
                        float _column = -((float)(_columns - 1) / 2.0f) + (i % 4);
                        _right *= _column * 6.0f;
                        _right -= nodeForward * 3.0f;
                    }
                }
               
                players[i].transform.eulerAngles = _node.transform.eulerAngles;
                players[i].SetLandingTarget(_pivot + _right);
                players[i].SetPrevNode(_node);        
            }
        }

        public void UpdatePlayerLanding(float _time, float _duration)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].UpdateLandingTarget(_time, _duration);
            }
        }

        //Set all players to alive, reset the vel and acc, move position to node
        public void ResetPlayers(TrackNode _node)
        {
            if (!_node)
            {
                _node = leadTrackNode;
            }
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].Reset(_node);
            }
            alivePlayers = players.Count;
        }

        public void UnpausePlayers()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].SetPaused(false);
            }
        }

        //Updates the position aerial players should head towards
        public void UpdateAerialPlayers()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].GetIsDead())
                {
                    players[i].UpdateAerialPlayer(players[0].transform.position);
                }
            }
        }

        //Adds a single new player to the player data and setup the default values
        public void NewPlayer(int _playerNum, int _controllerNum)
        {
            GameObject _object = Instantiate(playerPrefab) as GameObject;
            players.Add(_object.GetComponent<DilloDashPlayer>());
            _object.GetComponent<DilloDashPlayer>().Init(_playerNum, _controllerNum, "");
            ++alivePlayers;
        }

        public void NewPlayer(int _index)
        {
            if (!GlobalPlayerManager.instance.GetPlayerData(_index).activePlayer)
            {
                return;
            }
            GameObject _object = Instantiate(playerPrefab) as GameObject;
            players.Add(_object.GetComponent<DilloDashPlayer>());
            string _name = CharacterProfiles.instance.TypeToString(GlobalPlayerManager.instance.GetPlayerData(_index).character);
            _object.GetComponent<DilloDashPlayer>().Init(_index, GlobalPlayerManager.instance.GetPlayerData(_index).controllerIndex, _name);
            //Setup wihch animator to use here next
            ++alivePlayers;
        }

        public void DetermineWinner()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (!players[i].GetIsDead())
                {
                    players[i].Win();
                }
            }
        }

        public void SendPlayerData()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].SendData();
            }
        }

        public int GetNumberAlivePlayers()
        {
            return alivePlayers;
        }

        public int GetNumberPlayers()
        {
            return players.Count;
        }

        public TrackNode GetLeadTrackNode() { return leadTrackNode; }

        #endregion

        #region player-level interaction

        void locatePlayerOnMap()
        {
            // iterate through the players
            for (int i = 0; i < players.Count; ++i)
            {
                if (!players[i].GetIsDead())
                {
                    Vector2 pos = new Vector2(players[i].transform.position.x, players[i].transform.position.y);

                    // call the function with all the effects
                    ChooseColourEffect(i, TileManager.Singleton().whatTileEffect(pos), pos);
                }
            }
        }

        void ChooseColourEffect(int _playerInt, TileEffect _colourInt, Vector2 pos)
        {
            switch (_colourInt)
            {
                case TileEffect.DEFAULT: // default - do nothing
                    ;
                    break;
                case TileEffect.TRACK: // track - do nothing
                    trackPlayer(_playerInt);                   
                    break;
                case TileEffect.SAND: // off track - slow player
                    slowPlayer(_playerInt);
                    break;
                case TileEffect.BOOST: // boost - speed player
                    speedPlayer(_playerInt);
                    break;
                case TileEffect.SLIP:
                    slipPlayer(_playerInt);
                    break;
                case TileEffect.POWERUP:
                    //powerUp(_playerInt, pos);
                    break;
                case TileEffect.TARMAC:
                    break;
            }
        }

        //This function is pointless, all it does is call another function...
        void KillColourEffect(int _playerNo)
        {
            KillPlayer(_playerNo);            
        }

        void slowPlayer(int _playerNo)
        {
            players[_playerNo].setSlowTileMod();           
        }

        void speedPlayer(int _playerNo)
        {
            players[_playerNo].setSpeedTileMod();           
        }

        void slipPlayer(int _playerNo)
        {
            players[_playerNo].setSlipTileOn();        
        }

        void trackPlayer(int _playeNo)
        {
            players[_playeNo].setTrackTile();
        }
        #endregion


    }
}
