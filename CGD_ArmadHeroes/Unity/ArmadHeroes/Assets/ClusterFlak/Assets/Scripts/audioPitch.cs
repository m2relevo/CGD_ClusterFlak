using UnityEngine;
using System.Collections;

public class audioPitch : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        float pitchChange = Random.Range(1f, 2f);
        AudioSource Shot = GetComponent<AudioSource>();
        Shot.pitch = pitchChange;
        Shot.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
