using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class Crate : MonoBehaviour
    {
        public AudioSource cratePickupSound;

        void Start()
        {
            cratePickupSound = GameObject.FindGameObjectWithTag("ShellShock/CRATESOUND").gameObject.GetComponent<AudioSource>();
        }
        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                cratePickupSound.Play();
                coll.gameObject.GetComponent<ShellShock.RewiredController>().ChangeWeapon();
                Destroy(gameObject.transform.parent.transform.parent.gameObject);
            }
        }
    }
}
