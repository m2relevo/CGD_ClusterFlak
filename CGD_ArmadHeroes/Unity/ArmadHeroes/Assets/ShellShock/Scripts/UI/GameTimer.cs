using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace ShellShock
{
    public class GameTimer : MonoBehaviour
    {
        public float roundLength = 60f;
        public float maxRoundLength = 5f;
        public bool isGameRunning = true;
        public Text timerText;
        public GameObject roundOverText;
        public bool isGamePaused;
        public bool countDownFinished = false;
        private int roundCount = 3;
        private int currentRound = 0;
        public GameObject[] players;
        public GameObject[] maps;
        public bool roundOver = false;
        public bool firstRoundStarted = true;
        private bool setMapOnce = true;
        private float lateStartTimer = 0f;
        private float lateStartThreshold = 0.1f;
        private bool callLateStart = false;
        public DynamicWallManager dynamicWallManager;
        public AudioSource roundEndSound;
        public CameraShake cameraShake;
        public bool shouldCameraShake = false;
        public AudioSource backgroundMusic;

        void Start()
        {
            ArmadHeroes.ShellShock_CountDownTimer.instance.CountDown(3, true);
            // ArmadHeroes.CountDownTimer.instance.CountDown(3, true);
            dynamicWallManager = GameObject.FindGameObjectWithTag("ShellShock/DYNAMICMANAGER").GetComponent<DynamicWallManager>();
            roundEndSound = GetComponent<AudioSource>();
            cameraShake = GameObject.FindGameObjectWithTag("ShellShock/CAMERA").GetComponent<CameraShake>();
            backgroundMusic = GameObject.FindGameObjectWithTag("ShellShock/BACKGROUNDMUSIC").GetComponent<AudioSource>();
            currentRound++;
        }

        void LateStart()
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            DisablePlayers();
            SpawnPlayersAtRoundStart();
            dynamicWallManager.CheckDynamicWalls();
        }

        void ProcessLateStart()
        {
            if (!callLateStart)
            {
                lateStartTimer += Time.deltaTime;
                if (lateStartTimer >= lateStartThreshold)
                {
                    lateStartTimer = 0;
                    callLateStart = true;
                    LateStart();
                }
            }
        }

        void DisablePlayers()
        {
            if (players != null)
            {
                for (int i = 0; i < players.Length; i++)
                {

                    players[i].gameObject.GetComponent<RewiredController>().disablePlayerUpdate = true;
                }
            }
        }

        void EnablePlayers()
        {
            if (players != null)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].gameObject.GetComponent<RewiredController>().disablePlayerUpdate = false;
                }
            }
        }

        void SetCurrentMap(int currentRound)
        {
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].SetActive(false);
            }

            maps[currentRound].SetActive(true);
            RespawnManager.Instance.SetNewSpawnPoints();
            dynamicWallManager.CheckDynamicWalls();
            SpawnPlayersAtRoundStart();
        }

        void SpawnPlayersAtRoundStart()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerLogic>().Spawn(RespawnManager.Instance.spawnPoints[i].transform.position);
                players[i].GetComponent<RewiredController>().playerHUD.SetActive(true);
                players[i].GetComponent<PlayerLogic>().ResetPlayerProperties();
            }
        }

        void Update()
        {
            ProcessLateStart();

            if (roundLength <= 0 && isGameRunning && currentRound == roundCount)
            {
              //  Debug.Log("INSIDE END");
                EndGame();
            }
            if (!ArmadHeroes.ShellShock_CountDownTimer.instance.m_countFinished)
            {
               
                if (setMapOnce)
                {
                    SetCurrentMap(currentRound - 1);
                    
                    setMapOnce = false;
                    //Debug.Log(setMapOnce);
                }
                ArmadHeroes.ShellShock_CountDownTimer.instance.UpdateText();
            }
            else
            {
                if (!shouldCameraShake)
                {
                    cameraShake.shakeCamera(0.2f, 0.25f);
                    backgroundMusic.Play();
                    shouldCameraShake = true;
                }
            }
            if (ArmadHeroes.ShellShock_CountDownTimer.instance.m_countFinished)
            {
                roundOver = false;
                if (firstRoundStarted)
                {
                    EnablePlayers();
                    firstRoundStarted = false;
                }

                if (!isGamePaused && isGameRunning)
                {
                    roundLength -= Time.deltaTime;
                    ArmadHeroes.CanvasManager.instance.updateTime(roundLength);
                }
            }

            if (roundLength <= 0 && isGameRunning && !roundOver && currentRound != roundCount)
            {
                // Debug.Log("CALLED");
                DisablePlayers();
                StartCoroutine(WaitAfterRoundFinished(2.0f));
            }
        }

        void EndGame()
        {
            // Debug.Log("GAME ENDED");
            isGameRunning = false;
            timerText.text = "00:00";
            //Application.Quit();

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<RewiredController>().SendData();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("DebriefScene");
        }

        private IEnumerator WaitAfterRoundFinished(float _time)
        {
            roundLength = maxRoundLength;
            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.Find("BallMode").GetComponent<AudioSource>().volume = 0;
                players[i].GetComponent<RewiredController>().playerHUD.SetActive(false);
                // players[i].GetComponent<PlayerLogic>().ResetPlayerProperties();
            }
            roundOverText.SetActive(true);
            backgroundMusic.Stop();
            roundEndSound.Play();
            yield return new WaitForSeconds(2.0f);
            SiloManager.Instance.mSiloList.Clear();
            Helicopter.Instance.ClearDropPoints();
            roundOverText.SetActive(false);
            roundLength = maxRoundLength;
            ArmadHeroes.ShellShock_CountDownTimer.instance.CountDown(3, true);
            shouldCameraShake = false;
            roundOver = true;
            currentRound++;
            firstRoundStarted = true;
            // Debug.Log("CURRENT ROUND:" + currentRound);
            setMapOnce = true;
        }
    }
}
