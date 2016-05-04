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

    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag == "ClusterFlak/Enemy")
        {
            Debug.Log("enemy Hit");
            Destroy(gameObject);
        }

        if (Player.gameObject.tag == "ClusterFlak/EnemyBullet")
        {
            Debug.Log("enemy bullet hit");
            Destroy(gameObject);
        }
    }
}
