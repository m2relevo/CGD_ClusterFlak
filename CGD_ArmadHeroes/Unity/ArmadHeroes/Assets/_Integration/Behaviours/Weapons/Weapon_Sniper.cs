using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_Sniper : Weapon
    {
        protected void Awake()
        {
            SetWeaponCharacteristics(70, 0, 1.0f, 25.0f, 2f, 10);
            ObjectPoolBullets();
        }
    }
}
