using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

public class TestFireProjectile : MonoBehaviour
{//standard tracking boss turret code
    public List<GameObject> ListTarget;
    GameObject Target = null;

    void Start ()
    {
        float spread = Random.Range(-1f, 1f);
        
        
        //creates list of enemy players and randomly picks one
        ListTarget.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
        int RNG = Random.Range(0, ListTarget.Count);

        if (GameObject.FindGameObjectWithTag("ClusterFlak/Player"))
        {
            Target = (GameObject)ListTarget[RNG];
        }

        if (GameObject.FindGameObjectWithTag("ClusterFlak/Player"))
        {
            transform.right = (Target.transform.position) - transform.position + (new Vector3(spread, 0f, 0f));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {//makes bullet continue fire movement but without homing 
        if (GameObject.FindGameObjectWithTag("ClusterFlak/Player"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 10, Space.Self);
        }
    }

    void OnTriggerEnter2D(Collider2D Player)
    {//collision code for bullets 
        if (Player.gameObject.tag == "ClusterFlak/Player")
        {
            
            Destroy(gameObject);
        }

        if (Player.gameObject.GetComponent<Projectile>())
        {
          
            Destroy(gameObject);
        }
        if (Player.gameObject.tag == "ClusterFlak/Bulletshield")
        {
            
            Destroy(gameObject);
        }
    }
}