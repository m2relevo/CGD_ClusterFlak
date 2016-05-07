//Armad Heros 
//SUMO 
//Gareth Griffiths, Peter Maloney, Alex Nuns, Jake Downing
//Vehichle movement v2
//
//Handles vehichle controll and movement

using UnityEngine;
using ArmadHeroes;

namespace sumo
{
	public class sumo_VehicleMovementV2 : PlayerActor
	{
        //Used by custom collision system
        public Vector2 bump;
		public Vector2 direction;
		public float speed = 1.0f, turnSpeed = 1.0f;
		bool isMoving;

        sumo_SpriteList _spriteList;

		Vector3 scale, invScale;
		public string state;
		public float baseSpeed, acceleration , boostSpeed, boostDecay, maxSpeed;
		public float holdSpeed;

        public GameObject sumo_bodyCollider;       
        
        public bool constantMove;

        public Vector2 hitDirection;
        public float hitSpeed;

        public Vector2 vely;

		public GameObject parachute;


		public void Init(int _playerNumber, int _controllerNum, string _name)
		{
			playerID = _playerNumber;
			m_controllerID = _controllerNum;
			ActorName = _name;
			m_override.SetCharacter (ActorName);
		}
		
		// Use this for initialization
		void Start()
		{
			//
			constantMove = false;
			state = "still";
			_spriteList = this.GetComponent<sumo_SpriteList>();
			this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

			direction = new Vector2(1.0f, 1.0f);
			accolade_unique = 0;
			scale = this.transform.localScale;
			invScale = scale;
			invScale.x = -invScale.x;

        }
		
		// Update is called once per frame
		void Update()
		{
			base.Update ();

			accolade_unique += Time.deltaTime;
            //Debug.Log(GetComponent<Rigidbody2D>().velocity);
            //To be able to see current velocity (for debugging)
            vely = GetComponent<Rigidbody2D>().velocity;
            //if player is not boosting it can TURN 

			if (speed > 0 && !isMoving) 
			{
				isMoving = true;
				m_armaAnimator.SetBool ("enterBall", isMoving);
				m_armaAnimator.SetBool ("exitBall", !isMoving);
			} 
			else if (speed <= 0 && isMoving) 
			{
				isMoving = false;
				m_armaAnimator.SetBool ("exitBall", !isMoving);
				m_armaAnimator.SetBool ("enterBall", isMoving);
			}

            if (state != "boost") 
			{	
                //Sets sprite direction	
               	setDirection ();

				m_armaAnimator.SetFloat ("angle", Vector2.Angle(direction, new Vector2(-1f,-1f)));
				//Debug.Log (Vector2.Angle (direction, new Vector2 (1f, 1f)));
                //For more vehicle like behaviour, cant turn while stopped (on the spot)
			
                if (speed > 0)
                {				
					
                    //If LEFT is held, rotate facing to the left            
					if(controller.moveX.GetValue() < 0 || Input.GetKey(KeyCode.LeftArrow) && playerNumber == 1 || Input.GetKey(KeyCode.A) && playerNumber == 2 || (Input.GetAxis("LeftStickX1") < 0.0f && playerNumber == 1) || (Input.GetAxis("RightStickX1") < 0.0f && playerNumber == 2))
					{
                        direction = Quaternion.Euler(0.0f, 0.0f, turnSpeed) * direction;
                    }
                    //If RIGHT is held, rotate facing to the right
					if(controller.moveX.GetValue() > 0 || Input.GetKey(KeyCode.RightArrow) && playerNumber == 1 || Input.GetKey(KeyCode.D) && playerNumber == 2 || (Input.GetAxis("LeftStickX1") > 0.0f && playerNumber == 1) || (Input.GetAxis("RightStickX1") > 0.0f && playerNumber == 2))
					{
                        direction = Quaternion.Euler(0.0f, 0.0f, -turnSpeed) * direction;
                    }
                }
			}			
			//If player is booting, player essentially has no control until boost is finished
			if (state == "boost")
			{
                //Set sprite to ball sprite
				//this.GetComponent<SpriteRenderer>().sprite = _spriteList.spriteArray[_spriteList.spriteNames.IndexOf("Ball")];
                //decrease speed over duration of boost
				speed -= boostDecay;
				holdSpeed = speed;
                //if speed is now longer above normal top speed, player is no longer boosting
				if (speed <= baseSpeed)
				{
                    //no longer in boost state
					speed = holdSpeed;
					state = "still";
				}
			}
            //If vehicles are not set to be always moving forward
            if (!constantMove)
            {
                //If the player presses FORWARD, increase speed by acceleration			             
				if(controller.accelerateButton.IsDown() || Input.GetKey(KeyCode.UpArrow) && playerNumber == 1 || Input.GetKey(KeyCode.W) && playerNumber == 2 || (Input.GetAxis("L11") > 0.0f && playerNumber == 1) || (Input.GetAxis("R11") > 0.0f && playerNumber == 2))
				{
                    //Only 'accelerate' if not at max speed
                    if (speed < baseSpeed)
                    {
                        speed += acceleration;
                    }
                }
                //If player is not pressing FORWARD
                else
                {
                    //Decrease speed if it is greater than 0;
                    if (speed > 0)
                    {
                        speed -= acceleration;
                    }
                    //incase speed has been decreased below 0, set it to be 0
                    else
                    {
                        speed = 0;
                    }
                }
            }
            //If vehicles set to have constant speed, speed is always set as max speed
            else
            {
                speed = baseSpeed;
            }				
			
			//if space pressed, set state boost
			if(controller.boostButton.JustPressed() || Input.GetKeyDown(KeyCode.Space) && playerNumber == 1 || Input.GetKey(KeyCode.LeftControl) && playerNumber == 2 || (Input.GetAxis("L21") > 0.0f && playerNumber == 1))
			{
                //Set speed to max boost speed
				speed = boostSpeed;
				state = "boost";
			} 

			accolade_distance += speed / 600;

			if (speed > 0) 
			{
				
			}


		}

