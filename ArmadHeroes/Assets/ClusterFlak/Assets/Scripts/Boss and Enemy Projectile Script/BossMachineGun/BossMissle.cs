using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMissle : MonoBehaviour
{
    public List<GameObject> ListTarget;
    public float MissileSpeed;
    public GameObject rockexplosion;
    public GameObject rocklocation;
    private GameObject hometarget;
    
    // Use this for initialization
    void Start ()
    {
        float spread = Random.Range(-1f, 1f);


        //GameObject.FindGameObjectWithTag("ClusterFlak/Player");

        ListTarget.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
        int RNG = Random.Range(0, ListTarget.Count);
        Debug.Log(ListTarget.Count);

        GameObject Target = (GameObject)ListTarget[RNG];
        hometarget = Target;
        transform.right = (Target.transform.position) - transform.position + (new Vector3(spread, 0f, 0f));
    }
	
	// Update is called once per frame
	void Update ()
    {
       // GameObject Target = GameObject.FindGameObjectWithTag("ClusterFlak/Player");
        transform.right = hometarget.transform.position - transform.position;

        transform.Translate(Vector3.right * Time.deltaTime * MissileSpeed, Space.Self);


    }

    void OnTriggerEnter2D(Collider2D Player)
    {
        
            if (Player.gameObject.tag == "ClusterFlak/Player")
            {
                Debug.Log("player Hit");

                GameObject rockdeath = (GameObject)Instantiate(rockexplosion);
                rockdeath.transform.position = rocklocation.transform.position;
                Destroy(gameObject);
                
            }

            if (Player.gameObject.tag == "ClusterFlak/PlayerBullet")
            {
                Debug.Log("player bullet hit");
                GameObject rockdeath = (GameObject)Instantiate(rockexplosion);
                rockdeath.transform.position = rocklocation.transform.position;
                Destroy(gameObject);
                
            }
        
    }
}
