using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class DynamicWallScript : MonoBehaviour
    {

        public DynamicWallManager wallManager;
        private Animator wallAnimator;
        public int wallGroupID;

        [SerializeField]
        bool dilloDashLogic = false;
       // bool down = false;
        //public Animation wallAnim;

        // Use this for initialization
        void Start()
        {
            wallAnimator = GetComponent<Animator>();
            //wallAnim = GetComponent<ShellShock.Animation> ();
            wallAnimator.SetBool("isDown", false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WallChange(int wallGroup)
        {
            if (wallGroupID == wallGroup)
            {
                //Debug.Log("Wall Change");
                if (wallAnimator.GetBool("isDown") == true) //Walls are down
                {
                    WallsUp();
                    //Debug.Log("Wall Up");
                }

                else if (wallAnimator.GetBool("isDown") == false)
                {
                    WallsDown();
                    //Debug.Log("Wall Down");
                }
            }


        }

        public void WallsDown()
        {
            wallAnimator.SetBool("isDown", true); //Walls go down
            GetComponent<PolygonCollider2D>().enabled = dilloDashLogic ? true : false;

            if (dilloDashLogic)
            {
                GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            }
            //WallChange(0);
        }

        public void WallsUp()
        {
            wallAnimator.SetBool("isDown", false); //Walls go up
            GetComponent<PolygonCollider2D>().enabled = true;
            //WallChange(0);

            if (dilloDashLogic)
            {
                GetComponent<SpriteRenderer>().sortingLayerName = "Midground";
            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (dilloDashLogic)
            {
                if (coll.tag == "DilloDash/APC")
                {
                    WallsDown();

                }
            }
        }

        void OnTriggerExit2D(Collider2D coll)
        {
            if (dilloDashLogic)
            {
                if (coll.tag == "DilloDash/APC")
                {

                    WallsUp();

                }
            }
        }
    }
}
