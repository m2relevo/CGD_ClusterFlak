using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class StatTracker : MonoBehaviour
    {

        //public GameObject p; //Reference to the player game object which this script is attached to
        //public GameObject accoladeManager; //Adds the stat tracker to list of stat trackers ready to be sorted

        //Vector3 oldPos; //List of variables for seeing if something has changed between ticks
        //float oldHPValue;
        //float currentHPValue;
        //float currentAmmoValue;
        //float pickUps;
        //float numberOfKills;


        //public float DamageTaken;  //Linked to the accolades (damage taken)
        //public float timeSatStill; //Linked to the acolade, squatter
        //public float amountOfBullets; //Linked to the acolade, spray and pray
        //                              //Need another script to compare all of the different score
        //                              // Use this for initialization

        //public float mostPickups;
        //public float mostKills;

        //void Start()
        //{
        //    accoladeManager.GetComponent<ShellShock.Accolades>().statTrackers.Add(this.gameObject);
        //    //oldHPValue = p.GetComponent<ShellShock.PlayerLogic>().HPSlider.value;
        //    oldHPValue = p.GetComponent<ShellShock.PlayerLogic>().healthBar.fillAmount;
        //    //oldAmmoValue = p.GetComponent<ShellShock.PlayerLogic> ().ammoSlider.value;
        //    oldPos = new Vector3(0, 0, 0);
        //    numberOfKills = p.GetComponent<ShellShock.PlayerLogic>().playerScore;
        //}

        //// Update is called once per frame
        //void Update()
        //{
        //    //currentHPValue = p.GetComponent<ShellShock.PlayerLogic>().HPSlider.value;
        //    currentHPValue = p.GetComponent<ShellShock.PlayerLogic>().healthBar.fillAmount;
        //    //currentAmmoValue = p.GetComponent<ShellShock.PlayerLogic> ().ammoSlider.value;

        //    pickUps = p.GetComponent<ShellShock.PlayerLogic>().weaponsChanged;

        //    if (p.transform.position == oldPos) //If player position doesn't change from previous tick then player has been sat still
        //    {
        //        timeSatStill += Time.deltaTime;
        //    }
        //    oldPos = p.transform.position;

        //    if (currentAmmoValue < oldAmmoValue) //If the current ammoslider value is different to before then a bullet has been fired
        //    {
        //        //	Debug.Log ("Fired!");
        //        amountOfBullets += 1;
        //    }

        //    if (currentHPValue < oldHPValue)
        //    { //If current HPValue has changed then damage has been taken
        //      //Debug.Log ("Hit");
        //        DamageTaken++;
        //    }

        //    if (p.GetComponent<ShellShock.PlayerLogic>().weaponsChanged > pickUps) // Giorgio's
        //    {
        //        Debug.Log("PickUps");
        //        mostPickups++;
        //    }

        //    if (p.GetComponent<ShellShock.PlayerLogic>().acc_playerKilled == true) // Increase the counter if a player kills another player
        //    {
        //        Debug.Log("Kills");
        //        mostKills += 1;
        //    }
    }
}
