using UnityEngine;
using System.Collections;

/// <summary>
/// Calls collision callbacks on a specified gameobject when this gameobject collision callbacks trigger
/// </summary>
public class CollisionOnGameobject : MonoBehaviour {

	public GameObject linkedGameObject;

	void OnCollisionEnter2D(Collision2D other)
	{
		linkedGameObject.SendMessage ("OnCollisionEnter2D", other);
	}

	void OnCollisionExit2D(Collision2D other)
	{
		linkedGameObject.SendMessage ("OnCollisionExit2D", other);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		linkedGameObject.SendMessage ("OnTriggerEnter2D", other);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		linkedGameObject.SendMessage ("OnTriggerExit2D", other);
	}
}
