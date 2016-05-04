/// <summary>
/// ArmadHeroes_Pause.cs
/// Created and Implemented by Daniel Weston 27/04/2016
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace ArmadHeroes
{
    public class ArmadHeroes_Pause : MonoBehaviour
    {
        #region State
        private enum PauseState
        {
            paused,
            unpaused
        };
        private PauseState m_state;
        private PauseState state {get{return m_state;} set{ChangeState(value);}}
        void ChangeState(PauseState _state)
        {
            switch (_state)
            {
                case PauseState.paused:
                    //m_pause.SetActive(true);
                    m_animator.SetBool("isPaused", true);//trigger animation
                    m_audioPanel.SetActive(false);//turn off audio panel
                    m_mainPanel.SetActive(true);//turn on main
                    m_currentMenu = m_MainMenuOptions;//switch to main menu options
                    menuIndex = 0;//reset index
                    CurrentWindowState = WindowStates.MainMenu;//set tick state to main
                    break;
                case PauseState.unpaused:
                    //m_pause.SetActive(false);
                    m_animator.SetBool("isPaused", false);
                    break;
                default:
                    break;
            }
            m_state = _state;
        }
        #endregion

        #region Window States
        public enum WindowStates
        {
            MainMenu,
            AudioMenu
        };
        public WindowStates CurrentWindowState = WindowStates.MainMenu;
        #endregion

        #region Main Menu Options
        public enum MainMenu
        {
            Continue,
            Options,
            Exit
        };
        public MainMenu MainMenuOption = MainMenu.Continue;
        #endregion

        #region Audio Menu Options
        public enum AudioMenu
        {
            BGM,
            SFX
        };
        public AudioMenu AudioMenuOption = AudioMenu.BGM;
        #endregion

        #region Singleton
        private static ArmadHeroes_Pause m_instance;
        public static ArmadHeroes_Pause instance { get { return m_instance; } protected set { m_instance = value; } }
        #endregion

        #region Public Members
        public List<Text> m_MainMenuOptions = new List<Text>();
        public List<Text> m_AudioMenuOptions = new List<Text>();
        public Animator m_animator;
        public Toggle m_bgmToggle, m_sfxToggle;
        public Color m_highlight = Color.red, m_unselected = Color.white;
        public AudioClip m_select, m_cursor;
        public GameObject m_pause, m_mainPanel, m_audioPanel;
        public int playerID;
        #endregion

        #region Private Members 
        private int menuIndex;
        private List<Text> m_currentMenu;
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            //set up singleton
            instance = this;
            //default state
            state = PauseState.unpaused;
            m_animator.SetBool("isPaused", false);
        }

        void Update()
        {
            //check state
            switch (state)
            {
                case PauseState.paused:
                    Tick();
                    break;
                case PauseState.unpaused:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region ArmadHeroes_Pause Behaviours
        /// <summary>
        /// To be called by players
        /// Actors pass their controller ID
        /// </summary>
        /// <param name="_controllerID"></param>
        public void Pause(int _controllerID)
        {
            //if not paused already
            if (state != PauseState.paused)
            {
                GameManager.instance.state = GameStates.pause;
                //pause state
                state = PauseState.paused;
                //set up controller listen
                playerID = _controllerID;
                return;
            }
            //if the player is pause'in again
            if(_controllerID == playerID)
            {
                GameManager.instance.state = GameStates.game;
                state = PauseState.unpaused;
            }
        }

        /// <summary>
        /// Only tick when paused
        /// </summary>
        private void Tick()
        {
            //check D-pad input from paused 'master' player
            if(ControllerManager.instance.GetController(playerID).d_down.JustReleased() || (ControllerManager.instance.GetController(playerID).moveY.JustPressed() && ControllerManager.instance.GetController(playerID).moveY.GetValue() <0))
            {
                SoundManager.instance.PlayClip(m_cursor);
                //change index ~ account for wrap
                menuIndex = menuIndex == m_currentMenu.Count-1 ? 0 : menuIndex += 1;
            }
            if (ControllerManager.instance.GetController(playerID).d_up.JustReleased() || (ControllerManager.instance.GetController(playerID).moveY.JustPressed() && ControllerManager.instance.GetController(playerID).moveY.GetValue()>0))
            {
                SoundManager.instance.PlayClip(m_cursor);
                menuIndex = menuIndex == 0 ? m_currentMenu.Count - 1 : menuIndex -= 1;
            }

            //Tick current window
            switch (CurrentWindowState)
            {
                case WindowStates.MainMenu:
                    MainMenuTick();
                    break;
                case WindowStates.AudioMenu:
                    AudioMenuTick();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Tick for MainMenu 
        /// Options
        /// </summary>
        private void MainMenuTick()
        {
            //check for select button
            if (ControllerManager.instance.GetController(playerID).boostButton.JustReleased())
            {
                SoundManager.instance.PlayClip(m_select);
                switch (MainMenuOption)
                {
                    case MainMenu.Continue:
                        Pause(playerID);//unpause game 
                        break;
                    case MainMenu.Options:
                        m_currentMenu[menuIndex].color = m_unselected;//reset 
                        m_currentMenu = m_AudioMenuOptions;//switch to audio menu option
                        menuIndex = 0;//reset index
                        CurrentWindowState = WindowStates.AudioMenu;//set new tick state
                        m_audioPanel.SetActive(true);//turn on audio panel
                        m_mainPanel.SetActive(false);//turn off main
                        break;
                    case MainMenu.Exit:
                        DOTween.TogglePauseAll();
                        Time.timeScale = 1.0f;
                        state = PauseState.unpaused;
                        GameManager.instance.state = GameStates.game;
                        m_animator.SetBool("isPaused", false);
                        SceneManager.LoadScene("MainMenu");
                        break;
                    default:
                        break;
                }
            }

            //Update Main Menu Selection
            m_currentMenu[(int)MainMenuOption].color = m_unselected;
            //set MainMenu state
            MainMenuOption = (MainMenu)menuIndex;
            //set selected highlight
            m_currentMenu[menuIndex].color = m_highlight;     
        }

        /// <summary>
        /// Tick for AudioMenu 
        /// Options
        /// </summary>
        private void AudioMenuTick()
        {
            //check for select button 
            if (ControllerManager.instance.GetController(playerID).boostButton.JustReleased())
            {
                SoundManager.instance.PlayClip(m_select);
                float vol;
                switch (AudioMenuOption)
                {
                    case AudioMenu.BGM:
                        m_bgmToggle.isOn = m_bgmToggle.isOn ? false : true;
                        vol = m_bgmToggle.isOn ? 0.0f : -80.0f;
                        SoundManager.instance.SetMasterChannelVol(vol);
                        break;
                    case AudioMenu.SFX:
                        m_sfxToggle.isOn = m_sfxToggle.isOn ? false : true;
                        vol = m_sfxToggle.isOn ? 0.0f : -80.0f;
                        SoundManager.instance.SetSFXChannelVol(vol);
                        break;
                    default:
                        break;
                }
            }

            //Check for back button 
            if(ControllerManager.instance.GetController(playerID).activateButton.JustReleased())
            {
                m_currentMenu[menuIndex].color = m_unselected;//reset
                m_currentMenu = m_MainMenuOptions;//switch to audio menu option

                menuIndex = 0;//reset index
                CurrentWindowState = WindowStates.MainMenu;//set new tick state
                m_audioPanel.SetActive(false);//turn on audio panel
                m_mainPanel.SetActive(true);//turn off main
            }

            //Update Main Menu Selection
            m_currentMenu[(int)AudioMenuOption].color = m_unselected;
            //set MainMenu state
            AudioMenuOption = (AudioMenu)menuIndex;
            //set selected highlight
            m_currentMenu[menuIndex].color = m_highlight;     
        }
        #endregion
    }
}