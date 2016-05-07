using UnityEngine;
using System.Collections;

namespace sumo
{
    public class sumo_PlayerLives : MonoBehaviour
    {

        public int lives;

        public void LoseLife()
        {
            if (lives > 1)
            {
                
                lives -= 1;
            }

            else
            {
                lives -= 1;
                if (FindObjectOfType<ScoringManager>())
                {
					//indObjectOfType<ScoringManager>().addLoser(this.GetComponentInParent<sumo_VehicleMovementV2>().playerNumber, this.gameObject);
                }
                if(FindObjectOfType<sumo_RoundManager>())
                {
                    FindObjectOfType<sumo_RoundManager>().addLoser(1, this.gameObject);
                }  
                this.gameObject.SetActive(false);
             
                //LOSE
            }
        }
    }
}

