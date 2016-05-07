using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class NewAssaultRifle : NewWeapon
    {
        protected override void Start()
        {
            base.Start();

            mDamage = 20;
            mTimeReloading = 0f;
            mTimeToReload = 2f;
            mSpreadAngle = 10f;
            mFireRate = 0.1f;
            mChargeTime = 0f;
            mProjectileSpeed = 50f;
            mRecoveryTime = 0.1f;
            mTimeRecovering = 1f;
            mTimeCharging = 0f;
            mTotalRounds = 100;
            mCurrentRounds = 100;
            mMagazineCapacity = 10;
            mRoundsInMagazine = 10;
            mRecoilForce = 0f;

            mMaxNumActiveProjectiles = 30;
            mNumberOfBounces = 3;

            //base.InitialiseProjectilePool();
        }
    }
}
