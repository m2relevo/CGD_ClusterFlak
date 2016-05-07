using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ArmadHeroes
{
    public class Weapon_Laser : Weapon
    {
        public AudioClip charge;

        private SpriteRenderer laserBeamSprite;
        private AudioSource chargeSource;
        int playerID;
		float height;
        Actor owner;

        public enum LaserState
        {
            InActive,
            Charging,
            Firing
        }

        public  LaserState laserState;
        Sequence chargeSequence;

        //If fire has been called this frame
        private bool isFiring = false;

        void Awake()
        {
			mVibrationStrength = 1.0f;
			mDamage = 40.0f;
			maxRounds = 5;
            mRecoilForce = 2.5f;
			firesBullets = false;
			m_projectile.GetComponent<Projectile> ().damage = mDamage;
            laserState = LaserState.InActive;

            laserBeamSprite = m_projectile.GetComponent<SpriteRenderer>();
        }

        public override void StopFire()
        {
            base.StopFire();
            if (!isFiring && laserState == LaserState.Charging)
            {
                SetInactive();
            }
        }

        public override void Shoot(Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner, Color colour,ActorType _type = ActorType.Player, 
            BulletModifier _modifiers = BulletModifier.vanilla,float _height = 0)
        {
            if (CheckCoolDown())
            {
				height = _height;
               
				if (laserState != LaserState.Firing)
				{
                    m_projectile.transform.position = _spawnPos;
					if (_owner.transform.localScale.x>0)
						m_projectile.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2 (_shootDir.y, _shootDir.x) * Mathf.Rad2Deg);
					else
						m_projectile.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2 (_shootDir.y, -_shootDir.x) * Mathf.Rad2Deg);
				}

                 switch (laserState)
                 {
                     case LaserState.InActive:
                         StartCharging(_spawnPos,_shootDir,_shooter,_owner);
                         break;

                 }
            }
        }

        void StartCharging(Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner)
        {
            playerID = _shooter;
            owner = _owner;

            if (owner.GetComponent<Armatillery.ArmaPlayer>())
            {
                _owner.GetComponent<Armatillery.ArmaPlayer>().LaserCharge.Play();
            }
            //grab bullet from pool and ready it for firing
          
            //StatTracker_Singleton.instance.AddToBulletsFired(_shooter);
            if (mCurrentCoolDown < mCoolDown)
            {
                //return;
            }
            m_projectile.SetActive(true);
            laserState = LaserState.Charging;
            m_projectile.transform.localScale = new Vector3(1, 0.1f, 1);
            laserBeamSprite.color = new Color(1, 1, 1, 0);

            chargeSource = SoundManager.instance.PlayClip(charge, transform.position);

            chargeSequence = DOTween.Sequence();
            chargeSequence.Insert(0, m_projectile.transform.DOScaleY(1.0f, 1.0f));
            chargeSequence.Insert(0, laserBeamSprite.DOColor(new Color(1, 1, 1, 1), 1.0f));
            chargeSequence.OnComplete(Shoot);
           	
			if (_owner.gameObject.GetComponent<PlayerActor> ()) {
				_owner.gameObject.GetComponent<PlayerActor> ().controller.StartVibration(0.1f, 1);
			}

            
        }

        void Shoot()
        {
            laserState = LaserState.Firing;
			isFiring = true;
            mCurrentCoolDown = mCoolDown;
			m_projectile.SetActive(true);
            //Stop charging audio
            if (chargeSource != null && chargeSource.clip == charge)
            {
                chargeSource.Stop();
            }
			///
			m_projectile.GetComponent<Projectile>().Fire(playerID, height);

            SoundManager.instance.PlayClip(ShotSound, transform.position);
			m_projectile.transform.localScale = new Vector3(1, 0.1f, 1);

			if (owner.gameObject.GetComponent<PlayerActor> ())
            {
                owner.gameObject.GetComponent<PlayerActor>().controller.StartVibration(mVibrationStrength, 0.5f);
			}
           
            Sequence shootSequence = DOTween.Sequence();
            shootSequence.Insert(0, m_projectile.transform.DOScaleX(800.0f, 0.3f));
            shootSequence.Insert(0, m_projectile.transform.DOScaleY(1.0f, 0.3f));
            shootSequence.Append(laserBeamSprite.DOColor(new Color(1, 1, 1, 0), 0.5f));

            shootSequence.OnComplete(() =>
            {
                UseAmmo();
                SetInactive();
            });

            mCoolDown = 0;

			//Stop charging audio
			if (chargeSource != null && chargeSource.clip == charge)
			{
				chargeSource.Stop();
			}

			SoundManager.instance.PlayClip(ShotSound, transform.position);
        }
			
        void SetInactive()
        {
            laserState = LaserState.InActive;
            m_projectile.SetActive(false);
            //Stop charging audio
            if (chargeSource != null)
            {
                SoundManager.instance.FadeAndKillAudio(chargeSource, 0.3f);
                chargeSource = null;
            }


            if (chargeSequence != null && chargeSequence.IsPlaying())
            {
                chargeSequence.Kill();
            }
        }

    }
}

