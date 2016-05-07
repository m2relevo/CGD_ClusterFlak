using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace ShellShock
{
    public class Accolades : MonoBehaviour
    {

        //public List<GameObject> statTrackers = new List<GameObject>();

        //public GameObject gameTimer;

        //public Text mostElusive; //Used for when displaying the correct end of round stats
        //public Text mostTimeSatStill;
        ////public Text mostTimeBallMode;
        //public Text mostPickUps;
        //public Text mostKills;

        //public GameObject gameEndPanel;
        //public GameObject[] players;
        //public int[] playerScores;
        //private bool hasGameFinished = false;
        //public PlayerLogic[] playerProperties;

        //// Use this for initialization
        //void Start()
        //{
        //    playerScores = new int[4];

        //    players = GameObject.FindGameObjectsWithTag("Player");
        //    playerProperties = new PlayerLogic[players.Length];

        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        playerProperties[i] = players[i].GetComponent<PlayerLogic>();
        //    }

        //}

        //void Update()
        //{
        //    if (!gameTimer.GetComponent<GameTimer>().isGameRunning && !hasGameFinished)
        //    { //Checks to see if the game has stopped
        //      //BubbleSortleastDamageTaken (); // and if it has then it sorts the end of round stats and sets the corresponding texts to true
        //      //MostPickups();
        //      //            MostTimeSatStill();
        //      //            MostKills();
        //      //           // MostTimeinBallMode();
        //      //            gameEndPanel.SetActive(true);
        //      //            hasGameFinished = true;
        //        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //        for (int i = 0; i < players.Length; i++)
        //        {
        //            players[i].GetComponent<RewiredController>().SendData();
        //        }

        //        UnityEngine.SceneManagement.SceneManager.LoadScene("DebriefScene");

        //    }
        //}

        ////The reason why there are seperate functions but they all sort the seemingly same game objects 
        ////is that each function is sorting them dependant on a specific variable within those game objects.

        ////In this instance the game objects which are being sorted is the list of "stat trackers" in statTrackers

        //void BubbleSortleastDamageTaken() //Sorts damage in order from least to most across all the players. 

        ////Please refer to an earlier comment if confused about re-using of code.

        ////To change the output from least to most simply change the number in the [] at line 79 
        ////from the beginning of the list to the end of the list... that could be along the lines of .size()...
        //{
        //    bool madeChanges;
        //    GameObject temp;
        //    int itemCount = statTrackers.Count;
        //    //Compare all the statTrackers
        //    //Compare elusiveScore
        //    do
        //    {
        //        madeChanges = false;
        //        for (int i = 0; i < itemCount - 1; i++)
        //        {
        //            //if (statTrackers[i].GetComponent<ShellShock.StatTracker>().DamageTaken > statTrackers[i + 1].GetComponent<ShellShock.StatTracker>().DamageTaken)
        //            //{
        //            //    temp = statTrackers[i + 1];
        //            //    statTrackers[i + 1] = statTrackers[i];
        //            //    statTrackers[i] = temp;
        //            //    madeChanges = true;
        //            //}

        //        }
        //    } while (madeChanges);

        //    GameObject p = statTrackers[statTrackers.Count - 1].GetComponent<ShellShock.StatTracker>().p;//This returns the player's game object attached to the specific stat tracker.
        //                                                                                                 //It is called p because it is a reference to the player and it is only used to get the player number.
        //    mostElusive.text = "Player " + p.GetComponent<PlayerLogic>().PlayerNumber.ToString() + " took the least damage";
        //}

        //void MostTimeSatStill() //Using the amount of time not moving from the stat trackers, 
        //                        //the function sorts the stat trackers again in order of smallest to biggest

        ////Please refer to an earlier comment if confused about re-using of code.
        //{
        //    bool madeChanges;
        //    //GameObject temp;
        //    int itemCount = statTrackers.Count;

        //    do
        //    {
        //        madeChanges = false;
        //        for (int i = 0; i < itemCount - 1; i++)
        //        {
        //            //if (statTrackers [i].GetComponent<ShellShock.StatTracker> ().timeSatStill > statTrackers [i + 1].GetComponent<ShellShock.StatTracker> ().timeSatStill) {
        //            //	temp = statTrackers [i+1];
        //            //	statTrackers [i+1] = statTrackers [i];
        //            //	statTrackers [i] = temp;
        //            //	madeChanges = true;

        //            //}	

        //        }
        //    }
        //    while (madeChanges);

        //    GameObject p = statTrackers[statTrackers.Count - 1].GetComponent<ShellShock.StatTracker>().p; //This returns the player's game object attached to the specific stat tracker.
        //                                                                                                  //It is called p because it is a reference to the player and it is only used to get the player number.
        //    mostTimeSatStill.text = "Player " + p.GetComponent<PlayerLogic>().PlayerNumber.ToString() + " is a squatter."; //Sets the player number to the correct number.
        //}


        //void MostPickups() // Giorgio needs to add comments here if anyone is confused about the function
        //{
        //    bool madeChanges;
        //    GameObject temp;
        //    int itemCount = statTrackers.Count;

        //    do
        //    {
        //        madeChanges = false;
        //        for (int i = 0; i < itemCount - 1; i++)
        //        {
        //            if (statTrackers[i].GetComponent<ShellShock.StatTracker>().mostPickups > statTrackers[i + 1].GetComponent<ShellShock.StatTracker>().mostPickups)
        //            {
        //                temp = statTrackers[i + 1];
        //                statTrackers[i + 1] = statTrackers[i];
        //                statTrackers[i] = temp;
        //                madeChanges = true;

        //            }

        //        }
        //    }
        //    while (madeChanges);

        //    GameObject p = statTrackers[statTrackers.Count - 1].GetComponent<ShellShock.StatTracker>().p;
        //    mostPickUps.text = "Player " + p.GetComponent<PlayerLogic>().PlayerNumber.ToString() + " picked up the most number of weapons";
        //}

        //void MostKills()
        //{
        //    bool madeChanges;
        //    GameObject temp;
        //    int itemCount = statTrackers.Count;

        //    do
        //    {
        //        madeChanges = false;
        //        for (int i = 0; i < itemCount - 1; i++)
        //        {
        //            if (statTrackers[i].GetComponent<ShellShock.StatTracker>().mostKills > statTrackers[i + 1].GetComponent<ShellShock.StatTracker>().mostKills)
        //            {
        //                temp = statTrackers[i + 1];
        //                statTrackers[i + 1] = statTrackers[i];
        //                statTrackers[i] = temp;
        //                madeChanges = true;

        //            }

        //        }
        //    }
        //    while (madeChanges);

        //    GameObject p = statTrackers[statTrackers.Count - 1].GetComponent<ShellShock.StatTracker>().p;
        //    mostKills.text = "Player " + p.GetComponent<PlayerLogic>().PlayerNumber.ToString() + " killed the most enemies";
        //}

        ///* void MostTimeinBallMode()
        // {
        //     bool madeChanges;
        //     GameObject temp;
        //     int itemCount = statTrackers.Count;

        //     do
        //     {
        //         madeChanges = false;
        //         for (int i = 0; i < itemCount - 1; i++)
        //         {
        //             if (statTrackers[i].GetComponent<ShellShock.StatTracker>().mostKills > statTrackers[i + 1].GetComponent<ShellShock.StatTracker>().mostKills)
        //             {
        //                 temp = statTrackers[i + 1];
        //                 statTrackers[i + 1] = statTrackers[i];
        //                 statTrackers[i] = temp;
        //                 madeChanges = true;

        //             }

        //         }
        //     }
        //     while (madeChanges);

        //     GameObject p = statTrackers[statTrackers.Count].GetComponent<ShellShock.StatTracker>().p;
        //     mostTimeBallMode.text = "Player " + p.GetComponent<PlayerLogic>().PlayerNumber.ToString() + " BALL MODE";
        // }*/

    }
}
