using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol
{
    public class TurretManager : MonoBehaviour {
        //GameObject Turret;
        private bool BuildTurret;

        void Start (){
            //Turret = new GameObject("Turret");
            //Turret.AddComponent<Rigidbody>();
            //Turret.AddComponent<BoxCollider>();
            //Turret.active = false;
    	}
    	
    	void Update () {
            BuildTurret = gameObject.GetComponent<CapturePoint>().buildTurret;
            if (BuildTurret == true)
            {
                //Turret.active = true;            
            }
            else if (BuildTurret == false)
            {
                //Turret.active = false;
            }
            if (BuildTurret == true)//Turret.active == true)
            {
                //shoot
                Debug.Log("built and shooting");
            }
    	}
    }
}
