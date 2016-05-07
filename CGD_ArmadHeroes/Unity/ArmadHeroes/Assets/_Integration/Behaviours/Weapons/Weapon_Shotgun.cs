using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Weapon_Shotgun : Weapon
    {
        
        protected virtual void Awake()
        {
            shells = true;
            SetWeaponCharacteristics(3, 0.05f, 0.9f, 20.0f, 1f, 2);
            ObjectPoolBullets();
			mVibrationStrength = 0.5f;
			mVibrationTime = 0.3f;
			mRandomSpread = false;
        }  

		public override void Shoot (Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner, Color colour, ActorType _type = ActorType.Player, BulletModifier _modifiers = BulletModifier.vanilla, float height = 0)
		{
			
			if (infiniteAmmo)
			{
				mCurrentRounds = 15;
			}
			if (mCurrentRounds > 0)
			{
				if (CheckCoolDown())
				{

					TriggerFireSound();
					UseAmmo();

					for (int i = -3; i < 4; i++) {
						Vector3 shootDir = _shootDir.normalized;
						shootDir.x += mSpreadAngle * i;
						shootDir.y += mSpreadAngle * i;

						FireProjectile (_spawnPos,shootDir,_shooter,_owner,_type,_modifiers,height);
					}


					//Vibrate controller
					if (_owner.gameObject.GetComponent<PlayerActor>())
					{
						_owner.gameObject.GetComponent<PlayerActor>().controller.StartVibration(mVibrationStrength, mVibrationTime);
					}

					//Only emit when on ground
					if (bulletCasingParticleSystem != null && height == 0)
					{
						bulletCasingParticleSystem.Emit(1);
					}
					if (muzzleFlashParticleSystem != null)
					{
						muzzleFlashParticleSystem.Emit(1);
					}
				}
			}
            
        }
    }
}
