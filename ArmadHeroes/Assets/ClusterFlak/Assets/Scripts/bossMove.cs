using UnityEngine;
using System.Collections;

public class bossMove : MonoBehaviour {
	float speed = 0.05f;
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x < .80)
        {
         pos = new Vector2(pos.x + speed, pos.y);
         transform.position = pos;
        }
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Finish") 
		{	
			speed = 0.05f;
		}
		if (col.gameObject.tag == "Respawn") 
		{
			speed = -0.05f;
		}
	}
}
