using UnityEngine;
using System.Collections;

public class TestMissleProjectile : MonoBehaviour {
    public float speed = 5; 
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject Target = GameObject.FindGameObjectWithTag("ClusterFlak/Player");
        transform.right = Target.transform.position - transform.position;

        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
    }
}
