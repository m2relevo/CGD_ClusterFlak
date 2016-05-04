/// <summary>
/// CanvasController.cs 
/// Created and implemented by Daniel Weston - 24/02/16
/// </summary>
/// 
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public class CanvasController : MonoBehaviour
    {
        #region Singleton
        private static CanvasController m_instance;
        public static CanvasController instance { get { return m_instance; } }
        #endregion

        public GameObject PickUpPrompt,
            DefendPrompt;
        public GameObject FinishText;

        private bool prompt = false;

        #region Unity Callbacks
        void Awake()
        {
            m_instance = this;
        }

        void Update()
        {
            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    if (!prompt)
                    {
                        TweenTextIn(PickUpPrompt);
                        prompt = !prompt;
                    }
                    break;
                case GameStates.pause:
                    break;
                case GameStates.gameover:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CanvasController Behaviours
        public void TweenTextIn(GameObject _object)
        {
            Tweener GrabAGun = _object.transform.DOMoveY(Screen.height - 100.0f, 1.0f);
            GrabAGun.SetEase(Ease.InElastic);
            GrabAGun.easePeriod = 2.0f;
            //GrabAGun.OnComplete(TweenTextOut);
        }

        public void TweenTextInOut(GameObject _object, float time)
        {
            Tweener GrabAGun = _object.transform.DOMoveY(Screen.height - 100.0f, 0.0f);
            GrabAGun.SetEase(Ease.InElastic);
            GrabAGun.easePeriod = 0.5f;

            StartCoroutine(ITweenTextOut(_object, time));
        }

        public void TweenTextOut(GameObject _object)
        {
            Tweener GrabAGun = _object.transform.DOMoveY(Screen.height + 100.0f, 2.0f);
            GrabAGun.SetEase(Ease.OutElastic);
            GrabAGun.easePeriod = 2.0f;

            if (_object.GetComponent<TextLerp>())
            {
                _object.GetComponent<TextLerp>().enabled = false;
            }
        }

        IEnumerator ITweenTextOut(GameObject _object, float time)
        {
            yield return new WaitForSeconds(time);
            Tweener GrabAGun = _object.transform.DOMoveY(Screen.height + 100.0f, 2.0f);
            GrabAGun.SetEase(Ease.OutElastic);
            GrabAGun.easePeriod = 2.0f;

            if (_object.GetComponent<TextLerp>())
            {
                _object.GetComponent<TextLerp>().enabled = false;
            }
        }
        #endregion
    }
}