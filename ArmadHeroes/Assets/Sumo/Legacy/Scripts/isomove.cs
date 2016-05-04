using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class isomove : MonoBehaviour {

	//Cartesian forward Vector -- (1.0, 0.0, 0.0) tranlates into NW direction --
	Vector3 forward;
	//Holds original scale of sprite, used for inverting sprite
	Vector3 scale;
    //Reference to connected spritelist script (Saves making a getcomponent call all the time)
    sumo_SpriteList _spriteList;
	//Speed of sprites movement
	float speed;
	//Holds the last sprite used by the character (currently used to return to correct sprite after rolling)
	Sprite prevSprite;

	// Use this for initialization
	void Start () 
	{
		scale = this.transform.localScale;
		_spriteList = this.GetComponent<sumo_SpriteList>();
		speed = 2.0f;	
	}

	// Update is called once per frame
	void Update () {

		////////////////////////
		///        N         ///
		///    NW  |   NE    ///
		///     \     /      ///   
		/// W -         - E  ///
		///     /     \      ///
		///   SW   |  SE     ///
		///        S         ///
		////////////////////////
		 
		////////////////////////////////////////////////////////////////////////
		/// Following block takes directional input, sets the forward (cartesian) for the character, and sets the correct sprite (and scale) for the new forward
        ///         
		
        //TRUE UP (N)
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            forward = new Vector3(0.01f, 0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("N")];
            this.transform.localScale = scale;
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        //TRUE DOWN (S)
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            forward = new Vector3(-0.01f, -0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("S")];
            this.transform.localScale = scale;
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        //TRUE LEFT (W)
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            forward = new Vector3(-0.01f, 0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
            this.transform.localScale = new Vector3((-scale.x), scale.y, scale.z);
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        //TRUE RIGHT (E)
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            forward = new Vector3(0.01f, -0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
            this.transform.localScale = scale;
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        // NE
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            forward = new Vector3(0.01f, 0.0f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
            this.transform.localScale = scale;
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        // SE
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            forward = new Vector3(0.0f, -0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
            this.transform.localScale = scale;
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        // SW
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            forward = new Vector3(-0.01f, 0.0f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
            this.transform.localScale = new Vector3((-scale.x), scale.y, scale.z);
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        // NW
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            forward = new Vector3(0.0f, 0.01f, 0.0f);
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
            this.transform.localScale = new Vector3((-scale.x), scale.y, scale.z);
            prevSprite = GetComponent<SpriteRenderer>().sprite;
        }
        //////////////////////////////////////////////////////////////////////////////
        /// Takes spacebar input for 'boost',  increases speed
        if (Input.GetKey (KeyCode.Space))
		{		
			speed = 7;
		}
		//////////////////////////////////////////////////////////////////////////////
		/// Moves character (converts forward to iso here), in forwards direction at current speed
		this.transform.position += ExtensionMethods.toIso(forward * speed);	
		//////////////////////////////////////////////////////////////////////////////
		/// 
		if (speed > 2) 
		{
			speed -= 0.1f;
			this.GetComponent<SpriteRenderer> ().sprite = _spriteList.spriteArray [_spriteList.spriteNames.IndexOf ("Ball")];
		}
		/// 
		else 
		{
			GetComponent<SpriteRenderer>().sprite = prevSprite;
		}
	}


	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//If a character leaves the 'ground' trigger, bounce will be called, whcih will reverse its direction and sprite
	public void bounce()
	{
		if (forward == new Vector3 (0.01f, 0.0f, 0.0f)) 
		{
			forward = new Vector3 (-0.01f, 0.0f, 0.0f);
			this.GetComponent<SpriteRenderer> ().sprite = _spriteList.spriteArray [_spriteList.spriteNames.IndexOf ("SE")];
			this.transform.localScale = new Vector3 ((-scale.x), scale.y, scale.z);
			prevSprite = GetComponent<SpriteRenderer> ().sprite;
		}
		else if (forward == new Vector3 (-0.01f, 0.0f, 0.0f))
		{
			forward = new Vector3 (0.01f, 0.0f, 0.0f);
			this.GetComponent<SpriteRenderer> ().sprite = _spriteList.spriteArray [_spriteList.spriteNames.IndexOf ("NE")];
			this.transform.localScale = scale;
			prevSprite = GetComponent<SpriteRenderer> ().sprite;
			
		}
		else if (forward == new Vector3 (0.0f, 0.01f, 0.0f))
		{
			forward = new Vector3 (0.0f, -0.01f, 0.0f);
			this.GetComponent<SpriteRenderer> ().sprite = _spriteList.spriteArray [_spriteList.spriteNames.IndexOf ("SE")];
			this.transform.localScale = scale;
			prevSprite = GetComponent<SpriteRenderer> ().sprite;
		}
		else if (forward == new Vector3 (0.0f, -0.01f, 0.0f))
		{
			forward = new Vector3 (0.0f, 0.01f, 0.0f);
			this.GetComponent<SpriteRenderer> ().sprite = _spriteList.spriteArray [_spriteList.spriteNames.IndexOf ("NE")];
			this.transform.localScale = new Vector3 ((-scale.x), scale.y, scale.z);
			prevSprite = GetComponent<SpriteRenderer> ().sprite;
		}
	}
}
