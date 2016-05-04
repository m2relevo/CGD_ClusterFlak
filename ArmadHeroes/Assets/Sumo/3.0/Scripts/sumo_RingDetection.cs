using UnityEngine;
using System.Collections;

namespace sumo
{
    public class sumo_RingDetection : MonoBehaviour
    {

        public bool killOnExit;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!killOnExit)
            {
                if (collider.tag == "Sumo/Ground")
                {

                }
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (killOnExit)
            {           
                if (collider.tag == "Sumo/Ground")
                {
                    if (transform.parent.gameObject.GetComponent<sumo_PlayerLives>().lives != 0)
                    {
                        transform.parent.gameObject.GetComponent<sumo_PlayerLives>().LoseLife();
                        if (this.transform.parent.gameObject.GetComponent<sumo_PlayerLives>().lives > 0)
                        {
                            {
                                transform.parent.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }      
    }
}
