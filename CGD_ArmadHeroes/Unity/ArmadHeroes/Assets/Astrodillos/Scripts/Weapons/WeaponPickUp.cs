using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ArmadHeroes;

namespace Astrodillos
{
    public class WeaponPickUp : MonoBehaviour
    {
        private Vector2 dropPoint;
        public GameObject weapon;
        public GameObject shadow;
        public GameObject sandParticles;
        public AudioClip dropped;
		SpriteRenderer boxRenderer, shadowRenderer;
        Rigidbody2D body;
        Tweener dropTween;
        bool once = false;
        bool hasPlayed = false;
       
        // Use this for initialization
        void Awake()
        {
          body = GetComponent<Rigidbody2D>();
			boxRenderer = weapon.GetComponent<SpriteRenderer> ();
			shadowRenderer = shadow.GetComponent<SpriteRenderer> ();
        }

        // Update is called once per frame
        void Update()
        {
            DropItLikeItsHot();

            if (!hasPlayed)
            {
                sandParticles.transform.position = dropPoint;
            }

        }
       
       
       public void SetDropPoint(Vector2 _drop)
        {
            dropPoint = _drop;
            shadow.transform.position = new Vector2(dropPoint.x,dropPoint.y-0.2f);
			boxRenderer.sortingOrder = SpriteOrdering.GetOrder (dropPoint.y);
			shadowRenderer.sortingOrder = SpriteOrdering.GetOrder (shadow.transform.position.y);
            shadow.transform.SetParent(null);
        }
        void DropItLikeItsHot()
        {
           dropTween = body.DOMove(dropPoint, 0.3f);
           dropTween.OnComplete(() =>
               {
                   if (!hasPlayed&&!sandParticles.GetComponent<ParticleSystem>().isPlaying)
                   {
                       sandParticles.GetComponent<ParticleSystem>().Play();
                       hasPlayed = true;
                       SoundManager.instance.PlayClip(dropped, transform.position, false, 1);
                   }
                
                 SpawnAndFade();
               });
        }
        void SpawnAndFade()
        {
            
          
            if (transform.position == new Vector3(dropPoint.x, dropPoint.y, 0))
            {
               if (!once)
                {
                   
                    GameObject spawned = null;
                    if (spawned == null)
                    {
                        spawned = GameObject.Instantiate(weapon);
                        spawned.transform.position = transform.position;
                    }
                    shadow.GetComponent<SpriteRenderer>().DOFade(0, 1);
                    Tweener fade = gameObject.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);
                    fade.OnComplete(() =>
                    {
                        Destroy(shadow);
                        Destroy(gameObject);
                    });
                    once = true;
                }
            }
        }
    }
}