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
    {//collision tag list for enemy bullets (DEPRECATED) 
        if (Player.gameObject.tag == "ClusterFlak/Player")
        {
            
            Destroy(gameObject);
        }

        if (Player.gameObject.tag == "ClusterFlak/PlayerBullet")
        {
            
            Destroy(gameObject);
        }
    }
}
