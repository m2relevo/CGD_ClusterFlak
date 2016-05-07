/***********************************
 * CharacterAnimationOverride.cs
 * Created by Alan Parsons 04/02/16
 * *********************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CharacterAnimationOverride_Sprite : CharacterAnimationOverride {
	public SpriteRenderer spriteRenderer;

	//Sets sprite to correct character from spritesheet
	public override void UpdateSprite()
	{
		if (spriteRenderer.sprite != null)
		{
			string spriteName = spriteRenderer.sprite.name;

			//Find the name of the current sprite in the loaded spritesheet
			Sprite newSprite = Array.Find(subSprites, item => item.name == spriteName);
			if (newSprite != null)
			{
				spriteRenderer.sprite = newSprite;
			}
		}
	}
}
