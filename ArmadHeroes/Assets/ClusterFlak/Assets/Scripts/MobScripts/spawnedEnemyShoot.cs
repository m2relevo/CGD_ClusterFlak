using UnityEngine;
using System.Collections;

public class spawnedEnemyShoot : MonoBehaviour {
    float ellapsed = 3;
    public GameObject Missile;

	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        ellapsed -= Time.deltaTime;
        transform.rotation = Quaternion.identity;
        if (ellapsed < 0)
        {
            fireMissile();
        }
    }  

    void fireMissile()
    {
        Instantiate(Missile, transform.position, Quaternion.identity);
        ellapsed = 2;  
    }
}
