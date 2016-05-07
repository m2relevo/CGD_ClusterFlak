using UnityEngine;
using System.Collections;
namespace DilloDash
{
    public class LaserBeam : MonoBehaviour
    {
        public Vector3 maxLaserSize = new Vector3(1f, 30f, 1f);
        Vector3 LaserVariation = Vector3.zero;
        public Vector3 shrinkLaserSize = new Vector3(0f, 0f, 1f);
        public bool chargeBall;
        private bool GoingUp;
        [SerializeField]
        private float expandSpeed = 2f;
        [SerializeField]
        private float shrinkSpeed = 10f;
        [SerializeField]
        private float shrinkTime = 1f;
        [SerializeField]
        private float useTime = 4f;
        [SerializeField]
        private float totalTime = 5f;
        bool usingLaser = false;
        private float laserCount = 0;

        // Use this for initialization
        void Awake()
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            LaserVariation = maxLaserSize * 0.85f;
        }
        // Update is called once per frame
        void Update()
        {
            laserBeam();
        }
        public void SetLaserGrowSpeed(float _enlarge)
        {
            expandSpeed = _enlarge;
        }
        public void SetLaserShrinkTime(float _shrinkTime)
        {
            shrinkTime = _shrinkTime;
        }
        public void SetLaserShrinkSpeed(float _shrink)
        {
            shrinkSpeed = _shrink;
        }
        public void SetLaserTime(float _laserTime)
        {
            totalTime = _laserTime;
            useTime = totalTime - shrinkTime;
        }
        void laserBeam()
        {
            switch (usingLaser)
            {
                case true:
                    if (laserCount >= useTime)
                    {
                        ShrinkLaser();
                    }
                    else if (laserCount >= totalTime)
                    {
                        laserCount = 0f;
                        usingLaser = false;
                    }
                    else
                    {
                        //add time to countdown
                        laserCount += Time.fixedDeltaTime;
                        GrowLaser();
                        if (chargeBall)
                        {
                           transform.Rotate(0f, 0f, 2000f * Time.deltaTime);
                        }
                    }
                    break;
                case false:
                    usingLaser = true;
                    break;
            }
        }
        void GrowLaser()
        {
            if (GoingUp)
            {
                if (gameObject.transform.localScale == maxLaserSize)
                {
                    GoingUp = false;
                }
                else
                {
                    gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, maxLaserSize, expandSpeed * Time.deltaTime);
                }

              
            }
            else {
                if ((gameObject.transform.localScale == LaserVariation))
                {
                    GoingUp = true;
                }
                {
                    gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, LaserVariation, expandSpeed * 3f * Time.deltaTime);
                }
            }
        }
        void ShrinkLaser()
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, shrinkLaserSize, shrinkSpeed * Time.deltaTime);
        }

        protected void OnTriggerStay2D(Collider2D _other)
        {
            if (_other.tag == "Player" && !chargeBall)
            {
                _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position,true);
            }
        }
    }

}
