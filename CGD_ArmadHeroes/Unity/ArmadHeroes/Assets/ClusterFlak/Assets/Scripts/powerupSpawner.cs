using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class powerupSpawner : MonoBehaviour
{
    public GameObject powerupParent;

    public List<GameObject> PowerupObject;

    public int Frequency = 30;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnPowerUp", 2, Frequency);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PUpicker()
    {
        int Powerupnumber = Random.Range(1, 2);
    }

    void SpawnPowerUp()
    {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            int Powerupnumber = Random.Range(0, PowerupObject.Count);

            GameObject PU = (GameObject)Instantiate(PowerupObject[Powerupnumber]);
            PU.transform.parent = powerupParent.transform;
            PU.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
    }
}
