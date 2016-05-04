//Armad Heros 
//SUMO 
//Gareth Griffiths, Peter Maloney, Alex Nuns, Jake Downing
//Custom Collision
//
//Custom collision system for vehichle collisions

using UnityEngine;


public class sumo_CustomCollisions : MonoBehaviour
{

    public static sumo_CustomCollisions instance;
    public int collisionID;
	public float bounceStrength, speedReduce;

	public AudioClip collisionSound; 

	// Use this for initialization
	void Awake ()
    {

        if(instance == null)
        {
            instance = this;
        }
        if(this != instance)
        {
            Destroy(this);
        }

        collisionID = 0;
	
	}
	
	public void collision(GameObject colliderOne, GameObject colliderTwo, int ID, ContactPoint2D collisionPoint)
	{
		ArmadHeroes.SoundManager.instance.PlayClip (collisionSound, false, 1);

		if (colliderOne.GetComponent<sumo.sumo_VehicleMovementV2> () && colliderTwo.GetComponent<sumo.sumo_VehicleMovementV2> ())
        {
			sumo.sumo_VehicleMovementV2 colliderOneScript = colliderOne.GetComponent<sumo.sumo_VehicleMovementV2> ();
			sumo.sumo_VehicleMovementV2 colliderTwoScript = colliderTwo.GetComponent<sumo.sumo_VehicleMovementV2> ();

			float angle = Vector2.Angle (colliderOneScript.direction, colliderTwoScript.direction);

			//Each player gets knocked back, the force decided by the speed of what they are hit by;
			colliderOneScript.bump = -(Vector2)(colliderTwo.transform.position - colliderOne.transform.position) * (bounceStrength * (1 + colliderTwoScript.speed / colliderTwoScript.maxSpeed));
			colliderTwoScript.bump = -(Vector2)(colliderOne.transform.position - colliderTwo.transform.position) * (bounceStrength * (1 + colliderTwoScript.speed / colliderTwoScript.maxSpeed));

			if (colliderOneScript.speed - speedReduce > 0) {
				colliderOneScript.speed -= speedReduce;
			} else {
				colliderOneScript.speed = 0;
			}
			if (colliderTwoScript.speed - speedReduce > 0) {
				colliderTwoScript.speed -= speedReduce;
			} else {
				colliderTwoScript.speed = 0;
			}
		} 
		else 
		{
			if (colliderOne.GetComponent<sumo.sumo_VehicleMovementV2> ()) 
			{	
				colliderOne.GetComponent<sumo.sumo_VehicleMovementV2> ().bump = -(Vector2)(colliderTwo.transform.position - colliderOne.transform.position) * (bounceStrength);
			}

			else if(colliderTwo.GetComponent<sumo.sumo_VehicleMovementV2> ())
			{
				colliderTwo.GetComponent<sumo.sumo_VehicleMovementV2> ().bump = -(Vector2)(colliderTwo.transform.position - colliderOne.transform.position) * (bounceStrength);				
			}
		}
	}
}
