using UnityEngine;
using System.Collections;

public class ApacheMove : MonoBehaviour {
    float apachespeed = 0.15f;
    int frequency = 3;
    private Vector3 node;
    // Use this for initialization
    void Start () {
        InvokeRepeating("SetDestination", 1, frequency);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, node, apachespeed);
    }

    void SetDestination ()
    {
        int xcoord = Random.Range(-9, 10);
        int ycoord = Random.Range(8, 1);
        node = new Vector3(xcoord, ycoord, 0);
        
    }
}
