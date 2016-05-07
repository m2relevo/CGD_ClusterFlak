using UnityEngine;
using System.Collections;
using ArmadHeroes;


namespace ZonePatrol
{
    public class FlagAnimScript : MonoBehaviour
    {

        private int counter = 0;
        private int flapInterval = 0;
        private int spriteNum = 0;

        public Sprite sprite1;
        public Sprite sprite2;
        // Use this for initialization
        void Start()
        {
            flapInterval = Random.Range(9, 12);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            counter++;
            if (counter >= flapInterval)
            {
                counter = 0;
                if(spriteNum == 1)
                {
                    spriteNum = 0;
                    //change to sprite 0 (1)
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
                }
                else
                {
                    spriteNum = 1;
                    //change to sprite 1 (2)
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
                }
                //flip flag sprite
                //gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            }
        }
    }
}
