using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos
{
    public class ExplodableObject : MonoBehaviour
    {
		public AudioClip explodeSfx;

        private float health = 30;
		private SpriteRenderer spriteRenderer;
       
		void Awake(){
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}
        void Explode()
        {
			if (Gametype_Astrodillos.instance != null) {
				Gametype_Astrodillos.instance.Explosion (transform.position,false,1.5f);
			}

			if (explodeSfx != null) {
				SoundManager.instance.PlayClip (explodeSfx);
			}
			Destroy (gameObject);
			health = 0;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
      
			if (health <= 0) {
				Explode ();
			} else {
				float strength = (health / 30);
				spriteRenderer.color = new Color (1, strength, strength, 1);
			}
        }

		public virtual void OnTriggerEnter2D(Collider2D col)
		{
			if (health <= 0) {
				return;
			}
			//check if col is a projectile
			if (col.gameObject.GetComponent<Projectile>())
			{
				Projectile _projectile = col.gameObject.GetComponent<Projectile>();

				TakeDamage(_projectile.damage);

			}

			if (col.gameObject.GetComponent<Weapon_Flamethrower> ()) {

				Explode ();
			}
		}
            
    }
}