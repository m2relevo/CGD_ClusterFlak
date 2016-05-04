using UnityEngine;
using System.Collections;

namespace sumo
{
    public class _sumo_VehicleMovement : MonoBehaviour
    {
        Quaternion angle;
        Vector2 vector;
        Vector2 direction;
        public float speed = 1.0f, turnSpeed = 1.0f;
        sumo_SpriteList _spriteList;
        Vector3 scale, invScale;
        string state;
		public float baseSpeed, boostSpeed, boostDecay;

        // Use this for initialization
        void Start()
        {
            state = "still";
            _spriteList = this.GetComponent<sumo_SpriteList>();
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            angle = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            vector = new Vector2(1.0f, 1.0f);
            direction = new Vector2(1.0f, 1.0f);
            scale = this.transform.localScale;
            invScale = scale;
            invScale.x = -invScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
            //if still or walking, can turn, and hold forward to move
            if (state == "still" || state == "walking")
            {
                setDirection();
                angle = angle * Quaternion.Euler(0.0f, 0.0f, 0.1f);

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("LeftStickX1") < 0.0f)
                {
                    direction = Quaternion.Euler(0.0f, 0.0f, turnSpeed) * direction;
                }
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("LeftStickX1") > 0.0f)
                {
                    direction = Quaternion.Euler(0.0f, 0.0f, -turnSpeed) * direction;
                }
                

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("R11") > 0.0f)
                {
                    state = "walking";
                    //this.transform.position += new Vector3(ExtensionMethods.toIso(direction).x, ExtensionMethods.toIso(direction).y, 0.0f) * speed;
					GetComponent<Rigidbody2D>().AddForce(new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y)* speed);

               }
                else
                {
                    state = "still";
                }
            }

            //otherwise, boosting and cannot turn
            else if (state == "boost")
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("Ball")];
                speed -= boostDecay;
                 
				GetComponent<Rigidbody2D>().AddForce(new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed));			

                if (speed < baseSpeed)
                {
                    state = "still";
                }
            }   
                        
            //if space pressed, set state boost
            if(Input.GetKeyDown(KeyCode.Space))
            {
                speed = boostSpeed;
                state = "boost";
            }   


          
            
            


        }



        void setDirection()
        {
            //N
            if (direction.x > 0.8f && direction.x < 1.2f && direction.y > 0.8f && direction.y < 1.2f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("N")];
                this.transform.localScale = scale;
            }
            //NE
            if (direction.x > 1.3f && direction.x < 1.7f && direction.y > -0.2f && direction.y < 0.2f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
                this.transform.localScale = scale;
            }
            //E
            if (direction.x > 0.8f && direction.x < 1.2f && direction.y > -1.2f && direction.y < -0.8f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
                this.transform.localScale = scale;
            }
            //SE
            if (direction.x > -0.2f && direction.x < 0.2f && direction.y > -1.7f && direction.y < -1.3f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
                this.transform.localScale = scale;
            }
            //S
            if (direction.x > -1.2f && direction.x < -0.8f && direction.y > -1.2f && direction.y < -0.8f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("S")];
                this.transform.localScale = scale;
            }
            //SW
            if (direction.x > -1.7f && direction.x < -1.3f && direction.y > -0.2f && direction.y < 0.2f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
                this.transform.localScale = invScale;
            }
            //W
            if (direction.x > -1.2f && direction.x < -0.8f && direction.y > 0.8f && direction.y < 1.2f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
                this.transform.localScale = invScale;
            }
            //NW
            if (direction.x > -0.2f && direction.x < 0.2f && direction.y > 1.3f && direction.y < 1.7f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
                this.transform.localScale = invScale;
            }
        }
    }
}




