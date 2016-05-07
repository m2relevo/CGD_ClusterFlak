using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShellShock
{
    public class WeaponManager : MonoBehaviour
    {
        private static WeaponManager mInstance;
        public static WeaponManager Instance
        {
            get
            {
                return mInstance;
            }
        }

        public List<GameObject> mWeaponList;

        void Start()
        {
            mInstance = this;
        }

        public GameObject GetWeapon(int weaponNumber)
        {
            return Instantiate(mWeaponList[weaponNumber]);
        }

        public GameObject GetWeapon(string weaponName)
        {
            foreach (GameObject weapon in mWeaponList)
            {
                if (weapon.name == weaponName)
                {
                    return Instantiate(weapon);
                }
            }
            Debug.LogError("No Weapon With that name in weapon list");
            return null;
        }
    }
}
