/// <summary>
/// PowerUpCountdown should be attached to power up radials
/// Created and implemented by Daniel Weston - 15/01/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Armatillery;

//Handles powerup UI management 
public class PowerUpCountdown : MonoBehaviour 
{
    public GameObject DialImage, DialIcon;//radial components
    public float timer = 10;//how fast I count down 
    private int waitTimer = 10;
    private bool active = false;//active state
    ArmaPlayer callback;
    ArmadHeroes.BulletModifier bm;

    /// <summary>
    /// Activate should be called by the player 
    /// when a powerup is picked up
    /// </summary>
    public void Activate(ArmaPlayer _owner, ArmadHeroes.BulletModifier _bm)
    {
        callback = _owner;
        bm = _bm;
        DialImage.SetActive(true);
        DialIcon.SetActive(true);
    }

    void Update()
    {
        if (DialIcon.activeSelf && DialIcon.activeSelf)
        {
            if (powerUpManager.instance.countDown)
            {
                //Debug.Log("in wave");
                timer -= 1.0f * Time.deltaTime;//count down timer
            }
            else if ((bm & ArmadHeroes.BulletModifier.teleport) != 0)
            {
                //Debug.Log("TP");
                timer -= 1.0f * Time.deltaTime;//count down timer
            }

            //set radials
            DialImage.GetComponent<Image>().fillAmount = timer / waitTimer;
            DialIcon.GetComponent<Image>().fillAmount = timer / waitTimer;
            
            //check/update timer
            active = timer >= 0 ? true : false;
            
            //if inactive reset timer
            if (!active)
            {
                callback.RemovePowerUp(bm);
                timer = 10;
            }
            //update components pending on time
            DialImage.SetActive(active);
            DialIcon.SetActive(active);
            gameObject.SetActive(active);        
        }
    }
}
