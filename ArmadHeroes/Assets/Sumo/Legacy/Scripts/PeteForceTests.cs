using UnityEngine;
using System.Collections;

public class PeteForceTests : MonoBehaviour {

    public Vector2 force, move;
    public float forceMulti;


    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
        }	
	}

    void FixedUpdate()
    {

        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(move);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(-move);
        }

        if (Input.GetMouseButtonDown(0))
        { 
            this.GetComponent<Rigidbody2D>().AddForce(force * forceMulti);            
        }        
    }

}
