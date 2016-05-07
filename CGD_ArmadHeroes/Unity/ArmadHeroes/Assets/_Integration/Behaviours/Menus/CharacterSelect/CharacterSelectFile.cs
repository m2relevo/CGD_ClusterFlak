/// <summary>
/// Created by Alan Parsons
/// Edited by Julian Stopher and Shaun Landy
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ArmadHeroes;

namespace ArmadHeroes{
	public class CharacterSelectFile : MonoBehaviour {

        private int fileIndex = 0;
        public Image mainCharacter, splitCharacter, splitOverlay;
		public Text characterName;
        public GameObject readyStamp, classifiedStamp;

		public Shader greyscaleShader;

		private string mainName = "raf"; 
		private string splitName = "raf";
		private bool split = false;
        public bool isSplit
        {
            get { return split; }
            set { }
        }
        private bool ready = false;

		private Controller controller {
			get { return ControllerManager.instance.GetController (controllerIndex); }
			set{ }
		}

		private Controller splitController {
			get { return ControllerManager.instance.GetController (splitControllerIndex); }
			set{ }
		}

        public int controllerIndex = -1;
        public int splitControllerIndex = -1;

        // Use this for initialization
        void Awake () {
			mainCharacter.material = new Material (greyscaleShader);
			mainCharacter.material.color = new Color (1, 1, 1, 1);
			mainCharacter.material.SetFloat ("_GrayscaleAmount", 1);
            classifiedStamp.SetActive(false);
		}
		
		// Update is called once per frame
		void Update () {
			if (CharacterSelectMenu.instance.fading) {
				return;
			}
			if (!ready) {
				int changeDirection = GetChangeDirection (controller);
				if (changeDirection != 0) {
					CharacterSelectMenu.instance.PlayPageTurnSFX ();
                    ChangeCharacter (changeDirection);
				}

                //Split
                if (split)
                {
                    changeDirection = GetChangeDirection(splitController);
                    if (changeDirection != 0)
                    {
                        CharacterSelectMenu.instance.PlayPageTurnSFX();
                        ChangeSplitCharacter(changeDirection);
                    }
                }

				//Split Dpad

				if (controller.splitButton.JustPressed())
				{
					if (!split)
					{
						SplitCharacter();
					}
					else
					{
						FuseCharacter();
					}
				}

			}

            if (controller.boostButton.JustPressed())
            {
				Ready ();
                return;
            }
            else if (controller.useButton.JustPressed())
            {
				Unready ();
                return;
            }
            if (split)
            {
                if (splitController.boostButton.JustPressed())
                {
                    Ready();
                }
                else if (splitController.useButton.JustPressed())
                {
                    Unready();
                }
            }
        }


		void Ready(){
            if (!ready)
            {
                readyStamp.SetActive(true);
                //Random rotation
                readyStamp.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-20.0f, 20.0f));
                ready = true;
                CharacterSelectMenu.instance.ReadyUp();
            }
		}
		public void Unready(){
            if (ready)
            {
                readyStamp.SetActive(false);
                ready = false;
                CharacterSelectMenu.instance.Unready();
            }
		}

		int GetChangeDirection(Controller _controller){
			if (_controller.moveX.GetValue () != 0 && _controller.moveX.JustPressed ()) {
				int changeDirection = _controller.moveX.GetValue () > 0 ? 1 : -1;
				return changeDirection;
			}

			//Dpad
			if (_controller.d_left.JustPressed ()) {
				return -1;
			}
			else if (_controller.d_right.JustPressed ()) {
				return 1;
			}

			return 0;
		}

		//Set an controller to control this character file
		public void AssignController(int _controllerIndex){
			controllerIndex = _controllerIndex;
			mainCharacter.material.SetFloat ("_GrayscaleAmount", 0);

			SetCharacterName ();
            classifiedStamp.SetActive(false);
        }

        //Initilise original character select file
        public void InitProfile(int _character)
        {
            fileIndex = _character;
            mainCharacter.sprite = CharacterProfiles.instance.GetProfile((CharacterType)_character).characterFileImage;
            splitCharacter.sprite = CharacterProfiles.instance.GetProfile((CharacterType)_character).characterFileImage_right;
            mainName = CharacterProfiles.instance.GetProfile((CharacterType)_character).characterName;
            splitName = mainName;
            SetCharacterName();
        }

        public void ChangeCharacter(int _direction){
            CharacterType _type = GlobalPlayerManager.instance.ChangeCharacter(controller.playerIndex, _direction);
            mainName = CharacterProfiles.instance.GetProfile(_type).characterName;
			if (!split) {
				mainCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage;
			} else {
				mainCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage_left;
			}

            
            SetCharacterName ();
		}

		public void ChangeSplitCharacter(int _direction){
            CharacterType _type = GlobalPlayerManager.instance.ChangeCharacter(controller.playerIndex + 4, _direction);
            splitName = CharacterProfiles.instance.GetProfile(_type).characterName;
            splitCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage_right;
            SetCharacterName();
        }

		void SplitCharacter(){
            //Process of splitting controller
            splitControllerIndex = ControllerManager.instance.SplitController(controller);
            GlobalPlayerManager.instance.SetActive(controller.playerIndex + 4, true);
            GlobalPlayerManager.instance.SetControllerIndex(controller.playerIndex + 4, splitControllerIndex);
            //Setting up character overlay properly
            splitOverlay.gameObject.SetActive (true);
			splitCharacter.gameObject.SetActive (true);

			CharacterType _type = GlobalPlayerManager.instance.GetPlayerData(controller.playerIndex).character;
            GlobalPlayerManager.instance.SetCharacter(controller.playerIndex + 4, (int)_type);
            mainCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage_left;
            splitCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage_right;
            splitName = CharacterProfiles.instance.GetProfile(_type).characterName;
            split = true;

			CharacterSelectMenu.instance.PlayCharacterSplitSFX ();

			SetCharacterName ();
		}

        void FuseCharacter()
        {
            GlobalPlayerManager.instance.SetActive(controller.playerIndex + 4, false);
            ControllerManager.instance.FuseController(controller);

            FuseGraphics();
        }  
 
        void FuseGraphics()
        {  
			CharacterType _type = GlobalPlayerManager.instance.GetPlayerData(controller.playerIndex).character;
			mainCharacter.sprite = CharacterProfiles.instance.GetProfile(_type).characterFileImage;
            
            splitOverlay.gameObject.SetActive(false);
            splitCharacter.gameObject.SetActive(false);
            mainCharacter.fillAmount = 1.0f;
            split = false;

            SetCharacterName();
        }

        void SetCharacterName()
		{
			//Unassigned player
			if (controllerIndex < 0) {
				characterName.text = "";
                classifiedStamp.SetActive(true);
                classifiedStamp.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-15, 15));
			}
			else if (!split) {
				characterName.text = mainName;
                
            } else {
				characterName.text = mainName + " / " + splitName;
            }
		}

        public void Reset()
        {
            SetCharacterName();
            FuseGraphics();
            mainCharacter.material.SetFloat("_GrayscaleAmount", 1);
            Unready();
            controllerIndex = -1;
        }
	}
}
