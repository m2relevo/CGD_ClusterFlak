using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour
{
    int shots;
    int bulletType = 1;
    public GameObject RedShot, BlueShot, YellowShot, GreenShot, DaggerShot;
    //GameObject Enemy;
    public double redFireRate = 0.1;
    int lastBullet;
    bool timerDone = true;

    int redFireCount = 0, blueFireCount = 0, standardFireCount = 0;


    void Start()
    {
        InvokeRepeating("fireStandard", 1, 1.5f);
    }

    void Update()
    {

        

        if(timerDone == true)
        {
            rBullet();
            phase1();
        }
      

        //if(bossHealth < 10000)
        //{ }      
  
    }


    void phase1() //BEGINNING PHASE
    {
        if (bulletType == 1 && timerDone == true)
        {

            //  for (int i = 0; i < shots; i++)
            // {          
            //   fireRed();
            timerDone = false;
            InvokeRepeating("fireRed", 1, 0.2F);            
            // }

        }
        if (bulletType == 2 && timerDone == true)
        {
            timerDone = false;
            InvokeRepeating("fireBlue", 1, 1f);            
        }
        //Instantiate(BlueShot, Enemy.Transform.position, (360 / shots) * i);
        if (bulletType == 3)
        {
            timerDone = true; //this NEEDS TO BE FALSE EVENTUALLY
            
        }

        if (bulletType == 4)
        {
            timerDone = false; //this NEEDS TO BE FALSE EVENTUALLY
        }
        
    }


    void rBullet()
    {
        bulletType = Random.Range(1, 4);            
        shots = Random.Range(10, 25);
        Debug.Log(bulletType);
      
        if (bulletType == lastBullet)
        {
            do
            {
                bulletType = Random.Range(1, 4);
            } while (bulletType != lastBullet);
        }

    }

    void fireRed()
    {
        redFireCount++;
        GameObject RedBull = (GameObject)Instantiate(RedShot, transform.position, Quaternion.identity);
        RedBull.transform.name = "redBullet_" + redFireCount.ToString();
        //RedBull.transform.parent = gameObject.transform;

       if(redFireCount >= shots)
        {
            CancelInvoke("fireRed");
            redFireCount = 0;
            timerDone = true;
        }

    }

    void fireBlue()
    {
        blueFireCount++;
        GameObject BlueBull = (GameObject)Instantiate(BlueShot, transform.position, Quaternion.identity);
        BlueBull.transform.name = "blueBullet_" + blueFireCount.ToString();
       // BlueBull.transform.parent = gameObject.transform;

        if (blueFireCount >= 5)
        {
            CancelInvoke("fireBlue");
            blueFireCount = 0;
            timerDone = true;
        }
    }

    void fireStandard()
    {
        standardFireCount++;
        GameObject standardBull = (GameObject)Instantiate(DaggerShot, transform.position, Quaternion.identity);
        standardBull.transform.name = "standardBullet_" + standardFireCount.ToString();
        //standardBull.transform.parent = gameObject.transform;       
    }

}