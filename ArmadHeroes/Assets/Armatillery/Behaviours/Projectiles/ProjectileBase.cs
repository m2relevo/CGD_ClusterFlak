/// <summary>
/// ProjectileBase is based from the projectile class
/// ProjectileBase is the base class to be used for all projectile types
/// Created and implemented by Daniel Weston - 10/01/16
/// </summary>
using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public class ProjectileBase : ArmadHeroes.Projectile
    {
        #region Public Members
        public GameObject explosion;
        #endregion

        #region Unity Callbacks
        protected override void Start()
        { 
            //set the countdown time equal to destruct
            CountDownTime = destructTime;
        }

        protected override void Update()
        {
            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    base.Update();
                    break;
                case GameStates.pause:
                    break;
                case GameStates.gameover:
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            //turn me off
            if (col.gameObject.GetComponent<Actor>())
            {
                if (type != col.gameObject.GetComponent<Actor>().type)
                {
                    this.gameObject.SetActive(false);
                }
            }

            if (col.GetComponent<EnvironmentSprite>())
            {
                explosion = ExplosionManager.instance.GetSmallExplosion();
                explosion.transform.position = this.transform.position;
                explosion.SetActive(true);
                explosion.GetComponent<Explosion>().InitExplode(1f);
            }
        }
        #endregion

        #region Projectile Behaviours
        public override void Fire(int _playerId, float height = 0, Collider2D _ignoreCollider = null)
        {
            this.gameObject.SetActive(true);
            callerID = _playerId;
            explosion = ExplosionManager.instance.GetExplosion();
            float rocketAngle = (Mathf.Rad2Deg * Mathf.Atan2(direction.x, -direction.y)) - 90;
            transform.localEulerAngles = new Vector3(0, 0, rocketAngle);
        }
        #endregion
    }
}
