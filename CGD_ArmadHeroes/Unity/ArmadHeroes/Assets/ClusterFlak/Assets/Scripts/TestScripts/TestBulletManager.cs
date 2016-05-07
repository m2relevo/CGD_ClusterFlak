using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBulletManager : MonoBehaviour
{//DEPRECATED
    public GameObject SpawnLocation;
    public List<GameObject> Phase1List;
    public List<GameObject> Phase2List;
    public List<GameObject> Phase3List;

    private static float BossHP;
    private float timer;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer = 180f - Time.deltaTime;
        BossHP = BossHealth.CurrentBosshealth;

        if (BossHP < 75 || timer < 120f)
        {
            InvokeRepeating("Phase1", 1, 1F);

            if (BossHP < 50 || timer < 80f)
            {
                InvokeRepeating("Phase2", 1, 0.5F);

                if (BossHP < 25 || timer < 40f)
                {
                    InvokeRepeating("Phase3", 1, 0.25F);;
                }
            }
        }
    }

    void Phase0()
    {

    }

    void Phase1()
    {
        int RNG = Random.Range(0, Phase1List.Count);

        GameObject Projectile = (GameObject)Instantiate(Phase1List[RNG]);
        Projectile.transform.position = SpawnLocation.transform.position;
    }

    void Phase2()
    {
        int RNG = Random.Range(0, Phase1List.Count);

        GameObject Projectile = (GameObject)Instantiate(Phase2List[RNG]);
        Projectile.transform.position = SpawnLocation.transform.position;
    }

    void Phase3()
    {
        int RNG = Random.Range(0, Phase1List.Count);

        GameObject Projectile = (GameObject)Instantiate(Phase3List[RNG]);
        Projectile.transform.position = SpawnLocation.transform.position;
    }
}