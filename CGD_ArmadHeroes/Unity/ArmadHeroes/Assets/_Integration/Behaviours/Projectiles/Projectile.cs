/// <summary>
/// Projectile.cs
/// Base class for integration projectiles 
/// Created by Chris, M., David, G & Daniel, W ~ 20/04/2016
/// </summary>
using UnityEngine;
using System.Collections;
using Astrodillos;

namespace ArmadHeroes
{
    public class Projectile : MonoBehaviour
    {
        #region Public Members
        public BulletModifier m_mods = BulletModifier.vanilla;
        public Vector3 direction;//bullet direction of travel
        public float destructTime = 5.0f;//the time it takes for the bullet to die
        public float moveSpeed;//translation speed of bullet
        public float damage;
        public int callerID;
        public GameObject shadow;
        public ArmadHeroes.Actor owner;
        public ActorType type;
        #endregion

        #region Protected Members
        protected float CountDownTime = 0.0f;
        #endregion

        #region Unity Callbacks
        protected virtual void Start()
        {
            //set the countdown time equal to destruct
            ResetAliveTime();
        }

        protected virtual void Update()
        {
            CountDown();
            transform.Translate((direction.normalized * moveSpeed) * Time.deltaTime, Space.World);
        }
        #endregion

        /// <summary>
        /// CountDown is called in the bullet update and 
        /// counts down the bullets active alive time
        /// </summary>  
        protected void CountDown()
        {
            //count down alive time 
            if (CountDownTime != -99)
            {
                CountDownTime -= 1.0f * Time.deltaTime;

                if (CountDownTime <= 0.0f)
                {
                    //reset object
                    this.gameObject.SetActive(false);
                    //reset timer
                    CountDownTime = destructTime;
                }
            }
        }

        public virtual void Fire(int _playerId, float height = 0, Collider2D _ignoreCollider = null)
        {
            callerID = _playerId;
            gameObject.layer = SpriteOrdering.CollsionLayerFromHeight(height);
			if (shadow) {
				shadow.transform.position = transform.position + new Vector3(0, -height - 0.33f, 0);
			}
            
            ResetAliveTime();
        }

        public void SetShadowActive(bool active)
        {
            shadow.SetActive(active);
        }

        void ResetAliveTime()
        {
            CountDownTime = destructTime;
        }

        public float GetCountdownTime()
        {
            return CountDownTime;
        }
    }
}
