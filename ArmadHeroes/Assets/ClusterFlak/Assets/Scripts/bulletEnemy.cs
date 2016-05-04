using UnityEngine;
using System.Collections;

public class bulletEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag == "ClusterFlak/Player")
        {
            Debug.Log("player Hit");
            Destroy(gameObject);
        }

        if (Player.gameObject.tag == "ClusterFlak/PlayerBullet")
        {
            Debug.Log("player bullet hit");
            Destroy(gameObject);
        }
    }
}
