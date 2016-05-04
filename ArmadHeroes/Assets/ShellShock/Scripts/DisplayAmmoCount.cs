using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ShellShock
{
    public class DisplayAmmoCount : MonoBehaviour
    {
        public Text ammoCount;
        public int playerID;
        public RewiredController player;

        void Awake()
        {
            player = transform.GetComponentInParent<RewiredController>();
        }

        void Update()
        {
            if (player.mSecondWeapon)
            {
                if (player.mSecondWeapon.infiniteAmmo)
                {
                    ammoCount.text = "∞";
                    ammoCount.fontSize = 55;
                }
                else
                {
                    ammoCount.text = player.mSecondWeapon.GetCurrentAmmo().ToString();
                    ammoCount.fontSize = 25;
                }
            }
        }
    }
}

