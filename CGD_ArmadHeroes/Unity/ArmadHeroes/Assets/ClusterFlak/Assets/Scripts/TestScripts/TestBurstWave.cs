using UnityEngine;
using System.Collections;

public class TestBurstWave : MonoBehaviour
{
    //DEPRECATED
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 6, Space.Self);
    }
}
