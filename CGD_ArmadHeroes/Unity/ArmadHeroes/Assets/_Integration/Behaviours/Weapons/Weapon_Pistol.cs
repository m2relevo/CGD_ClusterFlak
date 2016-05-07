using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_Pistol : Weapon
    {
        protected void Awake()
        {
            SetWeaponCharacteristics(15, 0, 0.5f, 20.0f, 1f, 15);
            ObjectPoolBullets();
        }
    }
}