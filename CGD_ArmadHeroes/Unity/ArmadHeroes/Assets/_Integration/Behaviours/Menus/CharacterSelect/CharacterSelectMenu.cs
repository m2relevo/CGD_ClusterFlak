/// <summary>
/// Created by Alan Parsons
/// Edited by Shaun Landy
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

namespace ArmadHeroes{
	public class CharacterSelectMenu : MonoBehaviour 
    {
        static private CharacterSelectMenu m_instance = null;
        static public CharacterSelectMenu instance { get { return m_instance; } }

		public AudioClip stampSfx, pageTurnSfx, characterSplitSfx;
        public CharacterSelectFile[] characterFiles;

		int activeCharacters = 0;
        int readyPlayers = 0;
        bool readyDelay = true;
		[HideInInspector]
		public bool fading = false;
        private bool toMissionBriefing = false;

		// Use this for initialization
		void Awake ()
        {
            m_instance = this;
		}


		void OnEnable(){
			fading = false;
		}

		public void Start()
        {
            for (int i = 0; i < characterFiles.Length; i++)
            {
                characterFiles[i].InitProfile(i);
            }

            toMissionBriefing = false;
            GameManager.instance.characterSelect = gameObject;
            //If coming from debrief screen
            if(GlobalPlayerManager.instance.currentMission != string.Empty)
            {
                toMissionBriefing = true;
                return;
            }

            activeCharacters = Mathf.Min (characterFiles.Length, ControllerManager.instance.assignableCount);
			//Assign controllers to files
			for (int i = 0; i < activeCharacters; i++) {
                int _controller = ControllerManager.instance.AssignNext();
                characterFiles [i].AssignController (_controller);
                GlobalPlayerManager.instance.SetActive(i, true);
                GlobalPlayerManager.instance.SetControllerIndex(i, _controller);
                GlobalPlayerManager.instance.SetCharacter(i, i);
            }

		}


        // Update is called once per frame
        void LateUpdate()
        {
            if (toMissionBriefing)
            {
                LoadMissionBriefing(false);
                toMissionBriefing = false;
                return;
            }
            if (fading) {
                return;
            }
            //Activate additional characters if controllers plugged in
            for (int i = activeCharacters; characterFiles.Length > activeCharacters && ControllerManager.instance.assignableCount > 0; i++)
            {
                int _controller = ControllerManager.instance.AssignNext();
                characterFiles[i].AssignController(_controller);
                GlobalPlayerManager.instance.SetActive(i, true);
                GlobalPlayerManager.instance.SetControllerIndex(i, _controller);
                GlobalPlayerManager.instance.SetCharacter(i, i);
                activeCharacters++;
            }

            if (!readyDelay)
            {
                if (ControllerManager.instance.GetController(0).back.JustPressed())
                {
                    ControllerManager.instance.ResetContollers();
                    for (int i = 0; i < CharacterSelectMenu.instance.characterFiles.Length; i++)
                    {
                        GlobalPlayerManager.instance.playerData[i + 4].activePlayer = false;
                        characterFiles[i].Reset();
                    }
                    ControllerManager.instance.UnassignAll();
                    GameManager.instance.FadeToBlack(() => { ToSplashScreen(); });
                }
            }
            if(readyPlayers <= 0)
            {
                readyDelay = false;
            }
            else
            {
                readyDelay = true;
            }
        }

        void ToSplashScreen()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        public void ReadyUp()
        {
            ++readyPlayers;
			SoundManager.instance.PlayClip (stampSfx);
            //Only join if there is more than 1 player
            //You can only play 5 / 7 games with 2 players minimum otherwise the game does not work
            //This is not a bug, this is intended, though there should be a visible que to let you know
            //DO NOT REMOVE
            if (readyPlayers == 1)
            {
                if (!characterFiles[0].isSplit)
                {
                    return;
                }
            }
            if(readyPlayers == ReInput.controllers.joystickCount || readyPlayers == 4)
            {
                LoadMissionBriefing();
            }
        }

        public void Unready()
        {
            --readyPlayers;
			readyPlayers = Mathf.Max (0, readyPlayers);
        }

		void LoadMissionBriefing(bool _fade = true){
            if (_fade)
            {
                fading = true;
                GameManager.instance.FadeToBlack(() =>
                {
                    gameObject.SetActive(false);
                    GameManager.instance.LoadMissionBriefing();
                });
            }
            else
            {
                gameObject.SetActive(false);
                GameManager.instance.LoadMissionBriefing();
            }
		}

		public void PlayPageTurnSFX(){
			SoundManager.instance.PlayClip (pageTurnSfx);
		}

		public void PlayCharacterSplitSFX(){
			SoundManager.instance.PlayClip (characterSplitSfx);
		}



	}
}
