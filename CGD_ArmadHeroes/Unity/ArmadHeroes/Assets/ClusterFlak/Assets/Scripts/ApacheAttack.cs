using UnityEngine;
using System.Collections;

public class ApacheAttack : MonoBehaviour
{//attack manager for Apache enemy
    int shots;
    int bulletType = 1;
    public GameObject BurstMissile, HomingMissile;
    public double burstFireRate = 0.1;
    int lastBullet = 1;
    bool timerDone = true;

    int burstFireCount = 0, homingFireCount = 0;


    void Start()
    {
        InvokeRepeating("fireStandard", 1, 1.5f);
    }

    void Update()
    {
        if (timerDone == true)
        {
            rBullet();
            phase1();
        }  
    }


    void phase1() //BEGINNING PHASE
    {
        if (bulletType == 1 && timerDone == true)
        {

          
            timerDone = false;
            InvokeRepeating("fireRed", 1, 0.2F);
            

        }
        if (bulletType == 2 && timerDone == true)
        {
            timerDone = false;
            InvokeRepeating("fireBlue", 1, 1f);
        }


    }


    void rBullet()
    {//code to randomly decide which bullet to fire 
        bulletType = Random.Range(1, 4);
        shots = Random.Range(10, 25);
        

        if (bulletType == lastBullet)
        {
            do
            {
                bulletType = Random.Range(1, 2);
            } while (bulletType != lastBullet);
        }

    }

    void fireRed()
    {
        burstFireCount++;
        GameObject Burst = (GameObject)Instantiate(BurstMissile, transform.position, Quaternion.identity);
        Burst.transform.name = "burstMissile_" + burstFireCount.ToString();

        if (burstFireCount >= shots)
        {
            CancelInvoke("fireRed");
           burstFireCount = 0;
            timerDone = true;
        }

    }

    
    void fireStandard()
    {
        homingFireCount++;
        GameObject homingMissile = (GameObject)Instantiate(HomingMissile, transform.position, Quaternion.identity);
        homingMissile.transform.name = "HomingMissile_" + homingFireCount.ToString();
              
    }

}