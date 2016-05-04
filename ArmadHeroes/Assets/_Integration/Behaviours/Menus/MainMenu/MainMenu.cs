using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace ArmadHeroes{
	public class MainMenu : MonoBehaviour {

		public GameObject logo;
		public AudioClip logoZoomSfx, logoHitSfx, startSfx;


		bool canPressStart = false;


        void Start ()
        {
           
			canPressStart = false;
            logo.SetActive(true);
            ZoomLogoTween();
            TweenLogo();
        }
		
		// Update is called once per frame
		void Update () {
            if (canPressStart) {
				CheckForStartPress ();
			}

		}


		/// <summary>
		/// Checks if any controller has pressed x this frame
		/// </summary>
		void CheckForStartPress(){
			for (int i = 0; i < ControllerManager.instance.controllerCount; i++) {
                if(ControllerManager.instance.GetController(i) == null)
                {
                    continue;
                }
				if (ControllerManager.instance.GetController (i).boostButton.JustPressed () || ControllerManager.instance.GetController (i).pauseButton.JustPressed ()) {
					LoadCharacterSelect ();

					canPressStart = false;
					SoundManager.instance.PlayClip (startSfx);
				}
			}
		}

		void LoadCharacterSelect(){

			GameManager.instance.FadeToBlack (() => {

				GameManager.instance.LoadCharacterSelect ();
			});
		}

		void TweenLogo(){
			Sequence logoMove = DOTween.Sequence ();
			Tweener moveDown = logo.transform.DOLocalMoveY (0, 1.2f);
			Tweener moveUp = logo.transform.DOLocalMoveY (30, 1.2f);
			logoMove.Append (moveDown);
			logoMove.Append (moveUp);
			moveDown.SetEase (Ease.InOutSine);
			moveUp.SetEase (Ease.InOutSine);
			logoMove.OnComplete (()=>{
				TweenLogo();
			});
		}
        void ZoomLogoTween()
        {
			
            Sequence scaleLogo = DOTween.Sequence();
            Tweener scaleLogoBig = logo.transform.DOScale(80, 1.0f);
            scaleLogo.Append(scaleLogoBig);
            scaleLogoBig.SetEase(Ease.OutCirc);
            scaleLogo.OnComplete(ZoomLogoTween);
			scaleLogo.OnComplete (()=>{
				SoundManager.instance.PlayClip(logoHitSfx);
				canPressStart = true;
			});

			scaleLogo.InsertCallback (0.3f, () => {
				SoundManager.instance.PlayClip(logoZoomSfx);
			});
        }
	}
}
