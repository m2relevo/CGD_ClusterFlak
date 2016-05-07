using UnityEngine;
using System.Collections;
namespace ArmadHeroes
{
    public class ShellShock_Weapon_FlameThrower : Weapon
    {
        public delegate void OnKill();

        #region public members
        public ParticleSystem flames;
        public OnKill onKillCallback;
        public BulletModifier bm;
        #endregion

        #region private members
        public ActorType type;
        private Renderer flamesRenderer;
        public Collider2D flameCollider;
        private AudioSource flameAudio;
        private float fuelBurnRate = 10.6f;
        private float currentFuel;
        Actor owner;
        private bool firing = false;
        #endregion

        void Awake()
        {
            mDamage = 1f;
            mProjectileSpeed = 0;
            mRecoilForce = 0;
            maxRounds = 50;
            mMagazineCapacity = 1;
            mCurrentRounds = 0;
            mNumberOfBounces = 0;
            mVibrationStrength = 0.1f;
            firesBullets = false;

            player = GetComponentInParent<Actor>();
            flamesRenderer = flames.GetComponent<Renderer>();

        }

        public override void Shoot(Vector3 _spawnPos, Vector3 _shootDir, int _shooter, Actor _owner, Color colour, ActorType _type, BulletModifier _modifiers = BulletModifier.vanilla, float height = 0)
        {
            owner = _owner;
            bm = _modifiers;

            if (currentFuel > 0 || infiniteAmmo)
            {
                type = _type;

                UseAmmo();

                ChangeFuel(-fuelBurnRate);
                transform.position = _spawnPos;
                float fireAngle = mSpreadAngle;

                mSpreadAngle = Mathf.Atan2(_shootDir.normalized.x, -_shootDir.normalized.y) * Mathf.Rad2Deg;
                fireAngle = mSpreadAngle;
                if (Mathf.Abs(mSpreadAngle) == 90)
                {
                    fireAngle -= 90;//0 or 180
                }
                else if (mSpreadAngle == 30 || mSpreadAngle == -150)
                {
                    fireAngle -= 60;//-30 or -210(30)
                }
                else if (mSpreadAngle == -30 || mSpreadAngle == 150)
                {
                    fireAngle -= 120;//-150 or 30
                }
                else
                { // 0 or 180
                    fireAngle -= 90; //-90 or 90 
                }

                if (_owner.transform.localScale.x > 0)
                    transform.eulerAngles = new Vector3(0, 0, fireAngle + 90);
                else
                    transform.eulerAngles = new Vector3(0, 0, -fireAngle - 90);

                if (flames.transform.parent != transform)
                    flames.transform.eulerAngles = new Vector3(0, 0, fireAngle + 90);


                flamesRenderer.sortingOrder = (fireAngle == -270 || fireAngle == 90) ? _owner.spriteRenderer.sortingOrder - 1 : _owner.spriteRenderer.sortingOrder + 1;
                firing = true;
                flames.Play();
                flameCollider.enabled = true;

                //Vibrate controller
                if (_owner.gameObject.GetComponent<PlayerActor>())
                {
                    _owner.gameObject.GetComponent<PlayerActor>().controller.StartVibration(mVibrationStrength, 0.1f);
                }

                //Audio
                if (flameAudio == null)
                {
                    flameAudio = SoundManager.instance.PlayClip(ShotSound, transform.position, true);
                }

                SoundManager.instance.SetPan(transform.position, flameAudio);

            }
            else
            {
                StopFire();
            }
        }

        public override void Reload()
        {
            base.Reload();
            currentFuel = mCurrentRounds;
        }

        public void ChangeFuel(float change)
        {
            currentFuel += change * Time.deltaTime;
            mCurrentRounds = (int)currentFuel;
        }

        public override void StopFire()
        {
            base.StopFire();
            firing = false;
            flameCollider.enabled = false;
            flames.Stop();
            SoundManager.instance.FadeAndKillAudio(flameAudio, 0.5f);
            flameAudio = null;
        }
        void UpdateRotation()
        {
            transform.rotation = player.transform.rotation;
        }

        void OnTriggerStay2D(Collider2D col)
        {

            //Ignore gravity
            if (!col.usedByEffector)
            {
                //If not ignore collider (player)
                if (col != player.bodyCollider)
                {
                    if (col.gameObject.GetComponent<Actor>() && firing)
                    {

                        Actor hitActor = col.gameObject.GetComponent<Actor>();
                        if (hitActor.type == ActorType.None || hitActor.type != type)
                        {

                            hitActor.TakeDamage(mDamage);
                            //hitActor.Burnt();
                            if (hitActor.health <= 0)
                            {
                                ShellShock.PlayerLogic playerLogic = col.gameObject.GetComponent<ShellShock.PlayerLogic>();
                                playerLogic.timeToRespawn = 3f;
                                playerLogic.ChangeKillersScore(owner.playerNumber, col.gameObject.GetComponent<ShellShock.RewiredController>().playerNumber);
                                ShellShock.RespawnManager.Instance.Kill(col.gameObject, playerLogic.timeToRespawn);
                                playerLogic.ResetPlayerProperties();

                                if ((bm & BulletModifier.explodeOnDeath) != 0)
                                {
                                    GameObject explosion = Armatillery.ExplosionManager.instance.GetBigExplosion();
                                    explosion.transform.position = col.transform.position;
                                    explosion.SetActive(true);
                                    explosion.GetComponent<Explosion>().InitExplode(1f, owner);
                                }

                                if (onKillCallback != null)
                                {
                                    onKillCallback();
                                }
                            }
                        }

                    }
                }
            }

        }
    }
}
