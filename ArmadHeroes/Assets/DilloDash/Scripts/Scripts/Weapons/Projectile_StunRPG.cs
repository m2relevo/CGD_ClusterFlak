using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace DilloDash
{

    public class Projectile_StunRPG : ArmadHeroes.Projectile
    {
        protected Rigidbody2D rb;
        protected bool objectHit = false;
        protected bool hasExploded = false;
        private float triggerRadius;
        public float explosionRadius = 2;
        [SerializeField] protected GameObject nose;
        protected CircleCollider2D myCollider;

        // Use this for initialization
        protected virtual void Awake()
        {
            myCollider = GetComponent<CircleCollider2D>();
            triggerRadius = myCollider.radius;
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public override void Fire(int _playerId, float height = 0, Collider2D _ignoreCollider = null)
        {
            gameObject.SetActive(true);
            myCollider.radius = triggerRadius;
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(-direction.y, direction.x, direction.z));
            rb.velocity += owner.GetComponent<Rigidbody2D>().velocity;
            callerID = _playerId;
            objectHit = false;
            hasExploded = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                rb.velocity += (Vector2)(nose.transform.position - gameObject.transform.position);
                if (hasExploded)
                {
                    GameObject _explosion = Armatillery.ExplosionManager.instance.GetExplosion();
                    _explosion.transform.position = transform.position;
                    _explosion.SetActive(true);
                    _explosion.GetComponent<Explosion>().InitExplode(0.5f);
                    _explosion.GetComponent<ParticleSystem>().startSize = 4.0f;
                    gameObject.SetActive(false);
                }
                if (objectHit)
                {
                    hasExploded = true;
                }
            }

        }        

        protected void OnTriggerStay2D(Collider2D _other)
        {
            if (hasExploded)
            {
                if (_other.tag == "Player")
                {
                    _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position,true);
                }
            }
        }
        
        protected void OnTriggerEnter2D(Collider2D _other)
        {
            if (!objectHit)
            {
                if (_other.tag == "Player")
                {
                    if (_other.GetComponent<ArmadHeroes.PlayerActor>().playerNumber != callerID)
                    {
                        objectHit = true;
                        myCollider.radius = explosionRadius;
                    }
                }
                else if (_other.tag == "DilloDash/Decor")
                {
                    objectHit = true;
                    myCollider.radius = explosionRadius;
                }
            }
        }
    }
}