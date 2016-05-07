using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour
{//bullet manager that randomly shoots different versions of the turret bullet 
    int shots;
    int bulletType = 1;
    public GameObject RedShot, BlueShot, YellowShot, GreenShot, DaggerShot;
    //GameObject Enemy;
    public double redFireRate = 0.1;
    int lastBullet = 1;
    bool timerDone = true;
    public bool mobsDead = true;
    float timer = 10f;

    int redFireCount = 0, blueFireCount = 0, standardFireCount = 0, yellowFireCount = 0;


    void Start()
    {
        InvokeRepeating("fireStandard", 1, 1.5f);
    }

    void Update()
    {
        timer = timer -= Time.deltaTime;
        
        
        if(timerDone == true)
        {
            rBullet();
            phase1();
        }

        if (GameObject.FindGameObjectWithTag("ClusterFlak/Mob") == null)
        {
            mobsDead = true;
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
            if (mobsDead && timer < 0)
            {
                timerDone = false;
                fireYellow();
            }
            else if(!mobsDead)
            {
                timerDone = true;
            }
            
        }

        if (bulletType == 4)
        {
            timerDone = true; //this NEEDS TO BE FALSE EVENTUALLY
        }
        
    }


    void rBullet()
    {
        bulletType = Random.Range(1, 4);            
        shots = Random.Range(10, 25);
        
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

    void fireYellow()
    {        
        if (mobsDead)
        {
            yellowFireCount++;
            mobsDead = false;
            GameObject YelBull = (GameObject)Instantiate(YellowShot, transform.position, Quaternion.identity);
            YelBull.transform.name = "yelBullet_" + yellowFireCount.ToString();
            timerDone = true;
        }
        else if (!mobsDead)
        {
            timerDone = true;
        }
    }

    void fireStandard()
    {
        standardFireCount++;
        GameObject standardBull = (GameObject)Instantiate(DaggerShot, transform.position, Quaternion.identity);
        standardBull.transform.name = "standardBullet_" + standardFireCount.ToString();   
    }
}