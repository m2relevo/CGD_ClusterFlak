using UnityEngine;
using System.Collections;

namespace Astrodillos{
	public static class SpriteOrdering {

		public static int GetOrder(float yPosition){
			return (int)(-(yPosition) * 10);
		}

		public static int CollsionLayerFromHeight(float height){

			if (height >= 0.5f && height < 1.5f) {
				return LayerMask.NameToLayer ("Astrodillos/Mid");
			} else if (height >= 1.5f && height < 2.5f) {
				return LayerMask.NameToLayer ("Astrodillos/Top");
			} else if (height >= 2.5f) {
				return LayerMask.NameToLayer ("Astrodillos/Sky");
			}

			return LayerMask.NameToLayer ("Astrodillos/Bottom");
		}
	}
}
