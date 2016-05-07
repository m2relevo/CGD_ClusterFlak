using UnityEngine;
using System.Collections;

public class SumoMineDrop : MonoBehaviour {

	public bool isFalling;
	Vector3 startPos;
	public float dropSpeed;
	public GameObject shadow, parachute;

	// Use this for initialization
	void Start () {	
		
		resetMine ();	
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!isFalling) 
		{
			shadow.SetActive (false);
			parachute.SetActive (false);
		}

		if (isFalling)
		{
			if (this.GetComponent<SpriteRenderer> ().sortingLayerName != "Foreground") 
			{
				this.GetComponent<SpriteRenderer> ().sortingLayerName = "Foreground";
			}
			this.gameObject.GetComponent<SpriteRenderer>().enabled = true;

			shadow.SetActive (true);
			parachute.SetActive (true);
			shadow.transform.position = startPos;		
			if (transform.position != startPos) 
			{
				this.transform.position -= new Vector3 (0, dropSpeed, 0);

			} 
			if(Vector3.Distance(transform.position, startPos) < 0.01f)
			{	
				isFalling = false;
				if (this.GetComponent<SpriteRenderer> ().sortingLayerName != "Midground") 
				{
					this.GetComponent<SpriteRenderer> ().sortingLayerName = "Midground";
				}
				this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			}
		}
	
	}

	public void resetMine()
	{
		startPos = this.transform.localPosition;
		this.transform.position = startPos + new Vector3 (0, 1200, 0);
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		isFalling = false;
	}
}
