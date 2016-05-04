using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{

    public class Projectile_Mine : Projectile
    {
        private bool triggered = false;
        protected bool detonate = false;
        public float detonationLength= 0.25f;
        private float triggerRadius;
        public float explosionRadius = 2;
        private CircleCollider2D myCollider;
        
        private float detonationCounter = 0.0f;

        protected virtual void Awake()
        {
            myCollider = GetComponent<CircleCollider2D>();
            triggerRadius = myCollider.radius;
            
        }

        public override void Fire(int _playerId, float height = 0, Collider2D _ignoreCollider = null)
        {
            gameObject.SetActive(true);
            myCollider.radius = triggerRadius;        
            callerID = _playerId;
            detonationCounter = 0;
            triggered = false;
            detonate = false;
        }     
          
        protected override void Update()
        {
            if (detonate)
            {
                GameObject _explosion = Armatillery.ExplosionManager.instance.GetExplosion();
                _explosion.transform.position = transform.position;
                _explosion.SetActive(true);
                _explosion.GetComponent<Explosion>().InitExplode(0.5f);
                _explosion.GetComponent<ParticleSystem>().startSize = 4.0f;
                gameObject.SetActive(false);
            }
            if (triggered)
            {
                detonationCounter += Time.deltaTime;
                
                if (detonationCounter >= detonationLength)
                {
                    detonate = true;
                    triggered = false;
                    
                }               
            }            
        }

        protected virtual void OnTriggerEnter2D(Collider2D _other)
        {
            if (!triggered && !detonate)
            {
                if (_other.tag == "Player")
                {
                    if (_other.GetComponent<PlayerActor>().playerNumber != callerID)
                    {
                        triggered = true;
                        myCollider.radius = explosionRadius;
                    }
                }
            }
        }

        protected virtual void OnTriggerStay2D (Collider2D _other)
        {
            if (detonate)
            {
                //Inherit from this and do your own explosion logic eg, call a stun function on player.
            }
        }
    }
}