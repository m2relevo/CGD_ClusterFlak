using UnityEngine;
using System.Collections;

public class CF_ICBM : MonoBehaviour {
    private Vector3 node;
    public GameObject explosion;
    public GameObject icbmexplosion;
    public bool explosiontrigger;
    private bool icbmebool;
    private bool icbme1bool;
    
	void Start () {
        node = new Vector3(0, 0, 0);
        
    }
	
	
	void Update ()
    {//launches missile towards designated node in centre of screen
        transform.position = Vector3.MoveTowards(transform.position, node, 1f);
        
        if (transform.position == node)//once the missile hits the node, triggers the explosion endgame for other scrupts and removes the missile
        {
            explosiontrigger = true;
            if (icbme1bool == false)
            {
                GameObject icbme1 = (GameObject)Instantiate(icbmexplosion);//plays initial explosion effect
                icbme1.transform.position = new Vector3(0, 0, 0);
                icbme1bool = true; 
            }

            if (icbmebool == false)//plays repeated explosion effect
            {
                InvokeRepeating("ICBMExplosion", 0.5f, 0.1f);
                icbmebool = true;
            }
            
            Destroy(GameObject.FindWithTag("ClusterFlak/ICBMsprite"));
            GameObject.Find("fade").GetComponent<cf_fade>().failbool = true;
        }
    }

    void ICBMExplosion()
    {
        //creates random explosion targets and instantiates the particle effect on each target
        GameObject icbme = (GameObject)Instantiate(explosion);
        int xcoord = Random.Range(-9, 10);
        int ycoord = Random.Range(8, -10);
        node = new Vector3(xcoord, ycoord, 0);
        icbme.transform.position = node;
    }

}
