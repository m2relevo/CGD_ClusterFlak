using UnityEngine;
using System.Collections;
//apache move code
public class ApacheMove : MonoBehaviour {
    float apachespeed = 0.15f;
    int frequency = 3;
    private Vector3 node;
    // Use this for initialization
    void Start () {
        InvokeRepeating("SetDestination", 1, frequency);
    }
	
	// Update is called once per frame
	void Update () {//moves towards node set by SetDestnation()
        transform.position = Vector3.MoveTowards(transform.position, node, apachespeed);
    }

    void SetDestination ()
    {//randomly creates a node on the map between a certain range 
        int xcoord = Random.Range(-9, 10);
        int ycoord = Random.Range(8, 1);
        node = new Vector3(xcoord, ycoord, 0);
        
    }
}
