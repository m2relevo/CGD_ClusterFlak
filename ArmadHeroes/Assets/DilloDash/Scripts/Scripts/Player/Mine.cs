using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

    public GameObject explosion; 
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            Destroy((GameObject)Instantiate(explosion, gameObject.transform.position, Quaternion.identity), 0.75f);
            Destroy(gameObject, 0.0f);
        }
    }
}
