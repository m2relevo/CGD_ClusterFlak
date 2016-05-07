using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace ArmadHeroes
{
    public enum GameStates
    {
        game, 
        pause,
        gameover
    };

	public class GameManager : MonoBehaviour
    {
        #region Callbacks
        public delegate void PauseGame();
        public PauseGame m_pauseGame;
        public delegate void PlayGame();
        public PlayGame m_playGame;
        public delegate void GameOver();
        public GameOver m_gameOver;
        #endregion

        #region State
        private GameStates m_state = GameStates.game;
        public GameStates state { get { return m_state; } set { SetState(value); } }
        private void SetState(GameStates _state)
        {
            m_state = _state;
            switch (_state)
            {
                case GameStates.game:
                    Play();
                    break;
                case GameStates.pause:
                    Pause();
                    break;
                case GameStates.gameover:
                    EndGame();
                    break;
                default:
                    break;
            }
        }
        private void Pause()
        {
            DOTween.PauseAll();
			if (m_pauseGame != null) {
				m_pauseGame();
                Time.timeScale = 0.0f;
			}
            Time.timeScale = 0.0f;
        }
        private void Play()
        {
            DOTween.TogglePauseAll();
			if (m_playGame != null) {
				m_playGame();
                Time.timeScale = 1.0f;
			}
            Time.timeScale = 1.0f;
        }
        private void EndGame()
        {
			if (m_gameOver != null) {
				m_gameOver();
			}
            
            SceneManager.LoadScene("DebriefScene");
        }


        #endregion

        #region Public Members
        public Image blackFade;
		public Canvas canvas;
	
		public GameObject characterSelect,missionBriefing;
        #endregion

        #region Singleton
        private static GameManager m_instance;
		public static GameManager instance {get {return m_instance;} protected set {m_instance = value;}}
        #endregion

        #region Unity Callbacks 
        void Awake()
        {
			//No instance exists
			if (GameManager.instance == null) {
                if (canvas)
                {
                    canvas.worldCamera = Camera.main;
                }
				instance = this;
				DontDestroyOnLoad (gameObject);
				//Enable all children
				for (int i = 0; i < gameObject.transform.childCount; i++) {
					gameObject.transform.GetChild (i).gameObject.SetActive (true);
				}
			} else {
				//Destroy as instance exists
				Destroy (gameObject);
			}

		}

		void Update(){
            if (canvas)
            {
                if (canvas.worldCamera == null)
                {
                    canvas.worldCamera = Camera.main;
                }
            }
		}
        #endregion

        #region GameManager Behaviours
        public void LoadMissionBriefing()
        {
			characterSelect.SetActive (false);
			missionBriefing.SetActive (true);
        }

        public void BackOutFromMissionBriefing()
        {
            GlobalPlayerManager.instance.Reset();
            GameManager.instance.FadeToBlack (() => {
				missionBriefing.SetActive (false);
				characterSelect.SetActive (true);

                ControllerManager.instance.ResetContollers();
                for (int i = 0; i < CharacterSelectMenu.instance.characterFiles.Length; i++) {
                    GlobalPlayerManager.instance.playerData[i + 4].activePlayer = false;
                    CharacterSelectMenu.instance.characterFiles[i].Reset();
				}
                ControllerManager.instance.UnassignAll();
                CharacterSelectMenu.instance.Start();
            });
        }

		public void LoadCharacterSelect()
        {
            SceneManager.UnloadScene("MainMenu");
			SceneManager.LoadScene("CharacterSelect");
		}

		public void FadeToBlack(TweenCallback midfadeCallback, float fadeTime = 0.5f)
        {
			Sequence fade = DOTween.Sequence ();
			Color col = Color.black;
			col.a = 0;
			blackFade.color = col;
			col.a = 1;
			fade.Append (blackFade.DOColor (col, fadeTime));
			fade.AppendCallback (midfadeCallback);
			//Needed for scene loads, otherwise fade in starts slightly
			fade.AppendInterval (0.02f);
			col.a = 0;
			fade.Append (blackFade.DOColor (col, fadeTime));
		}

		public void ShakeScreen(float duration, float strength, int vibrato)
		{
			Camera.main.DOShakePosition (duration, strength, vibrato);
		}
        #endregion
	}
}