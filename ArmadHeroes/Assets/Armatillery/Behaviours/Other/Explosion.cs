/// <summary>
/// Created and implemented by Craig Tinney - ?
/// Edited by Daniel Weston - 10/01/2016
/// </summary>
using UnityEngine;
using System.Collections;
using Armatillery;
public class Explosion : MonoBehaviour 
{
    //Needs to be called when bullet hits a collision
	public void InitExplode (float scale, ArmadHeroes.Actor _owner)
    {
        GetComponentInChildren<ProjectileBase>().owner = _owner;
        transform.localScale = new Vector3(scale, scale, 0.0f);//What do we have here
        //Camera.main.GetComponent<CameraShake>().ShakeCamera();
        Invoke("Explode", 0.025f);
	}

    public void InitExplode(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 0.0f);//What do we have here
        //Camera.main.GetComponent<CameraShake>().ShakeCamera();
        Invoke("Explode", 0.025f);
    }

    private void Explode()
    {
        //turn image to black
       GetComponent<ParticleSystem>().Play();
       Invoke("TurnOffObject", 0.35f);
    }

    private void SwitchOff()
    {
        //reset back to white
        GetComponent<SpriteRenderer>().material.color = Color.black;
        Invoke("TurnOffObject", 0.1f);
    }

    private void TurnOffObject()
    {
        //turn object off
        gameObject.SetActive(false);
    }
}
