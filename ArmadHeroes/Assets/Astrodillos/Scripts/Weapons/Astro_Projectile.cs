using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos
{

    public class Astro_Projectile : MonoBehaviour
    {
		public bool explodeOnImpact = false;
		public AudioClip impactSfx;
		protected ScreenWrap screenWrap;

		Projectile projectile;

        protected bool useGravity {
			private set{ }
			get { return Gametype_Astrodillos.instance.UseGravity(); }
		}

            
        // Use this for initialization
        public virtual void Awake()
        {
			screenWrap = GetComponent<ScreenWrap> ();

			projectile = GetComponent<Projectile>();
			if (projectile && projectile.shadow) {
				projectile.SetShadowActive(useGravity);

			}
        }

		void OnEnable(){
			screenWrap.enabled = !useGravity;
		}

        protected virtual void HitObject(GameObject hitObject)
        {
            //if (hitObject.tag == "Player")
            //{
			//	Actor_Armad hitActor = hitObject.GetComponent<Actor_Armad> ();
			//	hitActor.TakeDamage(projectile.damage);
            //}

			if (impactSfx != null) {
				SoundManager.instance.PlayClip (impactSfx);
			}
           	
			if (explodeOnImpact) {
				Vector3 direction = hitObject.transform.position - transform.position;
				direction.Normalize();
				Vector3 explosionPos = transform.position + (direction * 0.25f);

				Gametype_Astrodillos.instance.Explosion(explosionPos);
			}
            
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
				
            //If not ignore collider (player)
			if (col.usedByEffector!=true)
            {
				if (col.tag != "Astrodillos/ActorOnly" && col.tag != "Bullet")
                {
					if (!screenWrap.enabled || !screenWrap.hasWrapped) {
						if (col.tag == "Player" && col.GetComponentInParent<Actor_Armad>(). playerNumber == projectile.callerID) {
							return;
						}
					}

					HitObject(col.gameObject);
                    gameObject.SetActive(false);
            

				}
            
            }
        }
    }
}