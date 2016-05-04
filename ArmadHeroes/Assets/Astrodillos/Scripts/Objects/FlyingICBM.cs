using UnityEngine;
using System.Collections;
using ArmadHeroes;
using DG.Tweening;


namespace Astrodillos{
	public class FlyingICBM : MonoBehaviour {


		public GameObject shadow, end, weaponDrop, weaponPickup;
		public SpriteRenderer shadowSprite;
		public SpriteRenderer spriteRenderer;
		public SpriteRenderer flames;
		public Rigidbody2D body;
		public Collider2D col;
		public AudioClip explodeSfx, flyingSfx;

		private AudioSource flyingSource;
        private int jetCounter;
        private int jetMax;

		//When the asteroid comes on screen. Destroys when this is true but not on screen anymore
		bool beenVisible = false;
		bool useGravity = false;
		float damage = 20.0f;
        float aliveTime = 20;


		// Use this for initialization
		void Awake () 
        {
            jetMax = GlobalPlayerManager.instance.playerData.Length;
            jetMax += jetMax/2;
  		}

		public void Spawn(Vector3 spawnPos, float speed, bool rotateClockwise, bool _useGravity){
            aliveTime = 20;
			//Reset shadow
			useGravity = _useGravity;
			shadow.SetActive (useGravity);
			col.enabled = !useGravity;

			float angle = 0;

			if (useGravity) { //Ground asteroid
				shadowSprite.color = new Color(0,0,0,0);
				Vector2 dropPoint = Gametype_Astrodillos.instance.GetDropPoint ();
				shadow.transform.position = dropPoint; 
				Vector2 offset = shadow.transform.position - spawnPos;
				angle = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg;
				spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Astrodillos/Sky");
				spriteRenderer.sortingOrder = SpriteOrdering.GetOrder(shadow.transform.position.y);
				shadowSprite.sortingOrder = spriteRenderer.sortingOrder - 2;
			} else { //Space asteroid
				//Use the offset from the centre to work out angle and speed
				angle = Mathf.Atan2 (spawnPos.x, -spawnPos.y) * Mathf.Rad2Deg;
				//Add random offset
				angle += 90 + Random.Range (-40, 40);

				spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Astrodillos/Bottom");
			}

			flames.sortingOrder = spriteRenderer.sortingOrder - 1;

			spriteRenderer.gameObject.transform.eulerAngles = new Vector3 (0, 0, angle-90);

			//Shadow scale

			float shadowScale = Mathf.Abs(90 - spriteRenderer.gameObject.transform.eulerAngles.z%180);
			shadowScale = 6 - (shadowScale / 18);

			shadow.transform.localScale = new Vector3 (shadowScale, 2, 1);

			//Back to radians
			angle *= Mathf.Deg2Rad;

			//Set position
			spriteRenderer.gameObject.transform.position = spawnPos;


			//Speed vector from angle
			Vector2 force = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle))*speed;


			//Add the force
			body.AddForce (force);


			//Play flying sound
			flyingSource = SoundManager.instance.PlayClip(flyingSfx, transform.position, true, 0);
			flyingSource.DOFade (1.0f, 1.0f);
		}
		

		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}

			

			if (shadow.activeSelf) {
				SoundManager.instance.SetPan (transform.position, flyingSource);
				shadow.transform.position = new Vector3(spriteRenderer.gameObject.transform.position.x, shadow.transform.position.y, 0);
				float distance = Mathf.Abs(end.transform.position.y - shadow.transform.position.y);

				spriteRenderer.gameObject.layer = SpriteOrdering.CollsionLayerFromHeight(distance);


				float alpha = 1/(distance);
				alpha = Mathf.Min(alpha, 0.5f);
				shadowSprite.color = new Color(0,0,0,alpha);

				if(distance<0.1f){
					Explode(shadow.transform.position);
				}
			}

            if (aliveTime > 0)
            {
                aliveTime -= Time.deltaTime;
                if (aliveTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
			
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.attachedRigidbody != null) {
				other.attachedRigidbody.AddForceAtPosition (body.velocity, transform.position);
			}

				
			Explode (spriteRenderer.transform.position);

			if(other.gameObject.GetComponent<Actor_Armad>()){
				other.gameObject.GetComponent<Actor_Armad>().TakeDamage(damage);
			}


		}

		public void Reset(){
			if (flyingSource != null) {
				flyingSource.Stop ();
			}
		}

		void Explode(Vector3 position){
			//Stop audio
			flyingSource.Stop ();

			//Create a jetpack drop
			if (Gametype_Astrodillos.instance.UseGravity ()) 
            {
                JetpackOrWeapon();
			}
            else
            {
                GameObject weapon = GameObject.Instantiate(weaponDrop);
                weapon.transform.position = position;
                weapon.gameObject.SetActive(true);
            }
			SoundManager.instance.PlayClip (explodeSfx, end.transform.position);
			gameObject.SetActive(false);


			float areaOfEffect = useGravity ? 1 : 0;
			Gametype_Astrodillos.instance.Explosion (end.transform.position, false, areaOfEffect);
			beenVisible = false;
		}
        void JetpackOrWeapon()
        {
            
                float rand = Random.value;
                if (jetCounter<jetMax&& rand>0.7||jetCounter==0)
                {
                    JetpackManager.instance.SpawnPickup(end.transform.position);
                    jetCounter++;
                }
                else
                {
                    GameObject pickUp = GameObject.Instantiate(weaponPickup);
                    pickUp.transform.position = end.transform.position;
                }
             
        }

	}
}
