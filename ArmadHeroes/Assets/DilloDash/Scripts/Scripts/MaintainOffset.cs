using UnityEngine;
using System.Collections;

public class MaintainOffset : MonoBehaviour {

    private Vector3 offset;

	// Use this for initialization
	void Awake () {

        offset = transform.localPosition;
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = (transform.root.position + offset);
	
	}
}
