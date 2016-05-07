using UnityEngine;
using System.Collections;

namespace ZonePatrol
{
    public class Items : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.EquipeWeaponByType(WeaponsType.Machinegun);
                Object.Destroy(this.gameObject);
            }
        }
    }
}
