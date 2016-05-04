using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class PlayerActions : MonoBehaviour
    {
        public GameObject ballObj;
        public ShellShock.Player playerScript;
        public bool isBallin = false; //curled up in a ball
        public bool canBall = true;
        public int weaponNumber;
        private SpriteRenderer playerRenderer;
        private Collider2D playerCollider;

        public float timeSpentBallin = 0.0f;
        public float timeSpentNotBallin = 5.0f;

        void Start()
        {
            playerCollider = GetComponent<Collider2D>();
            playerRenderer = GetComponent<SpriteRenderer>();
            playerScript = GetComponent<ShellShock.Player>();
        }

        void Update()
        {
            if (Input.GetButton("Player_" + GetComponent<ShellShock.Player>().PlayerNumber + "_Ball") && timeSpentBallin < 2.0f)
            {
                Debug.Log(GetComponent<ShellShock.Player>().PlayerNumber);
                isBallin = true;
                PlayerRender();
                GetComponent<ShellShock.Aiming>().ReticleRender();
                ballObj.GetComponent<ShellShock.Ball>().BallRender();
            }

            if (Input.GetButtonUp("Player_" + GetComponent<ShellShock.Player>().PlayerNumber + "_Ball") || timeSpentBallin > 3.0f)
            {
                isBallin = false;
                PlayerRender();
                GetComponent<ShellShock.Aiming>().ReticleRender();
                ballObj.GetComponent<ShellShock.Ball>().BallRender();
            }

            if (isBallin)
            {
                if (timeSpentBallin < 3.0f)
                {
                    timeSpentBallin += Time.deltaTime;
                }
                if (timeSpentNotBallin < 0.0f)
                {
                    timeSpentNotBallin -= Time.deltaTime;
                }
            }
            else
            {
                timeSpentBallin -= Time.deltaTime;
                timeSpentNotBallin += Time.deltaTime;
            }


            //if (Input.GetButtonUp("Player_" + GetComponent<ShellShock.Player>().PlayerNumber + "_Switch"))
            //{
            //    if (weaponNumber < playerScript.weaponList.Length)
            //    {
            //        weaponNumber++;
            //        playerScript.ChangeWeapon(weaponNumber);
            //        Debug.Log(weaponNumber);

            //    }
            //    if (weaponNumber == playerScript.weaponList.Length)
            //    {
            //        weaponNumber = 0;
            //        playerScript.ChangeWeapon(weaponNumber);
            //    }
            //}

        }

        void PlayerRender()
        {
            if (isBallin == false)
            {
                playerCollider.enabled = true;
                playerRenderer.enabled = true;
            }

            if (isBallin == true)
            {
                playerCollider.enabled = false;
                playerRenderer.enabled = false;
            }
        }
    }
}
