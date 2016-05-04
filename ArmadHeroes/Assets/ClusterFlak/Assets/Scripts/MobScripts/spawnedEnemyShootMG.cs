using UnityEngine;
using System.Collections;

public class spawnedEnemyShootMG : MonoBehaviour {
    float ellapsed = 3;
    public GameObject MG;

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
            fireMG();
        }
    }  

    void fireMG()
    {
        Instantiate(MG, transform.position, Quaternion.identity);
        ellapsed = 1f;  
    }
}
