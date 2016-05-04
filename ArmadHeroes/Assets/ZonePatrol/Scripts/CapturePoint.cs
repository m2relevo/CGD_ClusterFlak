using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol
{
    public class CapturePoint : MonoBehaviour
    {
        private bool growing = false;
        private int ownerId = -1;
        private int invaderId = -1;
        private float timer = 0.0f;
        public float maxSize = 2.5f;
        private int playersNear = 0;
        private List<Player> playersNearList = new List<Player>();
        public Vector3 growingSpeed = new Vector3(0.0125f, 0.0f, 0.0075f);
        public Vector3 decreasingSpeed = new Vector3(0.025f, 0.0f, 0.015f);
        private Color defaultColor;
        public bool buildTurret = false;
        // Use this for initialization
        void Start()
        {
            defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collision");

            if (other.gameObject.tag == "Player")
            {
                playersNear++;
                Player player = other.gameObject.GetComponent<Player>();
                playersNearList.Add(player);
                if (ownerId == -1) // no owner (nuetral)
                {
                    invaderId = player.playerNumber;
                    growing = true;
                }
                else if (ownerId != -1 && player.playerNumber != ownerId)
                {
                    invaderId = player.playerNumber;
                    growing = false;
                }
                if (ownerId == player.playerNumber && growing == false)
                {
                    buildTurret = true;
                }
            }

        }

        public void FixedUpdate()
        {
            for(int i = 0;i<playersNearList.Count;i++)
            {
                if(!playersNearList[i].isAlive())
                {
                    playersNear--;
                    playersNearList.RemoveAt(i);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exit");
            if (other.gameObject.tag == "Player")
            {
                playersNear--;
                //if the enemy leaves the area then assign the invader to the owner
                if (invaderId != ownerId) // if the invader has not captured the base then degrade it
                {
                    if (ownerId != -1)
                    {
                        growing = true;
                    }
                    else
                    {
                        growing = false;
                    }
                }
                invaderId = ownerId;
                buildTurret = false;
            }
        }

		// Update is called once per frame
		void Update()
		{
            if (GameManager.instance.state != ArmadHeroes.GameStates.game)
            {
                // Dont update if the game is paused !
                return;
            }

            //point banking timer
            // if someone owns the flag
            if (ownerId != -1 && Time.time - timer > 1.0 && ownerId == invaderId) // if the base is currently under attack, do not add points
            {
                Debug.Log("points sent");
                PlayerManager.getInstance().getPlayerById(ownerId).GetComponent<Player>().addScore(); // magic nmumber
                ArmadHeroes.CanvasManager.instance.setPlayerValue(ownerId, PlayerManager.getInstance().getPlayerById(ownerId).GetComponent<Player>().getScore());
                timer = Time.time;
            }

            if (playersNear <= 1) // only change territory is there is only 1 player near
            {
                // growing if the size is lower than 
                if (growing && transform.localScale.x < maxSize)
                {
                    transform.localScale += growingSpeed;
                    if (transform.localScale.x >= maxSize) // if the the flag is captured set owner
                    {
                        if (ownerId != invaderId)
                        {
                            Debug.Log("new owner assigned");
                            timer = Time.time;
                            Debug.Log(invaderId);
                            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", PlayerManager.getInstance().getPlayerById(invaderId).GetComponent<Player>().color);
                            PlayerManager.getInstance().getPlayerById(invaderId).GetComponent<Player>().addZoneScore();

                        }
                        Debug.Log("captured");
                        ownerId = invaderId;
                    }
                }
                // Decreasing
                else if (!buildTurret && !growing && transform.localScale.x > 0.0f)
                {
                    // Decreasing but not decrease if the its held
                    transform.localScale -= decreasingSpeed;

                    // Change owner ship if there is a invader and it is completed decreased
                    if (transform.localScale.x <= 0.0f && ownerId != -1)
                    {
                        ownerId = -1;
                        growing = true;
                        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", defaultColor);
                    }
                }
            }
		}
	}
}
