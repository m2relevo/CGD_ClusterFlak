/*
 * Actor class stolen from Danieltillary, improved by astroChris 20/04/16
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Astrodillos;


namespace ArmadHeroes
{
    public enum ActorType
    {
		None,
        Player,
        Enemy
    };

    public abstract class Actor : MonoBehaviour
    {
        private ActorType m_type;
        public ActorType type { get { return m_type; } set { m_type = value; } }
        public float burnTime = 0;

        #region Public Members
        public CharacterAnimationOverride m_override;
        public ParticleSystem smokin;
        public int playerNumber
        {
            private set { }
            get { return playerID; }
        }
        public List<Projectile> m_projectiles = new List<Projectile>();
        public string ActorName = "Sarge";
        public float health = 100f,
            moveSpeed = 8f,
            knockBack;
        public SpriteRenderer spriteRenderer;
        public Weapon m_weapon;
        public ArmadHeroes.Weapon_MachineGun m_machinegun;
        public ArmadHeroes.Weapon_RPG m_RPG;
        public ArmadHeroes.Weapon_Shotgun m_shotgun;
        public ArmadHeroes.Weapon_Flamethrower m_flamethrower;
        public ArmadHeroes.Weapon_Laser m_laser;
        public ArmadHeroes.Weapon_Pistol m_pistol;
        public ArmadHeroes.Weapon_Sniper m_sniper;
        public AudioClip m_armed, m_damage;
        public Animator m_armaAnimator;
        public Collider2D bodyCollider;
		public Vector3 shootDir = new Vector3(1, 0, 0);

        public bool m_useDamageAnimation = true; //Should the actor flash on hit
        public float m_damageFlashDuration = 0.3f;
        #endregion

        #region Protected Members
        protected int playerID = 0;
        protected int xScale;
        protected float currentAngle = 0;
        protected float CoolDownTimer;//cool down timer between firing 
        protected float RandomKnockBack;//updated when player fires
        protected bool m_inDamageAnimation = false; //Is the enemy currently in damage animation
        #endregion

        #region Actor Behaviours
        /// <summary>
        /// calcs player knock back
        /// </summary>
        protected void CalcKnockBack()
        {
            Random.seed = System.DateTime.Now.Millisecond;
            RandomKnockBack = Random.Range(knockBack, knockBack / 2);
        }

        public virtual void TakeDamage(float damage)
        {
            SoundManager.instance.PlayClip(m_damage, false, 0.5f);

            if (health > 0)
            {
                health -= damage;
            }

            if (m_useDamageAnimation)
            {
                if (!m_inDamageAnimation)
                {
                    StartCoroutine(FlashDamage());
                }
            }
        }

        public virtual void EquipWeapon(Weapon _weapon)
        {
            if (m_weapon) { m_weapon.gameObject.SetActive(false); }//turn off prev weapon  
            _weapon.gameObject.SetActive(true);//activate incoming weapon
            knockBack = _weapon.mRecoilForce;
            SoundManager.instance.PlayClip(m_armed);
            m_weapon = _weapon;//set equipped weapon to new reference
        }

        /// <summary>
        /// Set the angle of an animator
        /// </summary>
        /// <param name="_animator">Animator attached to GO</param>
        /// <param name="_animatorVar">Variable name</param>
        /// <param name="_angle">Angle val</param>
        private Vector3 oldScale = Vector3.zero;

        public virtual void SetAngle(float angle)
        {
            currentAngle = angle.toIsoAngle();
            //Set the x scale of the player based on the angle
            xScale = (currentAngle >= 0) ? 1 : -1;
            spriteRenderer.gameObject.transform.localScale = new Vector3(xScale, 1, 1);
            //Set the animation angle
            m_armaAnimator.SetFloat("angle", Mathf.Abs(currentAngle));
        }
        public float GetAngle()
        {
            return currentAngle;
        }

        protected virtual void ChangeAnimatorAngle(Animator _animator, string _animatorVar, float _angle)
        {
            if (!(_angle > 0f && _angle < 180f))
            {
                spriteRenderer.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);

                if (GetComponentInChildren<Canvas>() != null)
                {
                    if (oldScale == Vector3.zero)
                    {
                        oldScale = GetComponentInChildren<Canvas>().transform.localScale;
                    }
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
                }
            }
            else
            {
                spriteRenderer.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

                if (GetComponentInChildren<Canvas>() != null)
                {
                    if (oldScale == Vector3.zero)
                    {
                        oldScale = GetComponentInChildren<Canvas>().transform.localScale;
                    }
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(oldScale.x, oldScale.y, oldScale.z);
                }
            }
            //Set the animation angle
            float tempAngle = Mathf.Abs(_angle);
            _animator.SetFloat(_animatorVar, tempAngle);
        }

        
        /// <summary>
        /// Flashes red for given time
        /// </summary>
        /// <returns></returns>
        private IEnumerator FlashDamage()
        {
            spriteRenderer.material.SetFloat("_Damaged", 1.0f);
            m_inDamageAnimation = true;
            yield return new WaitForSeconds(m_damageFlashDuration);
            spriteRenderer.material.SetFloat("_Damaged", 0.0f);
            m_inDamageAnimation = false;
        }
        //public function to set burning
        public void Burnt()
        {
            if (!smokin.isPlaying)
            {
                smokin.Play();
            }
            burnTime = 1;
        }
        //counts down burntime, turns effect off
        void BurnCount()
        {
            if (smokin.isPlaying)
            {
                burnTime -= Time.deltaTime;

                if (burnTime <= 0)
                {
                    smokin.Stop();
                }

            }
        }
        #endregion

    }
}