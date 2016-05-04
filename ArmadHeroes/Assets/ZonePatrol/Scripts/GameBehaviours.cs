using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using ArmadHeroes;

namespace ZonePatrol
{
    public class GameBehaviours : MonoBehaviour
    {
        public float freezeTime = 5.0f;
        public int maxRounds = 2;
        public int currentRound = 0;
        public List<float> roundCountDown;
        public Text countDownText;
        public Text roundText;
        private Color defaultColor;

        public List<AudioClip> roundMusics;
        public AudioClip roundChangeAudio;
        public AudioClip freezeTimeAudio;

        private float timeLeft;
        private float minutes = -1;
        private float seconds = -1;
        public bool stopTimer = true;
        public bool startRound = true;
        public bool roundChange = false;
        IEnumerator beepCoroutine;

        private static GameBehaviours instance;
        public static GameBehaviours getInstance() { return instance; }

        void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            defaultColor = countDownText.color;
            ArmadHeroes.CanvasManager.instance.init();
            DontDestroyOnLoad(this.gameObject);
            freezeTime = freezeTimeAudio.length - 0.2f;
            startTimer(freezeTime, true);
        }

        void OnLevelWasLoaded(int level)
        {
            SoundManager.instance.PlayMusic(roundMusics[currentRound]);
        }

        void startTimer(float time, bool start = false)
        {
            resetTimer();
            timeLeft = time;
            stopTimer = false;
            if (start)
            {
                roundText.text = "Round " + (currentRound + 1);
				SoundManager.instance.PlayClip(freezeTimeAudio, false, 0.5f);
            }
            else
            {
                roundText.text = "";
            }
            roundChange = !start;
            startRound = start;
            Update(); // call update to initialize with correct time value
            StartCoroutine(updateTimer());
        }

        void resetTimer()
        {
            if (beepCoroutine != null)
            {
                StopCoroutine(beepCoroutine);
            }

            minutes = -1;
            seconds = -1;
            stopTimer = true;
            startRound = false;
            roundChange = false;
            countDownText.color = defaultColor;
        }

        // Update is called once per frame
        void Update()
        {
            if (stopTimer)
            {
                return;
            }

            timeLeft -= Time.deltaTime;

            minutes = Mathf.Floor(timeLeft / 60);
            seconds = timeLeft % 60;
            if (seconds > 59)
            {
                seconds = 59;
            }

            if (minutes < 0)
            {
                stopTimer = true;
                minutes = 0;
                seconds = 0;

                if (!startRound)
                {
                    currentRound++;
                    if (currentRound == 1)
                    {
                        PlayerManager.getInstance().savePlayerData();
                        SceneManager.LoadScene("Level2");
                        startTimer(freezeTime, true);
                    }
                    else if (currentRound == 2)
                    {
                        PlayerManager.getInstance().savePlayerData();
                        SceneManager.LoadScene("Level3");
                        startTimer(freezeTime, true);
                    }
                    else if (currentRound == maxRounds)
                    {
                        for (int playerId = 0; playerId < PlayerManager.getInstance().getNumberOfPlayers(); playerId++)
                        {
                            // Send all data to debrief scene
                            PlayerManager.getInstance().getPlayerById(playerId).GetComponent<Player>().sendData();
                        }
                        SceneManager.LoadScene("DebriefScene");
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    startTimer(roundCountDown[currentRound]);
                }
            }

        }
 
        private IEnumerator updateTimer()
        {
            while (!stopTimer)
            {
                if (minutes <= 0 && seconds < 10.5)
                {
                    if (roundChange)
                    {
                        roundChange = false;
                        beepCoroutine = UpdateBeep();
                        StartCoroutine(beepCoroutine);
                    }
                    countDownText.color = Color.red;
                }

                countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator UpdateBeep()
        {
            while (!roundChange)
            {
				SoundManager.instance.PlayClip(roundChangeAudio, false, 0.5f);
                yield return new WaitForSeconds(1.0f);
            }
        }

    }
}