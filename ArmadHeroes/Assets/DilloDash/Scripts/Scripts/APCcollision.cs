using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class APCcollision : MonoBehaviour
    {

       void OnCollisionEnter2D(Collision2D coll)
        {
            if(coll.gameObject.tag == "DilloDash/APC")
            {
                gameObject.GetComponent<DilloDashPlayer>().InitiateStun(coll.gameObject.transform.position);
            }
        }
    }
}