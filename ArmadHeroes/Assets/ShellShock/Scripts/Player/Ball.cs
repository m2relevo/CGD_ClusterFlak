using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class Ball : MonoBehaviour
    {

        public GameObject player;
        private SpriteRenderer ballRenderer;
        private Collider2D ballCollider;
        private Rigidbody2D rb;

        private AudioSource ballHumAudio;

        void Start()
        {
            player = transform.parent.gameObject;
            ballCollider = GetComponent<Collider2D>();
            ballRenderer = GetComponent<SpriteRenderer>();
            rb = player.GetComponentInParent<Rigidbody2D>();
            ballCollider.enabled = false;
            ballRenderer.enabled = false;
            ballHumAudio = GetComponent<AudioSource>();
            ballHumAudio.volume = 0;
        }

        public void BallRender()
        {
            if (player.GetComponent<RewiredController>().isBallin == true)
            {
                //add a manual force once the player is in ball mode
               // rb.MovePosition(new Vector2(player.transform.position.x + 0.005f, player.transform.position.y));
                float tCount = 0.0f;
                tCount += Time.deltaTime;
               // rb.drag = 8.5f;
               // rb.drag = 1000f;
                rb.isKinematic = true;
                
                // rb.velocity = new Vector3(0, 0, 0);
                //rb.velocity = rb.AddForce(new Vector2(rb.drag);
                //  if (tCount >= 3.0f)
                // {
                ballCollider.enabled = true;
                //ballRenderer.enabled = true;
                // }
                ballHumAudio.volume = .6f;
            }

            if (player.GetComponent<RewiredController>().isBallin == false)
            {
               // rb.drag = 100;
                rb.isKinematic = false;
                //rb.AddForce(new Vector2(player.transform.position.x, rb.drag));
                // rb.AddForce(new Vector2(rb.drag, player.transform.position.y));
                ballCollider.enabled = false;
                //ballRenderer.enabled = false;
                ballHumAudio.volume = 0;
            }
        }
    }
}
