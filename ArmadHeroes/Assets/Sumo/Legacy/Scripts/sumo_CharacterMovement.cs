//Armad Heros 
//SUMO 
//Gareth Griffiths, Peter Maloney, Alex Nuns, Jake Downing
//CharaterMovement
//
//Handles player charchter "on foot" movement

using UnityEngine;

public class sumo_CharacterMovement : MonoBehaviour
{

    MovementHandler.Direction playerDirection;

    public Vector2 characterVelocity = new Vector3(0, 0);
    float speed = 1.0f;

    void Start()
    {
        //playerDirection = MovementHandler.Direction.Center;
    }

    void Update()
    {
        playerDirection = GetComponent<MovementHandler>().playerDirection;

        //North Movement
        if(playerDirection == MovementHandler.Direction.North)
        {
            characterVelocity = new Vector2(0.0f, 1.0f);
        }

        //NorthEast Movement
        if (playerDirection == MovementHandler.Direction.NorthEast)
        {
            characterVelocity = new Vector2(0.75f, 0.75f);
        }

        //East Movement
        if (playerDirection == MovementHandler.Direction.East)
        {
            characterVelocity = new Vector2(1.0f, 0.0f);
        }

        // SouthEast Movement
        if (playerDirection == MovementHandler.Direction.SouthEast)
        {
            characterVelocity = new Vector2(0.75f, -0.75f);
        }

        //SouthMovement
        if (playerDirection == MovementHandler.Direction.South)
        {
            characterVelocity = new Vector2(0.0f, -1.0f);
        }

        //SouthWest Movement
        if (playerDirection == MovementHandler.Direction.SouthWest)
        {
            characterVelocity = new Vector2(-0.75f, -0.75f);
        }

        //West Movement
        if (playerDirection == MovementHandler.Direction.West)
        {
            characterVelocity = new Vector2(-1.0f, 0.0f);
        }

        //NorthWest Movement
        if (playerDirection == MovementHandler.Direction.NorthWest)
        {
            characterVelocity = new Vector2(-0.75f, 0.75f);
        }

        //No Direction
        if (playerDirection == MovementHandler.Direction.Center)
        {
            characterVelocity = new Vector2 (0,0);
        }

        //GetComponent<Rigidbody2D>().velocity = new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed);
    }

    void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().velocity = characterVelocity;
    }
}
