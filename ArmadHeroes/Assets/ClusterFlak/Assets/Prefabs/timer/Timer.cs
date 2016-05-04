using UnityEngine;
using System.Collections;
using UnityEngine.UI;

    public class Timer : MonoBehaviour
{
    public float timeLeft = 120.0f;
    public Text text;
    public GameObject boom;
    private Vector2 pos;
    private Vector3 boomloc = new Vector3(0, 500, 0);
    private Quaternion boomquat = Quaternion.Euler(0, 0, 0);
    private bool bomlock;

    void Start()
    {
        
    }
    void Update()
    {
        if (timeLeft > 0)
        { 
        timeLeft -= Time.deltaTime;
        text.text = "Time Until Missile Launch: " + Mathf.Round(timeLeft);
        }


        if (timeLeft < 0)
        {
            text.text = "MISSILE IMPACT IMMINENT";
            if (bomlock == false)
            {
                GameObject.Instantiate(boom, boomloc, boomquat);
                bomlock = true;
            }
            
            
        }
    }
}