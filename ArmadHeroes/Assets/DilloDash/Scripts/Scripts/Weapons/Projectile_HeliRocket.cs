//Using Armatillery Explosion Manager

using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class Projectile_HeliRocket : ArmadHeroes.Projectile
    {
        [SerializeField]
        private float dropRate = 5.0f;
        [SerializeField]
        private float randomRange = 0.5f;
        private float dropTimer = 0.0f;
        private bool impact = false;
        private float originalHeight;
        private Vector3 origin;
        private Vector3 shadowOrigin;
        private Vector3 target;
        [SerializeField] private GameObject frontPivot;
        [SerializeField] private GameObject shadowFrontPivot;

        public override void Fire(int _playerId, float height = 0, Collider2D _ignoreCollider = null)
        {
            gameObject.SetActive(true);
            callerID = _playerId;
            dropTimer = dropRate;
            impact = false;

            //Setup target point ot head towards as opposed to force movement
            origin = transform.position;
            target = transform.position;
            target.y -= 15.0f;
            shadowOrigin = target;
            shadow.transform.position = shadowOrigin;
            //Snap to an angle of 45 degrees
            float _angle = (Mathf.Rad2Deg * Mathf.Atan2(direction.x, -direction.y)) - 90;
            _angle = Mathf.Round(_angle / 45) * 45;
            direction = Quaternion.AngleAxis(_angle, Vector3.forward) * Vector3.right;
            //Use dot product to determine appropriate direction and distance in iso
            target.y += (Vector3.Dot(direction, Vector3.up) * 22.5f) + Random.Range(-randomRange, randomRange);
            target.x += (Vector3.Dot(direction, Vector3.right) * 45.0f) +Random.Range(-randomRange*2, randomRange*2);
            //Use this to initlise starting rotation so shadow does not set to an unusual location
            transform.localEulerAngles = new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(target.x - origin.x, -(target.y - origin.y)) - 90));
            SetTransform();
         
        }
        protected override void Update()
        {
            //Do nothing, Fixed Update manages to match physics on everything else
        }

        void FixedUpdate()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                if(impact)
                {
                    gameObject.SetActive(false);
                }
                dropTimer = Mathf.Max(dropTimer - Time.deltaTime, 0.0f);
                if(dropTimer == 0.0f)
                {
                    impact = true;
                    GameObject _explosion = Armatillery.ExplosionManager.instance.GetExplosion();
                    _explosion.transform.position = target;
                    _explosion.SetActive(true);
                    _explosion.GetComponent<Explosion>().InitExplode(0.20f);
                }
                SetTransform();
            }
        }

        void SetTransform()
        {
            transform.position = Vector3.Lerp(target - (frontPivot.transform.position - transform.position), origin, dropTimer / dropRate);
            shadow.transform.position = Vector3.Lerp(target - (shadowFrontPivot.transform.position - shadow.transform.position), shadowOrigin, dropTimer / dropRate);

            transform.localEulerAngles = new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(target.x - origin.x, -(target.y - origin.y)) - 90));
            shadow.transform.eulerAngles = new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(target.x - shadowOrigin.x, -(target.y - shadowOrigin.y)) - 90));
        }

        void OnTriggerStay2D(Collider2D _other)
        {
            if (impact)
            {
                if (_other.tag == "Player")
                {
                    _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position, true);
                }
            }
        }
    }
}


