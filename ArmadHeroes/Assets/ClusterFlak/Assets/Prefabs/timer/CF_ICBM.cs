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
    {
        transform.position = Vector3.MoveTowards(transform.position, node, 1f);
        
        if (transform.position == node)
        {
            explosiontrigger = true;
            if (icbme1bool == false)
            {
                GameObject icbme1 = (GameObject)Instantiate(icbmexplosion);
                icbme1.transform.position = new Vector3(0, 0, 0);
                icbme1bool = true; 
            }

            if (icbmebool == false)
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

        GameObject icbme = (GameObject)Instantiate(explosion);
        int xcoord = Random.Range(-9, 10);
        int ycoord = Random.Range(8, -10);
        node = new Vector3(xcoord, ycoord, 0);
        icbme.transform.position = node;
    }

}
