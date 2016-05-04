using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using ArmadHeroes;
using DG.Tweening;

namespace ArmadHeroes{
	[System.Serializable]
	public struct Mission
	{
		public string name;
		public string description;
		public string sceneToLoad;
		public Sprite missionImage;
        public AccoladeEnum uniqueAccolade;
	}

	public class MissionBriefing : MonoBehaviour {

		[SerializeField] private Mission[] missions;


		public AudioClip missionChangeSfx;
		public Image missionImage;
		public Text gameName;
        public Text gameDescription;


        private int currentMission = 0;
		public bool fading = false;

		// Use this for initialization
		void Awake () {
			
            SetMissionUI ();

			FlickerProjectorImage ();

        }

        void Start()
        {
            GameManager.instance.missionBriefing = gameObject;
            gameObject.SetActive(false);
        }

		void OnEnable(){
			fading = false;
        }
		
		// Update is called once per frame
		void Update () {
			if (fading) {
				return;
			}
			//Player 1 starts game
			
			if (ControllerManager.instance.GetController(0).confirm.JustPressed()) {
				StartGame ();
			}
			

			//Dpad
			if (ControllerManager.instance.GetController (0).d_left.JustPressed ()) {
				PrevMission ();
			} else if (ControllerManager.instance.GetController (0).d_right.JustPressed ()) {
				NextMission ();
			}

			//Analog
			else if (ControllerManager.instance.GetController(0).moveX.JustPressed())
			{
				if (ControllerManager.instance.GetController (0).moveX.GetValue () > 0) {
					NextMission ();
				} else {
					PrevMission ();
				}
			}

            if (ControllerManager.instance.GetController(0).back.JustPressed())
            {
                GameManager.instance.BackOutFromMissionBriefing();
               
            }

		}

		void StartGame()
		{
			if (missions [currentMission].sceneToLoad.Length > 0) {
				fading = true;
                GlobalPlayerManager.instance.SetGameAccolade(missions[currentMission].uniqueAccolade);
                GlobalPlayerManager.instance.SetCurrentMission(missions[currentMission].sceneToLoad);
                LoadScene (missions [currentMission].sceneToLoad);
			}

		}

		void LoadScene(string name){
			GameManager.instance.FadeToBlack(()=>{
				SceneManager.LoadScene(name);
			});

		}

		void NextMission(){
			currentMission++;

			if (currentMission> missions.Length-1) {
				currentMission = 0;
			}

			SetMissionUI ();

			SoundManager.instance.PlayClip (missionChangeSfx);
        }

		void PrevMission(){
			currentMission--;

			if (currentMission < 0) {
				currentMission = missions.Length-1;
			}

			SetMissionUI ();

			SoundManager.instance.PlayClip (missionChangeSfx);
        }

		void SetMissionUI(){
			missionImage.sprite = missions [currentMission].missionImage;
			gameName.text = missions [currentMission].name;
			gameDescription.text = missions [currentMission].description;
		}

		void FlickerProjectorImage(){
			Color flickerColor = missionImage.color;
			flickerColor.a = Random.Range(0.7f,0.9f);
			Sequence flicker = DOTween.Sequence ();
			flicker.Append(missionImage.DOColor (flickerColor, Random.Range(0.05f,0.15f)));
			flickerColor.a = 1.0f;
			flicker.Append(missionImage.DOColor (flickerColor, Random.Range(0.05f,0.15f)));
			flicker.OnComplete (FlickerProjectorImage);
		}
	}
}
