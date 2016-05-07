/// <summary>
/// CountDownTimer.cs
/// Created and implemented by Daniel Weston on 30/04/2016
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ArmadHeroes
{
    public class CountDownTimer : MonoBehaviour
    {
        #region Singleton
        private static CountDownTimer m_instance;
        public static CountDownTimer instance { get { return m_instance; } protected set { m_instance = value; } }
        #endregion

        #region Public Members
        public AudioClip countDownBeep, startBeep;
        public CountDownComplete m_callback;
        public Text m_countDownText;
        public float m_time = 0.0f;
        public bool m_countFinished = false;
        #endregion

        #region Private Members
        bool m_sound = true;
        #endregion

        #region delegate
        public delegate void CountDownComplete();
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            m_countDownText.text = "0";
            instance = this;
        }
        #endregion

        #region CountDownTimer Behaviours
        public void UpdateText()
        {
            m_countDownText.text = m_time.ToString();
            m_countDownText.color = m_time <= 5.0f && Mathf.Floor(m_time) % 2 != 0 ? Color.red : Color.black;//flash text red?
        }

        /// <summary>
        /// Public interface for 
        /// ICountDown, will init a 
        /// timer
        /// </summary>
        /// <param name="_time">The amount of time to count</param>
        public void CountDown(float _time, bool _sound = true)
        {
            m_countFinished = false;
            m_sound = _sound;
            m_time = _time;
            //init count down
            StartCoroutine(ICountDown(_time));
        }

        private IEnumerator ICountDown(float _time)
        {
            //timer
            yield return new WaitForSeconds(1.0f);
            if (_time <= 0)//check for end
            {
                if(m_sound){SoundManager.instance.PlayClip(startBeep);}//play final beep
                if(m_callback!=null)//if callback set
                {
                    m_callback();//launch callback
                }
                m_countFinished = true;//set fin flag
                m_countDownText.color = Color.white;
            }
            else if(_time <= 5.0f)//last 5 secs? play beep
            {
                if (m_sound) { SoundManager.instance.PlayClip(countDownBeep); }                
            }
            if (_time > 0) { --_time; m_time = _time; StartCoroutine(ICountDown(m_time)); }//recursive count down

        }
        #endregion
    }
}
