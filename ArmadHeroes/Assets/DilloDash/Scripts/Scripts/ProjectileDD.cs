using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class ProjectileDD : MonoBehaviour
    {
        [SerializeField] private float dropRate = 5.0f;
        private Timer dropTimer = null;
        [SerializeField] private float dropThreshold = 0.1f;
        [SerializeField] private float speed = 5.0f;

        [SerializeField] private GameObject shadow = null;

        void Awake()                                                                    
        {
            dropTimer = new Timer();
            dropTimer.QuickTimer(dropRate, DestroyProjectile);
        }
                                                                     
        void FixedUpdate()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                dropTimer.Update();
                transform.position = transform.position + (transform.up.toDirectionalIso() * Time.deltaTime * speed);
      
                shadow.transform.position = new Vector3(transform.position.x, transform.position.y + (-15.0f * (dropTimer.GetTime() / dropRate)), 0);
                
                Vector3 projRotation = transform.rotation.eulerAngles;
                shadow.transform.localEulerAngles = new Vector3(projRotation.x * -1.0f, projRotation.y * -1.0f, projRotation.z * -1.0f);
               
            }
        }

        void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        void OnTriggerStay2D(Collider2D _other)
        {
            if (_other.tag == "Player")
            {               
                if (Mathf.Abs(shadow.transform.position.y - transform.position.y) < dropThreshold)
                {                    
                    _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position);
                }


                //Logic for if a bullet hits a player
                //Need to determine if bullet is at right height thus need shadow(depth) and timer to represent how long 
                //it takes before bullet reaches ground level
            }
        }
    }
}
