using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class Crate : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                coll.gameObject.GetComponent<ShellShock.RewiredController>().ChangeWeapon();
                Destroy(gameObject.transform.parent.transform.parent.gameObject);
            }
        }
    }
}
