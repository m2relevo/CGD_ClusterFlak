using UnityEngine;
using System.Collections;

public class sumo_RingCloseManager : MonoBehaviour {

    /*
    public GameObject rocketOne = GameObject.Find("rocketOne");
	public GameObject rocketTwo = GameObject.Find("rocketTwo");
	public GameObject rocketThree = GameObject.Find("rocketThree");
	public GameObject rocketFour = GameObject.Find("rocketFour");
	public GameObject rocketFive = GameObject.Find("rocketFive");

	public GameObject rocketOneEnd = GameObject.Find("rocketOneEnd");
	public GameObject rocketTwoEnd = GameObject.Find("rocketTwoEnd");
	public GameObject rocketThreeEnd = GameObject.Find("rocketThreeEnd");
	public GameObject rocketFourEnd = GameObject.Find("rocketFourEnd");
	public GameObject rocketFiveEnd = GameObject.Find("rocketFiveEnd");

	public GameObject rocketOneExplosion = GameObject.Find("rocketOneExplosion");
	public GameObject rocketTwoExplosion = GameObject.Find("rocketTwoExplosion");
	public GameObject rocketThreeExplosion = GameObject.Find("rocketThreeExplosion");
	public GameObject rocketFourExplosion = GameObject.Find("rocketFourExplosion");
	public GameObject rocketFiveExplosion = GameObject.Find("rocketFiveExplosion");
    */

    public GameObject ringSprite;
 //   public GameObject ringSpriteTwo;
  //  public GameObject ringSpriteThree;

    public GameObject parachuteOne, parachuteTwo, parachuteThree, parachuteFour, parachuteFive, parachuteSix, parachuteSeven, parachuteEight,
    parachuteNine, parachuteTen, parachuteEleven, parachuteTwelve, parachuteThirteen, parachuteFourteen, parachuteFifteen, parachuteSixteen,
    parachuteSeventeen, parachuteEighteen;

    // Use this for initialization
    void Start () 
	{
			
	}


    //Just testing this rocket mechanic
    //Might need to go in a different function or script; I'm not sure at the moment
    //Will update via Slack on any progress made

    /*
	public Transform rocketOne_startMarker;
	public Transform rocketTwo_startMarker;
	public Transform rocketThree_startMarker;
	public Transform rocketFour_startMarker;
	public Transform rocketFive_startMarker;

	public Transform rocketOne_endMarker;
	public Transform rocketTwo_endMarker;
	public Transform rocketThree_endMarker;
	public Transform rocketFour_endMarker;
	public Transform rocketFive_endMarker;
    */

    public Transform ringSprite_startMarker, ringSprite_endMarker;
 //   public Transform ringSpriteTwo_startMarker, ringSpriteTwo_endMarker;
 //   public Transform ringSpriteThree_startMarker, ringSpriteThree_endMarker;

	public float speed = 1.0F;
	private float startTime;

    //private float rocketOne_journeyLength;
    //private float rocketTwo_journeyLength;
    //private float rocketThree_journeyLength;
    //private float rocketFour_journeyLength;
    //private float rocketFive_journeyLength;

    float ringSprite_journeyLength;
 //   float ringSpriteTwo_journeyLength;
  //  float ringSpriteThree_journeyLength;
	
