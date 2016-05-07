using UnityEngine;
using System.Collections;
//DEPRECATED player code pre-integration for bullets 
public class bulletPlayer : MonoBehaviour {
    Vector2 maxCam;
    Vector2 minCam;
    // Use this for initialization
    void Start () {
        maxCam = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        minCam = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > maxCam.y || transform.position.y < minCam.y)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.x > maxCam.x || transform.position.x < minCam.x)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D Enemy)
    {
         if (Enemy.gameObject.tag == "ClusterFlak/Enemy")
         {
             
             Destroy(gameObject);
         }

         if (Enemy.gameObject.tag == "ClusterFlak/EnemyBullet")
         {
             
             Destroy(gameObject);
         }
    }
}