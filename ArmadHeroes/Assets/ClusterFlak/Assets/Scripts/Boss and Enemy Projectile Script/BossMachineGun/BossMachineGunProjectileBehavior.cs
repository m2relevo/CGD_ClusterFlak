﻿using UnityEngine;
using System.Collections;

public class BossMachineGunProjectileBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 10, Space.Self);
    }
}
