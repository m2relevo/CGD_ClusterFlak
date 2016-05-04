using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_HeliRocketPod : Weapon
    {
        [SerializeField] private float mReloadCoolDown = 2.0f;
        private float mCurrentReloadCooldown = 2.0f;
        void Awake()
        {  
            SetWeaponCharacteristics(1.0f, 0.0f, 0.01f, 50.0f, 0.0f, 4);
            ObjectPoolBullets();
            mCurrentReloadCooldown = mReloadCoolDown;
        }

        public override void Tick()
        {
            base.Tick();
            if(mCurrentRounds == 0)
            {
                if(mCurrentReloadCooldown <= 0.0f)
                {
                    mCurrentReloadCooldown = mReloadCoolDown;
                    mCurrentRounds = 4;
                }
                else
                {
                    mCurrentReloadCooldown -= Time.deltaTime;
                }
            }           
        }
        //May put auto reload stuff

    }
}
