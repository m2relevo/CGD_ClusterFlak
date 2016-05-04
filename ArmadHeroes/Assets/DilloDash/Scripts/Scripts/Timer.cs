using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace DilloDash
{
    [System.Serializable]
    public class Timer
    {
        [SerializeField] private Text timerText = null;
        [SerializeField][Range(0, 600)] private float duration = 5.0f; //duration of timer in seconds
        [SerializeField] private bool countDown = true;
        public float currentTime;
        private bool timerPaused = true;
        private bool timerFinished = true;

        public delegate void TimerFinishedCall();
        private TimerFinishedCall timerFinishedCall = null;

        public void QuickTimer(float _duration, TimerFinishedCall _call)
        {
            duration = _duration;
            timerFinishedCall = _call;
            InitialiseTimer();
            StartTimer();
        }

        public void SetupTimer(Text _text, float _duration, bool _countDown)
        {
            timerText = _text;
            duration = _duration;
            countDown = _countDown;
            InitialiseTimer();
        }

        //Can pass through a void function that will be called automatically on timer completion
        public void AddFunctionCall(TimerFinishedCall _call)
        {
            timerFinishedCall = _call;
        }

        // Update is called once per frame
        public void Update()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                if (!timerPaused)
                {
                    //Counting up or down the timer
                    if (countDown)
                    {
                        currentTime -= Time.deltaTime;
                    }
                    else
                    {
                        currentTime += Time.deltaTime;
                    }
                    //Display text after the timer alteration
                    DisplayText();
                    //Test whether the timer has finished and provide appropriate functionality
                    if (!timerFinished)
                    {
                        if (countDown)
                        {
                            if (!(currentTime > 0.0f))
                            {
                                currentTime = 0.0f;
                                DisplayText();
                                StopTimer();
                                timerFinished = true;
                                if (timerFinishedCall != null)
                                {
                                    timerFinishedCall();
                                }
                            }
                        }
                        else
                        {
                            if (!(currentTime < duration))
                            {
                                currentTime = duration;
                                DisplayText();
                                StopTimer();
                                timerFinished = true;
                                if (timerFinishedCall != null)
                                {
                                    timerFinishedCall();
                                }
                            }
                        }
                    }
                }
            }
        }

        //Set up the starting time - Has to be called before start timer
        public void InitialiseTimer()
        {
            //Offset the starting time slightly to compensate for the rounding
            timerFinished = false;
            StopTimer();
            if (countDown)
            {
                currentTime = duration;
            }
            else
            {
                currentTime = 0.0f;
            }
            DisplayText();
        }

        public void BeginTimer()
        {
            InitialiseTimer();
            StartTimer();
        }

        //Initialise and begin the timer
        public void StartTimer()
        {
            timerPaused = false;
        }

        //Stops the timer
        public void StopTimer()
        {
            timerPaused = true;
        }

        public float GetTime()
        {
            return currentTime;
        }

        //Instantly set time to a certain value regardless on inspector parameters
        public void SetTime(int _seconds)
        {
            if (_seconds < 0)
            {
                _seconds = 0;
            }
            else if (_seconds > 600)
            {
                _seconds = 600;
            }
            currentTime = _seconds;
            DisplayText();
        }

        public bool HasTimerFinished()
        {
            return timerFinished;
        }

        //Determines the text output for the current time
        string TimeToDisplay()
        {
            //If counting down display an offset due to rounding down
            float time = currentTime;
            if (countDown)
            {
                time += 0.999f;
            }

            //Setup to display the minutes
            int minutes = (int)Mathf.Floor(time / 60.0f);
            string display;
            if (minutes > 9)
            {
                display = minutes.ToString() + ":";
            }
            else
            {
                display = "0" + minutes.ToString() + ":";
            }
            //Setup to display the seconds
            int seconds = Mathf.FloorToInt((time) % 60.0f);
            if (seconds > 9)
            {
                display += seconds.ToString();
            }
            else
            {
                display += "0" + seconds.ToString();
            }
            return display;
        }

        void DisplayText()
        {
            if (timerText)
            {
                timerText.text = TimeToDisplay();
            }
        }
    }
}
