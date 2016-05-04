using UnityEngine;
using System.Collections;

public class AudioDed : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("ded", 2, 1);
       
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void ded()
    {
        Destroy(this.gameObject);
    }
}
