/// <summary>
/// WaveManager.cs
/// Created and implemented by Daniel Weston on 02/03/2016
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using Armatillery;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public enum WaveState
    {
        startGame,
        startWave,
        inWave,
        endWave
    };

    public class WaveManager : MonoBehaviour
    {
        #region Singleton
        private static WaveManager m_instance;
        public static WaveManager instance { get { return m_instance; } }
        #endregion

        #region Public Memebrs
        public int m_wave = 0;
        [HideInInspector]
        public float m_CurrentWaveTime = 0.0f;
        public float m_WaveEndTime = 10.0f,
            m_WaveBetweenTime = 5.0f;
        public Text UIwave;
        #endregion

        #region Private Members
        private float m_startingWaveEndTime,
            m_startingWaveBetweenTime;
        #endregion

        #region WaveManager State Machine
        private WaveState m_state;
        public WaveState state { get { return m_state; } set { m_state = value; ChangeState(value); } }
        /// <summary>
        /// Pending on the passed state 
        /// carry out a specific action
        /// </summary>
        /// <param name="_state"></param>
        private void ChangeState(WaveState _state)
        {

            switch (_state)
            {
                case WaveState.startGame:
                    GameStart();
                    break;
                case WaveState.startWave:
                    //WaveStart();
                    break;
                case WaveState.inWave:
                    //m_CurrentWaveTime = m_WaveEndTime;//start wave with desired wave time
                    break;
                case WaveState.endWave:
                    //WaveEnd();
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            m_instance = this;
            m_startingWaveEndTime = m_WaveEndTime;
            m_startingWaveBetweenTime = m_WaveBetweenTime;
        }
        
        void Update()
        {
            switch (state)
            {
                case WaveState.startGame:
                    break;
                case WaveState.startWave:
                    CountDownTimer.instance.UpdateText();
                    break;
                case WaveState.inWave:
                    CountDownTimer.instance.UpdateText();
                    break;
                case WaveState.endWave:
                    CountDownTimer.instance.UpdateText();
                    break;
                default:
                    break;
            }
        }
        #endregion

        public void Init()
        {
            //make sure vars are 0
            state = WaveState.startGame;
        }

        #region WaveManager Behaviours
        /// <summary>
        /// When the game begins check all players
        /// for weapons
        /// </summary>
        private void GameStart()
        {
            m_wave = 0;
            m_CurrentWaveTime = 0.0f;
            CountDownTimer.instance.m_countDownText.text = "00";
            m_WaveEndTime = m_startingWaveEndTime;
            m_WaveBetweenTime = m_startingWaveBetweenTime;
        }

        /// <summary>
        /// Function called when 
        /// WaveState is changed to 
        /// WaveStart
        /// </summary>
        public void WaveStart()
        {
            if (state != WaveState.startWave)
            {
                Invoke("updateUIwaveCount", 6);
                state = WaveState.startWave;
                powerUpManager.instance.inWave = true;
                //start spawning enemies
                EnemyManager.instance.state = EnemyManagerStates.Spawn;
                m_CurrentWaveTime = m_WaveBetweenTime;//m_WaveEndTime;//reset timer
                CountDownTimer.instance.m_countDownText.text = m_CurrentWaveTime.ToString();

                ArmadHeroes.CountDownTimer.instance.m_callback = InWave;
                ArmadHeroes.CountDownTimer.instance.CountDown(5);
                ArmaPlayerManager.instance.ReviveAllPlayers();
            }
        }

        void updateUIwaveCount()
        {
                UIwave.text = "Wave  " + (m_wave+1);
        }

        private void InWave()
        {
            if (state != WaveState.inWave)
            {
                m_CurrentWaveTime = m_WaveEndTime;

                ArmadHeroes.CountDownTimer.instance.m_callback = WaveEnd;
                ArmadHeroes.CountDownTimer.instance.CountDown(m_CurrentWaveTime, false);
            }
        }

        /// <summary>
        /// Function called when 
        /// WaveState is changed to 
        /// WaveEnd
        /// </summary>
        private void WaveEnd()
        {
            if(state != WaveState.endWave)
            {
                state = WaveState.endWave;

                powerUpManager.instance.inWave = false;
                if (EnemyManager.instance.m_spawnedenemies.Count > 0)
                {
                    CanvasController.instance.TweenTextInOut(CanvasController.instance.FinishText, 3.0f);
                }
                EnemyManager.instance.state = EnemyManagerStates.Setup;
                ++m_wave;//increment wave
                ++m_WaveEndTime;//increment wave time
            }
        }
        #endregion
    }//end class
}//end namespace