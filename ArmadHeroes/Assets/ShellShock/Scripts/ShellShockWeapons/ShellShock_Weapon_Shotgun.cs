using UnityEngine;
using System.Collections;

public class ShellShock_Weapon_Shotgun : ArmadHeroes.Weapon_Shotgun
{

    // Use this for initialization
    protected override void Awake()
    {
        shells = true;
        SetWeaponCharacteristics(3, 0.05f, 0.9f, 20.0f, 1f, 2);
        ObjectPoolBullets();
        mVibrationStrength = 0.5f;
        mVibrationTime = 0.3f;
        mRandomSpread = false;
    }

    
}
