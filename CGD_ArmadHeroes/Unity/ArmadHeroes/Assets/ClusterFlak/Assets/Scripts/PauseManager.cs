using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {
    public bool pausebool;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (pausebool)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
	}
}