	// Update is called once per frame
	void Update () {

        ringSprite_journeyLength = ringSprite.transform.position.y - ringSprite_endMarker.position.y;
      //  ringSpriteTwo_journeyLength = ringSpriteTwo.transform.position.y - ringSpriteTwo_endMarker.position.y;
      //  ringSpriteThree_journeyLength = ringSpriteThree.transform.position.y - ringSpriteThree_endMarker.position.y;

        float distCovered = (Time.time - startTime) * speed;

        /*
        float rocketOne_fracJourney = distCovered / rocketOne_journeyLength;
		float rocketTwo_fracJourney = distCovered / rocketTwo_journeyLength;
		float rocketThree_fracJourney = distCovered / rocketThree_journeyLength;
		float rocketFour_fracJourney = distCovered / rocketFour_journeyLength;
		float rocketFive_fracJourney = distCovered / rocketFive_journeyLength;
        */

        float ringSprite_fracJourney = distCovered / ringSprite_journeyLength;
    
        /*
		rocketOne.transform.position = Vector3.Lerp(rocketOne_startMarker.position, rocketOne_endMarker.position, rocketOne_fracJourney);
		rocketTwo.transform.position = Vector3.Lerp(rocketTwo_startMarker.position, rocketTwo_endMarker.position, rocketTwo_fracJourney);
		rocketThree.transform.position = Vector3.Lerp(rocketThree_startMarker.position, rocketThree_endMarker.position, rocketThree_fracJourney);
		rocketFour.transform.position = Vector3.Lerp(rocketFour_startMarker.position, rocketFour_endMarker.position, rocketFour_fracJourney);
		rocketFive.transform.position = Vector3.Lerp(rocketFive_startMarker.position, rocketFive_endMarker.position, rocketFive_fracJourney);
        */

        ringSprite.transform.position = Vector3.Lerp(ringSprite.transform.position, ringSprite_endMarker.position, ringSprite_fracJourney);

        /*
        if (Time.deltaTime == 20)
        {
            ringSpriteTwo.transform.position = Vector3.Lerp(ringSpriteTwo.transform.position, ringSpriteTwo_endMarker.position, ringSpriteTwo_fracJourney);
        }
        if (Time.deltaTime == 40)
        {
            ringSpriteThree.transform.position = Vector3.Lerp(ringSpriteThree.transform.position, ringSpriteThree_endMarker.position, ringSpriteThree_fracJourney);
        }
        */

        if (ringSprite.transform.position == ringSprite_endMarker.transform.position)
		{
            parachuteOne.SetActive(false);
            parachuteTwo.SetActive(false);
            parachuteThree.SetActive(false);
            parachuteFour.SetActive(false);
            parachuteFive.SetActive(false);
            parachuteSix.SetActive(false);
            parachuteSeven.SetActive(false);
            parachuteEight.SetActive(false);
            parachuteNine.SetActive(false);
            parachuteTen.SetActive(false);
            parachuteEleven.SetActive(false);
            parachuteTwelve.SetActive(false);
            parachuteThirteen.SetActive(false);
            parachuteFourteen.SetActive(false);
            parachuteFifteen.SetActive(false);
            parachuteSixteen.SetActive(false);
            parachuteSeventeen.SetActive(false);
            parachuteEighteen.SetActive(false);
        }

        /*
       else if(ringSpriteTwo.transform.position == ringSpriteTwo_endMarker.transform.position)
        {
            parachuteNineteen.SetActive(false);
            parachuteTwenty.SetActive(false);
            parachuteTwentyOne.SetActive(false);
            parachuteTwentyTwo.SetActive(false);
            parachuteTwentyThree.SetActive(false);
            parachuteTwentyFour.SetActive(false);
            parachuteTwentyFive.SetActive(false);
            parachuteTwentySix.SetActive(false);
            parachuteTwentySeven.SetActive(false);
            parachuteTwentyEight.SetActive(false);
            parachuteTwentyNine.SetActive(false);
            parachuteThirty.SetActive(false);
            parachuteThirtyOne.SetActive(false);
            parachuteThirtyTwo.SetActive(false);
            parachuteThirtyThree.SetActive(false);
            parachuteThirtyFour.SetActive(false);
            parachuteThirtyFive.SetActive(false);
        }

       else if(ringSpriteThree.transform.position == ringSpriteThree_endMarker.transform.position)
        {
            parachuteThirtySix.SetActive(false);
            parachuteThirtySeven.SetActive(false);
            parachuteThirtyEight.SetActive(false);
            parachuteThirtyNine.SetActive(false);
            parachuteForty.SetActive(false);
            parachuteFortyOne.SetActive(false);
            parachuteFortyTwo.SetActive(false);
            parachuteFortyThree.SetActive(false);
            parachuteFortyFour.SetActive(false);
            parachuteFortyFive.SetActive(false);
            parachuteFortySix.SetActive(false);
            parachuteFortySeven.SetActive(false);
            parachuteFortyEight.SetActive(false);
            parachuteFortyNine.SetActive(false);
            parachuteFifty.SetActive(false);
            parachuteFiftyOne.SetActive(false);
            parachuteFiftyTwo.SetActive(false);
            parachuteFiftyThree.SetActive(false);
        }
        */
	}	


	void spawnRockets()
	{
		startTime = Time.time;
        ringSprite_journeyLength = Vector3.Distance(ringSprite.transform.position, ringSprite_endMarker.position);
    //    ringSpriteTwo_journeyLength = Vector3.Distance(ringSpriteTwo.transform.position, ringSpriteTwo_endMarker.position);
      //  ringSpriteThree_journeyLength = Vector3.Distance(ringSpriteThree.transform.position, ringSpriteThree_endMarker.position);
        /*
        rocketOne_journeyLength = Vector3.Distance(rocketOne_startMarker.position, rocketOne_endMarker.position);
		rocketTwo_journeyLength = Vector3.Distance(rocketTwo_startMarker.position, rocketTwo_endMarker.position);
		rocketThree_journeyLength = Vector3.Distance(rocketThree_startMarker.position, rocketThree_endMarker.position);
		rocketFour_journeyLength = Vector3.Distance(rocketFour_startMarker.position, rocketFour_endMarker.position);
		rocketFive_journeyLength = Vector3.Distance(rocketFive_startMarker.position, rocketFive_endMarker.position);
        */
	}
}
