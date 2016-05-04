using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_MachineGun : Weapon
    {
        #region Unity Callbacks
        void Awake()
        {
			maxRounds = 50;
            ObjectPoolBullets();
            shells = true;
            SetWeaponCharacteristics(5, 0.02f, 0.3f, 10.0f, 0.5f, 30);
        }
        #endregion
    }
}
