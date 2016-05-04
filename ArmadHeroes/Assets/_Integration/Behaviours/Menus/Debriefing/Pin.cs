using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ArmadHeroes
{
    public class Pin : MonoBehaviour
    {
        private int controllerID;
        public Controller controller
        {
            private set { }
            get { return ControllerManager.instance.GetController(controllerID); }
        }
        [SerializeField] private Image pin = null;

        private float fadeInTime = 0.0f;
        private float fadeInDuration = 0.2f;

        private float waitDuration = 2.0f;
        private float waitTime = 0.0f;
        private float targetDuration = 1.0f;
        private float targetTime = 0.0f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 startScale;
        private Vector3 targetScale;

        private bool waitFinished = false;
        private bool targetFinished = true;
        private AccoladeEnum accolade;
        public delegate void WaitReached(AccoladeEnum _enum, GameObject _this);
        WaitReached waitReached;
        public delegate void TargetReached();
        TargetReached targetReached;

        // Update is called once per frame
        void Update()
        {
            fadeInTime = fadeInTime >= fadeInDuration ? fadeInDuration : fadeInTime += Time.deltaTime;
            pin.color = new Color(1, 1, 1, fadeInTime / fadeInDuration);

            //Skip rest of update if finished
            if(targetFinished)
            {
                return;
            }
            //Skip Wait
            if (controller.boostButton.JustPressed() || controller.pauseButton.JustPressed())
            {
                waitTime = waitDuration;
            }
            waitTime = waitTime > waitDuration ? waitDuration : waitTime + Time.deltaTime;
            if(waitTime >= waitDuration)
            {
                //On wait finished attempt to call functionality then return
                if (!waitFinished)
                {
                    waitFinished = true;
                    if (waitReached != null)
                    {
                        waitReached(accolade, gameObject);
                    }
                    return;
                }
                //Skip animation
                if (controller.boostButton.JustPressed() || controller.pauseButton.JustPressed())
                {
                    targetTime = targetDuration;
                }
                targetTime = targetTime > targetDuration ? targetDuration : targetTime + Time.deltaTime;
                //Move and scale
                transform.position = Vector3.Lerp(startPosition, targetPosition, targetTime / targetDuration);
                transform.localScale = Vector3.Lerp(startScale, targetScale, targetTime / targetDuration);
                //On target finished attempt to call functionality
                if(targetTime >= targetDuration)
                {
                    if (!targetFinished)
                    {
                        targetFinished = true;
                        if(targetReached != null)
                        {
                            targetReached();
                        }
                    }
                }
            }
            
        }

        public void Init(int _controllerID, Vector3 _position, Vector3 _scale, float _wait, TargetReached _targetFunc = null, 
            WaitReached _waitFunc = null, AccoladeEnum _enum = AccoladeEnum.MAX_ACCOLADES)
        {
            controllerID = _controllerID;
            targetPosition = _position;
            targetScale = _scale;
            waitDuration = _wait;
            startPosition = transform.position;
            startScale = transform.localScale;
            waitReached = _waitFunc;
            accolade = _enum;
            targetReached = _targetFunc;
            targetFinished = false;
        }

        public void DestroyPin(float _time)
        {
            Destroy(gameObject, _time);
        }
    }
}
