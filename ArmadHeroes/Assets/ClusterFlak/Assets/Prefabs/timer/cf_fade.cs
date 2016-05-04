using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class cf_fade : MonoBehaviour {
    public Image image;
    public bool fadebool = false;
    public bool winbool = false;
    public bool failbool = false;
    private int fadetime;
    
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (failbool == true)
        {
            if (fadebool == false)
            { 
                InvokeRepeating("FadeInWhite", 1, 0.2f);
            fadebool = true;
            }
            
        }



        if (winbool == true)
        {
            if (fadebool == false)
            {
                InvokeRepeating("FadeInWhite", 1, 0.2f);
                Debug.Log("winbool working");
                fadebool = true;
            }
            
        }


        if (fadetime > 15)
        {
           SceneManager.LoadScene("DebriefScene"); 
        }
    }

    void FadeInWhite()
    {
        Color color = image.color;
        color.a += 0.1f;
        image.color = color;
        Debug.Log("FadeWorks");
        fadetime++; 

    }
}
