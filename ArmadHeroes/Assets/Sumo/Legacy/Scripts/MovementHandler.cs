    //TEAM SUMO       
   //Gareth Griffiths  
  //Movement Handler  
 //
//Handles the user imput, passing direction to the appropiate scripts

using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    GameObject[] player;


    //Direction
    public enum Direction {North, NorthEast, East, SouthEast, South, SouthWest, West,NorthWest,Center}
    public Direction playerDirection;
    public Vector3 playerVector;

    //Sprites
    Vector3 scale, invScale;
    sumo_SpriteList _spriteList;

    void Start()
    {
        _spriteList = this.GetComponent<sumo_SpriteList>();
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        scale = this.transform.localScale;
        invScale = scale;
        invScale.x = -invScale.x;
    }
    void Update()
    {

        //No direction
        //playerDirection = Direction.Center;

        //Initial Direction Boundary
        //Scalar Siee Boundary A
        //Scalar Side Boundary B

        //If stick is UP
        if (Input.GetAxis("LeftStickY1") > 0 &&
            Input.GetAxis("LeftStickX1") > -0.5 * Input.GetAxis("LeftStickY1") &&
            Input.GetAxis("LeftStickX1") < 0.5 * Input.GetAxis("LeftStickY1"))
        {
            playerDirection = Direction.North;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("N")];
            this.transform.localScale = scale;
        }

        //If stick is UP,RIGHT
        else if (Input.GetAxis("LeftStickX1") > 0 && Input.GetAxis("LeftStickY1")>0)

        {
            playerDirection = Direction.NorthEast;


            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
            this.transform.localScale = scale;
        }

        //If stick is RIGHT
        else if (Input.GetAxis("LeftStickX1") > 0 &&
                Input.GetAxis("LeftStickY1") > -0.5 * Input.GetAxis("LeftStickX1") &&
                Input.GetAxis("LeftStickY1") < 0.5 * Input.GetAxis("LeftStickX1"))
        {
            playerDirection = Direction.East;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
            this.transform.localScale = scale;
        }

        //If stick is DOWN, RIGHT
        else if (Input.GetAxis("LeftStickX1") > 0 && Input.GetAxis("LeftStickY1") < 0)
        {
            playerDirection = Direction.SouthEast;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
            this.transform.localScale = scale;
        }

        //If stick is DOWN
        else if (Input.GetAxis("LeftStickY1") < 0 && 
                Input.GetAxis("LeftStickX1") > -0.5 * Mathf.Abs(Input.GetAxis("LeftStickY1")) && 
                Input.GetAxis("LeftStickX1") < 0.5 * Mathf.Abs(Input.GetAxis("LeftStickY1")))
        {
            playerDirection = Direction.South;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("S")];
            this.transform.localScale = scale;
        }

        //If stick is DOWN, LEFT
        else if (Input.GetAxis("LeftStickX1") < 0 && Input.GetAxis("LeftStickY1") < 0)
        {
            playerDirection = Direction.SouthWest;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
            this.transform.localScale = invScale;
        }

        //If stick is LEFT
        else if (Input.GetAxis("LeftStickX1") < 0 && 
            Input.GetAxis("LeftStickY1") > -0.5 * Mathf.Abs(Input.GetAxis("LeftStickX1")) && 
            Input.GetAxis("LeftStickY1") < 0.5 * Mathf.Abs(Input.GetAxis("LeftStickX1")))
        {
            playerDirection = Direction.West;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
            this.transform.localScale = invScale;
        }

        //If stick is UP,LEFT
        else if (Input.GetAxis("LeftStickX1") < 0 && Input.GetAxis("LeftStickY1") > 0)
        {
            playerDirection = Direction.NorthWest;

            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
            this.transform.localScale = invScale;
        }

        else
        {
            playerDirection = Direction.Center;
            this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("Ball")];

        }
    }


}
