using UnityEngine;
using System.Collections;

namespace sumo
{
    public class _sumo_VehicleMovementV3 : MonoBehaviour
    {
        Quaternion angle;
        Vector2 vector;
        Vector2 direction;

        sumo_SpriteList _spriteList;
        Vector3 scale, invScale;
        string state;
        public float speed = 1.0f, turnSpeed = 1.0f;
        public float baseSpeed, acceleration, boostSpeed, boostDecay;
        public float holdSpeed;

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

            if (state != "boost")
            {
                setDirection();
                angle = angle * Quaternion.Euler(0.0f, 0.0f, 0.01f);

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("LeftStickX1") < 0.0f)
                {
                    direction = Quaternion.Euler(0.0f, 0.0f, turnSpeed) * direction;
                }
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("LeftStickX1") > 0.0f)
                {
                    direction = Quaternion.Euler(0.0f, 0.0f, -turnSpeed) * direction;
                }
            }

            if (state == "boost")
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("Ball")];
                speed -= boostDecay;
                holdSpeed = speed;
                if (speed <= baseSpeed)
                {
                    speed = holdSpeed;
                    state = "still";
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("R11") > 0.0f)
            {
                if (speed < baseSpeed)
                {
                    speed += acceleration;
                }
            }
            else
            {
                if (speed > 0)
                {
                    speed -= acceleration;
                }
                else
                {
                    speed = 0;
                }
            }

            //GetComponent<Rigidbody2D>().AddForce(new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed));
            GetComponent<Rigidbody2D>().velocity = new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed);


            if (Input.GetKey(KeyCode.Space))
            {
                boostSpeed = boostSpeed + 0.1f;
            }

            //if space pressed, set state boost
            if (Input.GetKeyUp(KeyCode.Space))
            {
                speed = boostSpeed;
                state = "boost";
				boostSpeed = 0;
            }


        }



        void setDirection()
        {

            Debug.Log(direction.x + " : " + direction.y);
            //N
            if (direction.x > 0.5f && direction.x < 1.25f && direction.y > 0.5f && direction.y < 1.25f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("N")];
                this.transform.localScale = scale;
            }
            //NE
            if (direction.x > 1.25f && direction.y > -0.5f && direction.y < 0.5f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
                this.transform.localScale = scale;
            }
            //E
            if (direction.x > -0.5f && direction.x < 1.25f && direction.y > -1.25f && direction.y < -0.5f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
                this.transform.localScale = scale;
            }
            //SE
            if (direction.x > -0.5f && direction.x < 0.5f && direction.y < -1.25f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
                this.transform.localScale = scale;
            }
            //S
            if (direction.x > -1.25f && direction.x < -0.5f && direction.y > -1.25f && direction.y < -0.5f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("S")];
                this.transform.localScale = scale;
            }
            //SW
            if (direction.x < -1.25f && direction.y > -0.5f && direction.y < 0.5f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("SE")];
                this.transform.localScale = invScale;
            }
            //W
            if (direction.x > -1.25f && direction.x < -0.5f && direction.y > 0.5f && direction.y < 1.25f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("E")];
                this.transform.localScale = invScale;
            }
            //NW
            if (direction.x > -0.5f && direction.x < 0.5f && direction.y > 1.25f)
            {
                this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("NE")];
                this.transform.localScale = invScale;
            }
        }
    }
}
