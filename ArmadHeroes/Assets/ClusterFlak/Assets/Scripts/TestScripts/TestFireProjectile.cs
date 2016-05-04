using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestFireProjectile : MonoBehaviour
{
    public List<GameObject> ListTarget;
    public GameObject bulletdeath;
    public GameObject bulletlocation;
	// Use this for initialization
	void Start ()
    {
        float spread = Random.Range(-1f, 1f);
        

        //GameObject.FindGameObjectWithTag("ClusterFlak/Player");

        ListTarget.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
        int RNG = Random.Range(0, ListTarget.Count);
        Debug.Log(ListTarget.Count);

        GameObject Target = (GameObject)ListTarget[RNG];

        transform.right = (Target.transform.position) - transform.position + (new Vector3(spread, 0f, 0f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 10, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag == "ClusterFlak/Player")
        {
            Debug.Log("player Hit");
            GameObject rockdeath = (GameObject)Instantiate(bulletdeath);
            rockdeath.transform.position = bulletlocation.transform.position;
            Destroy(gameObject);
        }

        if (Player.gameObject.tag == "ClusterFlak/PlayerBullet")
        {
            Debug.Log("player bullet hit");
            GameObject rockdeath = (GameObject)Instantiate(bulletdeath);
            rockdeath.transform.position = bulletlocation.transform.position;
            Destroy(gameObject);
        }
    }
}