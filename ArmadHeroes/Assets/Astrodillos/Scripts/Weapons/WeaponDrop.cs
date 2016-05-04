using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos{
	public class WeaponDrop : MonoBehaviour
	{

	    public Sprite MachineGun;
	    public Sprite Shotgun;
	    public Sprite Laser;
	    public Sprite Flamethrower;
        Weapon weapon;
	    public int NumberOfWeapons;

		private SpriteRenderer shadow;

	    private int WeaponType;

	    private float timeleft = 10.0f;

		void Awake(){
			shadow = GetComponent<SpriteRenderer> ();
		}

	    // Use this for initialization
	    void Start()
	    {
			shadow.enabled = Gametype_Astrodillos.instance.UseGravity (); 

			

	        //on spawn choose random weapon type and assign appropriate sprite and type
	        ChooseRandomWeapon();
	        
	    }

	    // Update is called once per frame
	    void Update()
	    {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
	        //do graphical effect updates here

	        //few second timer to destroy prefab
	        KillTimer();
	    }

	    void ChooseRandomWeapon()
	    {
	        // Randomly choose weapon type
	        WeaponType = Random.Range(0, NumberOfWeapons);
	        
	        //Assign weapon sprite and name to gameobject
	        if (WeaponType == 0)
	        {
	            GetComponent<SpriteRenderer>().sprite = MachineGun;
	            gameObject.transform.parent.name = "Weapon: Machine Gun";
	        }
	        else if (WeaponType == 1)
	        {
	            GetComponent<SpriteRenderer>().sprite = Shotgun;
	            gameObject.transform.parent.name = "Weapon: Shotgun";
	        }
	        else if (WeaponType == 2)
	        {
	            GetComponent<SpriteRenderer>().sprite = Laser;
	            gameObject.transform.parent.name = "Weapon: Laser";
	        }
	        else if (WeaponType == 3)
	        {
	            GetComponent<SpriteRenderer>().sprite = Flamethrower;
	            gameObject.transform.parent.name = "Weapon: Flamethrower";
	        }
	        else if (WeaponType == 4)
	        {
	            //unused atm
	            GetComponent<SpriteRenderer>().sprite = MachineGun;
	            gameObject.transform.parent.name = "Weapon: Shotgun?";
	        }
	    }

	    void KillTimer()
	    {
	        timeleft -= Time.deltaTime;

	        //delete this following if (will finish later want a flashing effect for last few seconds)
	        //if (timeleft <= 5)
	       // {
	       //     GetComponent<SpriteRenderer>().enabled = false;
	       // }

	        if(timeleft <= 0)
	        {
	            Destroy(transform.parent.gameObject);
	        }
	    }

	    void AssignWeaponToPlayer()
	    {
	        //get collided player and assign 
	    }
       
	}
}
