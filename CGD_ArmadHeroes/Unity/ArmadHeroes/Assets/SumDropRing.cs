using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SumDropRing : MonoBehaviour {

	List<GameObject> childMines = new List<GameObject>();

	// Use this for initialization
	void Start () {

		for(int i = 0; i < gameObject.transform.childCount; i++)
		{
			childMines.Add(gameObject.transform.GetChild(i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void drop()
	{
		foreach (GameObject mine in childMines)
		{
			mine.GetComponent<SumoMineDrop> ().isFalling = true;
		}
	}

	public void reset()
	{
		foreach (GameObject mine in childMines)
		{
			mine.GetComponent<SumoMineDrop> ().resetMine ();
		}
	}
		
}
