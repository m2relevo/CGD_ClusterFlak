using UnityEngine;
using System.Collections;
namespace ShellShock
{
    //integrated from ShellShock
    public class cf_camerashake : MonoBehaviour
    {

        public float shakeTimer; //Time the shake lasts
        public float shakeIntesity;//Intensity of the shake
        private Vector3 cameraStartPos;
       
        private bool explosiontriggerbool;
        
        
        // Use this for initialization
        void Start()
        {
            cameraStartPos = transform.position;
            explosiontriggerbool = false;
        }

        // Update is called once per frame
        void Update()
        {
            //gets game time and triggers camera shake on missile hit
            if (GameObject.Find("GameTimer").GetComponent<Timer>().timeLeft < 0)
            {
                explosiontriggerbool = GameObject.FindWithTag("ClusterFlak/ICBM").GetComponent<CF_ICBM>().explosiontrigger;
                if (explosiontriggerbool == true)
                {
                    shakeCamera(0.1f, 1.0f);
                }
            }

            if (shakeTimer >= 0)
            {
                Vector2 shakePosition = Random.insideUnitCircle * shakeIntesity; //Circle that has a 1 unit radius and picks a random x,y value that the camera moves to
                transform.position = new Vector3(transform.position.x + shakePosition.x, transform.position.y + shakePosition.y, transform.position.z); //updates camera position with the insideUnitCircle offset
                shakeTimer -= Time.deltaTime;
            }
            if (shakeTimer <= 0)
            {
                transform.position = cameraStartPos;

            }
        }

        public void shakeCamera(float shakePower, float shakeDuration)
        {
            shakeIntesity = shakePower;
            shakeTimer = shakeDuration;
        }
    }
}
