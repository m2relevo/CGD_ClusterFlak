using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class ShellShock_Weapon_RPG : ArmadHeroes.Weapon
    {
        void Awake()
        {
            SetWeaponCharacteristics(45, 0, 1.5f, 10, 10, 5);
            ObjectPoolBullets();
        }
    }
}
