using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DilloDash
{
    public class GameControllerDD : MonoBehaviour
    {
        public bool debugMode = true;
        public bool splitControllerMode = false;
        [Range(1, 4)]
        public int testPlayers = 2;

        private static GameControllerDD singleton;
        public static GameControllerDD Singleton() { return singleton; }

        [SerializeField]
        private TrackNode startLine = null;

        [SerializeField]
        private Camera followCam = null;
        [SerializeField]
        [Range(0, 1)]
        private float camWeighting = 0.75f;   //The camera tracks the better placed players with a 75% weighted average
        private Vector3 newCamPos;
        [SerializeField]
        private float camSpeed = 10.0f;

        [SerializeField]
        private Timer delayTimer = null;
        private bool resetting = true;
        [SerializeField]
        private float gameTime = 3.0f;
        private Timer gameTimer = null;

        [SerializeField]
        Text startText = null;
        private bool firstLoad = true;
        [SerializeField]
        GameObject pauseCanvas;

        void Awake()
        {
            singleton = this;
            gameTimer = new Timer();
            gameTimer.SetupTimer(ArmadHeroes.CanvasManager.instance.Timer, gameTime, true);
        }

        void Update()
        {
            startText.text = Mathf.FloorToInt(delayTimer.currentTime) == 0 ? "GO" : Mathf.FloorToInt(delayTimer.currentTime).ToString();
        }

        void FixedUpdate()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                delayTimer.Update();
                //If in the process of resetting race
                if (resetting)
                {
                    //When reset timer has finished let the players start
                    if (delayTimer.HasTimerFinished())
                    {
                        resetting = false;
                        PlayerManagerDD.Singleton().UnpausePlayers();
                        PlayerManagerDD.Singleton().UpdatePlayers();
                        PlayerManagerDD.Singleton().SortPlayersByPosition();
                        VehicleManager.Singleton().setVehiclesActive(true);
                    }
                    PlayerManagerDD.Singleton().UpdatePlayerLanding(Mathf.Max(0.0f,delayTimer.GetTime() - 1.5f), 3.0f);
                }
                //While not resetting keep updating and track of the players
                else
                {
                    gameTimer.Update();
                    PlayerManagerDD.Singleton().UpdatePlayers();
                    PlayerManagerDD.Singleton().SortPlayersByPosition();
                    PlayerManagerDD.Singleton().UpdateAerialPlayers();
                    LerpCamera();
                    PlayerManagerDD.Singleton().CheckPlayersOnScreen(followCam);
                }
                //After everything else check if 1 or less players remaining to determine the reset           
                if (PlayerManagerDD.Singleton().GetNumberAlivePlayers() <= 1)
                {
                    if (debugMode && PlayerManagerDD.Singleton().GetNumberAlivePlayers() != 0)
                    {
                        return;
                    }

                    //Player win animation of some form
                    //Reset everything
                    //null here means it will use the leading track node in the player manager
                    PlayerManagerDD.Singleton().DetermineWinner();
                    PlayerManagerDD.Singleton().ResetPlayers(null);
                    PlayerManagerDD.Singleton().SortPlayersByPosition();
                    PlayerManagerDD.Singleton().PositionPlayers(null);
                    PlayerManagerDD.Singleton().MoveCameraToStart(followCam);
                    VehicleManager.Singleton().setVehiclesActive(false);
                    delayTimer.InitialiseTimer();
                    InitCountdown();
                    resetting = true;
                }

            }
        }

        void EndGame()
        {
            PlayerManagerDD.Singleton().SendPlayerData();
            UnityEngine.SceneManagement.SceneManager.LoadScene("DebriefScene");
        }

        public Camera GetCamera() { return followCam; }

        //Move camera based on players positions and position data
        void LerpCamera()
        {
            newCamPos = WeightCameraPosition();
            Vector3 _camPos = followCam.transform.position;
            float catchSpeed = Vector3.SqrMagnitude(newCamPos - _camPos) / 10000;
            followCam.transform.position = Vector3.Lerp(_camPos, newCamPos, catchSpeed * Time.deltaTime * camSpeed);
        }

        //Instantly move camera to new position
        void MoveCamera()
        {
            newCamPos = WeightCameraPosition();
            followCam.transform.position = newCamPos;
        }

        //Gets the new position of the camera considering the place of each other player
        Vector3 WeightCameraPosition()
        {
            Vector3 _weightedPos = Vector3.zero;
            float _weight = 1.0f;
            int _i = 0;

            int _alivePlayers = PlayerManagerDD.Singleton().GetNumberAlivePlayers();

            //Get the positions of all alive players
            Vector3[] positions = PlayerManagerDD.Singleton().GetAllPlayersPositions();
            //Calculate weight for every player but last place
            for (; _i < _alivePlayers - 1; ++_i)
            {
                //multiply player position by current weighting and add to total weighted position
                _weightedPos += positions[_i] * (_weight * camWeighting);
                //Set the weighting for the next object
                _weight = _weight - (_weight * camWeighting);
            }
            //Add final weighting of left over player
            _weightedPos += positions[_i] * _weight;
            //Set z position and set camera position
            _weightedPos.z = -10.0f;
            return _weightedPos;
        }

        //When the first game begins
        public void Init()
        {
            PlayerManagerDD.Singleton().PositionPlayers(startLine);
            delayTimer.InitialiseTimer();

            InitCountdown();

            gameTimer.AddFunctionCall(EndGame);
            gameTimer.StartTimer();

            
            resetting = true;
            PlayerManagerDD.Singleton().MoveCameraToStart(followCam, startLine);
            ArmadHeroes.CanvasManager.instance.init();
        }

        void InitCountdown()
        {
            startText.enabled = true;
            delayTimer.currentTime = 4.0f;
            delayTimer.AddFunctionCall(RemoveCountdown);
            delayTimer.StartTimer();
        }

        void RemoveCountdown()
        {
            startText.enabled = false;
            gameTimer.AddFunctionCall(EndGame);
            gameTimer.StartTimer();
            pauseCanvas.SetActive(true);

        }


        public TrackNode GetStartLine() { return startLine; }
    }


}
