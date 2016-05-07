using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_MineDropper : Weapon
    {

        // Use this for initialization
        void Awake()
        {
            SetWeaponCharacteristics(1.0f, 0.0f, 0.01f, 0.0f, 0.0f, 1);
            ObjectPoolBullets();
        }
    }
}