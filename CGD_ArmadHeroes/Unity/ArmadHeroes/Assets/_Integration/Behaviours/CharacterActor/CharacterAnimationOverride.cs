

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public abstract class CharacterAnimationOverride : MonoBehaviour {

	protected string characterName = "basic";
	protected Sprite[] subSprites;//Array of sprites in the character's spritesheet


	////Replace sprite animation with the correct spritesheet sprite - necessary so we can use a single animation controller/animations
	protected virtual void LateUpdate () {
		if (characterName != "basic") {
			UpdateSprite ();
		}
	}

	public void SetCharacter(string character){
		characterName = character;
		SetSubSprites ("_Integration/Sprites/Armad/"+character+"_character_atlas");
	}

	/// <summary>
	/// Using the passed file path 
	/// SetSubSprites will populate the 
	/// SubSprites Array in actor
	/// </summary>
	/// <param name="path"></param>
	private void SetSubSprites(string path)
	{
		//Load an array of all sprites in the spritesheet
		subSprites = Resources.LoadAll<Sprite>(path);

	}

	//Sets sprite to correct animal from spritesheet - implemented in child classes
	public virtual void UpdateSprite(){}
}
