using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CluFlaPlayer2Movement : MonoBehaviour
{
    public float MaxPlayerAmmo;
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    private float CurrentAmmocount;

    public GameObject Bullet;
    public GameObject BulletLocation;

    public float movSpeed = 0f;
    public int bulletmax = 0;

    private float movex = 0f;
    private float movey = 0f;

    private bool RFSwitch = false;
    private bool Repeatfire = false;
    private int AmmoCountHeavy = 0;

    public static string Weapon;
    // Use this for initialization
    void Start()
    {
        Weapon = "Pistol";
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (Repeatfire == true)
        {
            projectile();
            AmmoCountHeavy--;
            CurrentAmmocount--;
            if (AmmoCountHeavy <= 0)
            {
                RFSwitch = false;
                Repeatfire = false;
                Weapon = "Pistol";
            }
        }
    }

    void Update()
    {
        SetAmmoUI();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            projectile();
            Debug.Log("Player Fire");
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && RFSwitch == true)
        {
            Repeatfire = true;

            if (AmmoCountHeavy <= 0)
            {
                RFSwitch = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            //projectile();
            Repeatfire = false;
            Debug.Log("Fire over!!! (Rapid Fire Off)");
        }

        if (Input.GetKeyUp(KeyCode.Space) && RFSwitch == true)
        {
            //projectile();
            Repeatfire = false;
            Debug.Log("Fire over!!! (Rapid Fire Off)");
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0f, movSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0f, movSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(movSpeed * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(movSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    void projectile()
    {
        GameObject PlayaBoolit = (GameObject)Instantiate(Bullet);
        PlayaBoolit.transform.position = BulletLocation.transform.position;

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ClusterFlak/PowerUpHeavyMachine")
        {
            Debug.Log("Player2 Heavy Machine Gun Hit");
            Weapon = "MachineGun";
            RFSwitch = true;
            AmmoCountHeavy = 100;
            MaxPlayerAmmo = 100;
            CurrentAmmocount = 100;
            SetAmmoUI();
        }
    }

    private void SetAmmoUI()
    {
        m_Slider.value = CurrentAmmocount;
    }
}