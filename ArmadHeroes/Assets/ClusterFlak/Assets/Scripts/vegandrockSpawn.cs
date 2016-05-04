using UnityEngine;
using System.Collections;

public class vegandrockSpawn : MonoBehaviour
{
    public GameObject RockandVegSpawn;

    public GameObject Rock1;
    public GameObject Rock2;
    public GameObject Rock3;
    public GameObject Veg1;
    public GameObject Veg2;
    public GameObject Veg3;

	// Use this for initialization
	void Start ()
    {
        spawnRock();
        spawnVeg();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void spawnRock()
    {
        int amountRock1 = Random.Range(1, 11);
        int amountRock2 = Random.Range(1, 11);
        int amountRock3 = Random.Range(1, 11);

        for (int i = 0; i < amountRock1; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject RockBig = (GameObject)Instantiate(Rock1);
            RockBig.transform.parent = RockandVegSpawn.transform;
            RockBig.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }

        for (int i = 0; i < amountRock2; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject RockMed = (GameObject)Instantiate(Rock2);
            RockMed.transform.parent = RockandVegSpawn.transform;
            RockMed.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }

        for (int i = 0; i < amountRock3; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject RockSma = (GameObject)Instantiate(Rock3);
            RockSma.transform.parent = RockandVegSpawn.transform;
            RockSma.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }
    }

    void spawnVeg()
    {
        int amountVeg1 = Random.Range(1, 11);
        int amountVeg2 = Random.Range(1, 11);
        int amountVeg3 = Random.Range(1, 11);

        for (int i = 0; i < amountVeg1; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject VegBig = (GameObject)Instantiate(Veg1);
            VegBig.transform.parent = RockandVegSpawn.transform;
            VegBig.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }

        for (int i = 0; i < amountVeg2; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject VegMed = (GameObject)Instantiate(Veg2);
            VegMed.transform.parent = RockandVegSpawn.transform;
            VegMed.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }

        for (int i = 0; i < amountVeg3; i++)
        {
            float Xlocation = Random.Range(-12.82f, 13.62f);
            float Ylocation = Random.Range(-1.4f, 1.67f);

            GameObject VegSma = (GameObject)Instantiate(Veg2);
            VegSma.transform.parent = RockandVegSpawn.transform;
            VegSma.transform.localPosition = new Vector3(Xlocation, Ylocation, 0f);
        }
    }
}
