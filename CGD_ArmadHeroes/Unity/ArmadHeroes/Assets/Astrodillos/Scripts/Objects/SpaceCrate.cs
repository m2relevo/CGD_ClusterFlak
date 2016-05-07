using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos
{
    public class SpaceCrate : MonoBehaviour
    {
        public GameObject getWeapon;
        public Rigidbody2D body;
        // Use this for initialization
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }
      void Awake()
        {
            Vector2 randForce = new Vector2(Random.Range(-20, 20), Random.Range(-10, 10));
            body.AddForceAtPosition(randForce, transform.position);
        }
        // Update is called once per frame
        void Update()
        {
           
        }
        void OnTriggerEnter2D(Collider2D col)
        {

            if (col.GetComponent<Astro_Projectile>() || col.GetComponent<Weapon_Flamethrower>())
            {
               
                GameObject inst = GameObject.Instantiate(getWeapon);
                inst.gameObject.transform.position = transform.position;

                Gametype_Astrodillos.instance.Explosion(transform.position);
                Destroy(gameObject);

            }
           
        }
    }
}