        void FixedUpdate()
        {
            //If player is not moving at max speed, add force in current direction
            if (vely.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed));
            }
            //Also add force constantly while boosting
            else if (state == "boost")
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed));
            }

			if (bump != Vector2.zero) 
			{
				GetComponent<Rigidbody2D> ().AddForce (bump);
				bump = Vector2.zero;
			}
            //Old version where velocity was set directly. Didn't produce acceleration behaviours like new version deos but may be useful for other reasons
            //GetComponent<Rigidbody2D>().velocity = new Vector2(ExtensionMethods.toIso(direction).x * speed, ExtensionMethods.toIso(direction).y * speed); 

        }


		void OnCollisionEnter2D(Collision2D collision)
		{		
            //Debug.Log("Collision");
			{
				sumo_CustomCollisions.instance.collisionID += 1;
				// Debug.Log(CustomCollisions.instance.collisionID % 4);
				if ((sumo_CustomCollisions.instance.collisionID % 2) == 0) {
					sumo_CustomCollisions.instance.collision (this.gameObject, collision.gameObject, sumo_CustomCollisions.instance.collisionID, collision.contacts [0]);
				}
			}

		

		}
		
		
		//Sets the player sprite depending on facing
		public void setDirection()
		{


			//N
			if (direction.x > 0.5f && direction.x < 1.25f && direction.y > 0.5f && direction.y < 1.25f)
			{
				this.transform.localScale = scale;
			}
			//NE
			if (direction.x > 1.25f && direction.y > -0.5f && direction.y < 0.5f)
			{
				
                this.transform.localScale = scale;
			}
			//E
			if (direction.x > -0.5f && direction.x < 1.25f && direction.y > -1.25f && direction.y < -0.5f)
			{
				
                this.transform.localScale = scale;
			}
			//SE
			if (direction.x > -0.5f && direction.x < 0.5f && direction.y < -1.25f)
			{
				
                this.transform.localScale = scale;
			}
			//S
			if (direction.x > -1.25f && direction.x < -0.5f && direction.y > -1.25f && direction.y < -0.5f)
			{
				
                this.transform.localScale = scale;
			}
			//SW
			if (direction.x < -1.25f && direction.y > -0.5f && direction.y < 0.5f)
			{
				
                this.transform.localScale = invScale;
			}
			//W
			if (direction.x > -1.25f && direction.x < -0.5f && direction.y > 0.5f && direction.y < 1.25f)
			{
				
                this.transform.localScale = invScale;
			}
			//NW
			if (direction.x > -0.5f && direction.x < 0.5f && direction.y > 1.25f)
			{
				this.transform.localScale = invScale;
			}
		}
	}

}
