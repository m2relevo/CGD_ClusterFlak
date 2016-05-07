using UnityEngine;
using System.Collections;

public class APCs : MonoBehaviour
{
    public GameObject APCOne;
    public GameObject APCTwo;

    public Transform APCOne_startPos, APCOne_endPos;
    public Transform APCTwo_startPos, APCTwo_endPos;

    public float speed = 1.0F;

    float APCOne_journeyLength;
    float APCTwo_journeyLength;

    // Use this for initialization
    void Start()
    {
        APCOne = GameObject.Find("APCOne");
        APCTwo = GameObject.Find("APCTwo");
    }

    private float startTime;

    void Update()
    {
        APCOneControl();
        APCTwoControl();
    }

    void APCOneControl()
    {
        APCOne_journeyLength = APCOne.transform.position.y + (APCOne_endPos.transform.position.y + 300);
        float distCovered = (Time.time - startTime) * speed;
        float APCOne_fracJourney = distCovered / APCOne_journeyLength;
        APCOne.transform.position = Vector3.Lerp(APCOne.transform.position, APCOne_endPos.position, APCOne_fracJourney);
    }

    void APCTwoControl()
    {
        APCTwo_journeyLength = APCTwo.transform.position.y - (APCTwo_endPos.transform.position.y - 300);
        float distCovered = (Time.time - startTime) * speed;
        float APCTwo_fracJourney = distCovered / APCTwo_journeyLength;
        APCTwo.transform.position = Vector3.Lerp(APCTwo.transform.position, APCTwo_endPos.position, APCTwo_fracJourney);
    }

    void resetAPCOne()
    {
        APCOne.transform.position = APCOne_startPos.transform.position;
    }

    void resetAPCTwo()
    {
        APCTwo.transform.position = APCTwo_startPos.transform.position;
    }
}