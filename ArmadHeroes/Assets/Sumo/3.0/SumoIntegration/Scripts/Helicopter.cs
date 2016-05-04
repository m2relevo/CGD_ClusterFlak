using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour
{
    public GameObject GO_Helicopter;

    public Transform startPos, helo_endPos;
 
    public float speed = 1.0F;

    float helo_journeyLength;   

        // Use this for initialization
        void Start ()
    {
        GO_Helicopter = GameObject.Find("Helicopter");
        GO_Helicopter.transform.Rotate(0, 0, 0);
        GO_Helicopter.transform.localScale = new Vector3(60, 60, 60);
        GO_Helicopter.transform.position = new Vector3(0, -600, 0); 
 	}

    private float startTime;

    // Update is called once per frame
    void Update ()
    {
        helo_journeyLength = GO_Helicopter.transform.position.y + (helo_endPos.transform.position.y + 1200);

        float distCovered = (Time.time - startTime) * speed;

        float helo_fracJourney = distCovered / helo_journeyLength;

        GO_Helicopter.transform.position = Vector3.Lerp(GO_Helicopter.transform.position, helo_endPos.position, helo_fracJourney);
    }
}
