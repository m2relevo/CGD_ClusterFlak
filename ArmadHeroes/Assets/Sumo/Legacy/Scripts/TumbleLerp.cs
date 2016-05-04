using UnityEngine;
using System.Collections;

public class TumbleLerp : MonoBehaviour
{
    GameObject tumbleWeed;

    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    // Use this for initialization
    void Start ()
    {
        tumbleWeed = GameObject.Find("Tumbleweed");
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
    }
}
