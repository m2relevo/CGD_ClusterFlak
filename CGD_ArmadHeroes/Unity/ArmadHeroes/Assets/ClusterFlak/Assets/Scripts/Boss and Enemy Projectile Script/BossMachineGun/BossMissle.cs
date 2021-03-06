﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

public class BossMissle : MonoBehaviour
{
    public List<GameObject> ListTarget;
    public float MissileSpeed;
    public GameObject rockexplosion;
    public GameObject rocklocation;
    private GameObject hometarget;
    
    // Use this for initialization
    void Start ()
    {
        //creates a list and randomly chooses a player from that list 
        ListTarget.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
        int RNG = Random.Range(0, ListTarget.Count);
        

        GameObject Target = (GameObject)ListTarget[RNG];
        hometarget = Target;
        transform.right = (Target.transform.position) - transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {//code to make the missiles home in on the player
        if (hometarget != null)
        {
            transform.right = hometarget.transform.position - transform.position;

            transform.Translate(Vector3.right * Time.deltaTime * MissileSpeed, Space.Self);
        }

        if (hometarget == null)
        {
            GameObject rockdeath = (GameObject)Instantiate(rockexplosion);
            rockdeath.transform.position = rocklocation.transform.position;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D Player)
    {
        //code that detonates the object on player and player projectile hits, and spawns an explosion particle effect
            if (Player.gameObject.tag == "ClusterFlak/Player")
            {
             
                GameObject rockdeath = (GameObject)Instantiate(rockexplosion);
                rockdeath.transform.position = rocklocation.transform.position;
                Destroy(gameObject);
                
            }


            if (Player.gameObject.GetComponent<Projectile>())
            {
                
                GameObject rockdeath = (GameObject)Instantiate(rockexplosion);
                rockdeath.transform.position = rocklocation.transform.position;
                Destroy(gameObject);
                
            }
        
    }
}
