using UnityEngine;
using System.Collections;


namespace ArmadHeroes
{
    public class Weapon_RPG : Weapon
    {
       
        void Awake()
        {
            SetWeaponCharacteristics(20, 0, 1.5f, 10, 10, 5);
            ObjectPoolBullets();
        }

    }
}