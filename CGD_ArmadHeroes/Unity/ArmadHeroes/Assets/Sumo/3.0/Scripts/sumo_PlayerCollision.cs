using UnityEngine;

namespace sumo 
{
    public class psumo_PlayerCollision : MonoBehaviour
    {

		void OnCollisionEnter2D(Collision2D collision)
		{
			Debug.Log("Collision");
			sumo_CustomCollisions.instance.collisionID += 1;
			if ((sumo_CustomCollisions.instance.collisionID % 2) == 0)
			{
                sumo_CustomCollisions.instance.collision(this.GetComponentInParent<GameObject>(), collision.gameObject.GetComponentInParent<GameObject>(), sumo_CustomCollisions.instance.collisionID, collision.contacts[0]);
			}
		}
	}
}

