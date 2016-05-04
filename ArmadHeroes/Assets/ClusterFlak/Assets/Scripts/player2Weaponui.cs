using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class player2Weaponui : MonoBehaviour
{
    public Text WeaponPlayer;

    private static string UIWeapon;

    // Use this for initialization
    void Start()
    {
        WeaponUI();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponUI();
    }

    void WeaponUI()
    {
        UIWeapon = CluFlaPlayer2Movement.Weapon;

        if (UIWeapon == "Pistol")
        {
            Debug.Log("This is a debuglog from the WeaponUI. The gun is pistol.");
        }

        if (UIWeapon == "MachineGun")
        {
            Debug.Log("This is a debuglog from the WeaponUI. The gun is a machinegun");
        }

        WeaponPlayer.text = UIWeapon;
    }

    void Fixedupdate()
    {

    }
}