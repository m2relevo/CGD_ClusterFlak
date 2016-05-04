using UnityEngine;
using System.Collections;

namespace Astrodillos{
	[ExecuteInEditMode]
	public class IsoSpriteOrder : MonoBehaviour {

		//0 is top of sprite, 1 is bottom
		public float spriteSortYPoint = 1.0f;
		SpriteRenderer spriteRenderer;
		// Use this for initialization
		void Awake () {
			spriteRenderer = GetComponent<SpriteRenderer> ();

			float yPos = (transform.position.y + (spriteRenderer.sprite.bounds.extents.y)) - (spriteRenderer.sprite.bounds.extents.y*spriteSortYPoint*2);
			spriteRenderer.sortingOrder = SpriteOrdering.GetOrder (yPos);
		}

	}
}
