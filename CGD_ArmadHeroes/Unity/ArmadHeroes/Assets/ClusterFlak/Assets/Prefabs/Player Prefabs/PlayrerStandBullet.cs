using UnityEngine;
using System.Collections;

public class PlayrerStandBullet : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.right = transform.position + (new Vector3(3f, 0f, 0f));
    }
    //basic collider script for the player's bullets colliding with enemy bullets and the enemy, deprecated
    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag == "ClusterFlak/Enemy")
        {
            
            Destroy(gameObject);
        }

        if (Player.gameObject.tag == "ClusterFlak/EnemyBullet")
        {
           
            Destroy(gameObject);
        }
    }
}
