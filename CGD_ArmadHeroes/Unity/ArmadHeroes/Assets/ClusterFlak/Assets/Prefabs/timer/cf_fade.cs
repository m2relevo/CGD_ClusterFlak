using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class cf_fade : MonoBehaviour {
    public Image image;
    public bool fadebool = false;
    public bool winbool = false;
    public bool failbool = false;
    private int fadetime;
    public List<GameObject> listPlayers2;


    // Use this for initialization
    void Start ()
    {
        
        
    }
	
	// Update is called once per frame
	void Update ()
    {
 
        //bool to trigger a fade when the missile trigger is activated
        if (failbool == true)
        {
            if (fadebool == false)
            { 
                InvokeRepeating("FadeInWhite", 1, 0.2f);
            fadebool = true;
            }
            
        }


        //triggers fade when main APC is destroyed
        if (winbool == true)
        {
            if (fadebool == false)
            {
                InvokeRepeating("FadeInWhite", 1, 0.2f);
                
                fadebool = true;
            }
            
        }
        //if players are all destroyed, this triggers the endgame fade
        if (GameObject.FindGameObjectWithTag("ClusterFlak/Player") == null)
            if (fadebool == false)
            {
                InvokeRepeating("FadeInWhite", 1, 0.2f);
                
                fadebool = true;
            }


        if (fadetime > 15)
        {
           SceneManager.LoadScene("DebriefScene"); 
        }
    }

    void FadeInWhite()
    {//alters canvas objet to fade into white 
        Color color = image.color;
        color.a += 0.1f;
        image.color = color;
        fadetime++; 

    }
}
