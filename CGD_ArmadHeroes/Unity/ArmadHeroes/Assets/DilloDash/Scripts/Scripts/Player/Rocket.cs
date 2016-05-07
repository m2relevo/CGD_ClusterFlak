using UnityEngine;
using System.Collections;
namespace DilloDash
{
    public class Rocket : MonoBehaviour
    {
        Rigidbody2D rb;
        public GameObject nose;
        public GameObject explosion;
        // Use this for initialization
        void Start()
        {

            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
        }
        void Movement()
        {
            rb.velocity += (Vector2)(nose.transform.position - gameObject.transform.position);
        }
        void OnCollisionEnter2D(Collision2D coll)
        {
            explosion.transform.localScale = gameObject.transform.localScale;
            Instantiate(explosion, nose.transform.position, Quaternion.identity);
        }
    }
}