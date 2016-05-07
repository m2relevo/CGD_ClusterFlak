using UnityEngine;
using System.Collections;

namespace Astrodillos{
	public class SpaceICBM : MonoBehaviour {

		Rigidbody2D body;
		Vector3 startPos, startRot;

		// Use this for initialization
		void Awake () {
			body = GetComponent<Rigidbody2D> ();
			startPos = transform.position;
			startRot = transform.localEulerAngles;
		}
		
		public void Reset(){
			body.velocity = Vector2.zero;
			body.angularVelocity = 0;
			transform.localPosition = startPos;
			transform.localEulerAngles = startRot;
		}
	}
}
