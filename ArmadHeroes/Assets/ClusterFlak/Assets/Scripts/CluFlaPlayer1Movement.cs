using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ArmadHeroes;



public class CluFlaPlayer1Movement : MonoBehaviour
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
    private bool Shotty = false;
    private int AmmoCountHeavy = 0;
    private int AmmoCountShotgun = 0;
    private bool ShotgunSwitch = false;

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

        if (Shotty == true)
        {
            shotgun();
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

        if (ControllerManager.instance.GetController(0).shootButton.JustPressed())
        {
            projectile();
            Debug.Log("Player Fire");
        }
        
        if (ControllerManager.instance.GetController(0).shootButton.JustPressed() && RFSwitch == true)
        {
            Repeatfire = true;
            
            if (AmmoCountHeavy  <=0)
            {
                RFSwitch = false;
            }
        }

        if (ControllerManager.instance.GetController(0).shootButton.JustPressed() && ShotgunSwitch == true)
        {
            shotgun();

            if (AmmoCountHeavy <= 0)
            {
                RFSwitch = false;
            }
        }

        if (ControllerManager.instance.GetController(0).shootButton.JustPressed() && RFSwitch == true)
        {
            Repeatfire = true;
            
            if (AmmoCountHeavy  <=0)
            {
                RFSwitch = false;
            }
        }

        if (ControllerManager.instance.GetController(0).shootButton.JustReleased())
        {
            //projectile();
            Repeatfire = false;
            Debug.Log("Fire over!!! (Rapid Fire Off)");
        }

        if (ControllerManager.instance.GetController(0).shootButton.JustReleased() && RFSwitch == true)
        {
            //projectile();
            Repeatfire = false;
            Debug.Log("Fire over!!! (Rapid Fire Off)");
        }


        if /*(Input.GetKey(KeyCode.W))*/ (ControllerManager.instance.GetController(0).d_up.IsDown())
        {
            transform.position += new Vector3(0f, movSpeed * Time.deltaTime, 0f);
        }

        if (ControllerManager.instance.GetController(0).d_down.IsDown())
        {
            transform.position -= new Vector3(0f, movSpeed * Time.deltaTime, 0f);
        }

        if (ControllerManager.instance.GetController(0).d_left.IsDown())
        {
            transform.position -= new Vector3(movSpeed * Time.deltaTime, 0f, 0f);
        }

        if (ControllerManager.instance.GetController(0).d_right.IsDown())
        {
            transform.position += new Vector3(movSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    void projectile()
    {
        GameObject PlayaBoolit = (GameObject)Instantiate(Bullet);
        PlayaBoolit.transform.position = BulletLocation.transform.position;
      
    }

    void shotgun()
    {
        for (int i = 0; i < 8; i++)
        {
            int Spread = Random.Range(-10,10);

            GameObject PlayaBoolit = (GameObject)Instantiate(Bullet);
            PlayaBoolit.transform.position = BulletLocation.transform.position;
            PlayaBoolit.transform.Rotate(0,0,0+Spread,0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ClusterFlak/PowerUpHeavyMachine")
        {
            Debug.Log("Player1 Heavy Machine Gun Hit");
            Weapon = "MachineGun";
            RFSwitch = true;
            AmmoCountHeavy = 100;
            MaxPlayerAmmo = 100;
            CurrentAmmocount = 100;
            SetAmmoUI();
        }

        if (col.gameObject.tag == "ClusterFlak/PowerUpShotgun")
        {
            Debug.Log("Player1 Shotgun Pickup");
            Weapon = "Shotgun";
            ShotgunSwitch = true;
            AmmoCountShotgun = 8;
            MaxPlayerAmmo = 8;
            CurrentAmmocount = 8;
            SetAmmoUI();
        }
    }

    private void SetAmmoUI()
    {
        m_Slider.value = CurrentAmmocount;
    }
}