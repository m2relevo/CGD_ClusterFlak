using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class PlayerActor : Actor
    {
        #region Public Members
        public float accolade_unique = 0;
        public float accolade_timesShot = 0;
        public float accolade_shotsFired = 0;
        public float accolade_distance = 0;
        public int chevron_score = 0;
        public int ControllerID { get { return m_controllerID; } }

        //Controller
        public ArmadHeroes.Controller controller
        {
            private set { }
            get { return ArmadHeroes.ControllerManager.instance.GetController(m_controllerID); }
        }
        #endregion

        protected int m_controllerID = 0;
        protected bool walking = false;
        protected float footstepCooldown;
        public ParticleSystem leftFootprint, rightFootprint;
        public AudioClip footstepSfx;

		protected virtual void Update()
        {
            if (controller.pauseButton.JustPressed())
            {
                if (ArmadHeroes_Pause.instance != null)
                {
                    if (m_weapon != null) { m_weapon.StopFire(); }
                    ArmadHeroes_Pause.instance.Pause(m_controllerID);
                }
            }

            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    Tick();
                    break;
                case GameStates.pause:
                    break;
                case GameStates.gameover:
                    break;
                default:
                    break;
            }
		}

        protected virtual void Pause()
        {
            if (controller.pauseButton.JustPressed())
            {
                if (ArmadHeroes_Pause.instance != null)
                {
                    if (m_weapon != null) { m_weapon.StopFire(); }
                    ArmadHeroes_Pause.instance.Pause(m_controllerID);
                }
            }
        }

        protected virtual void Tick()
        {
            if(m_weapon)
            {
                m_weapon.Tick();
            }
        }

        protected virtual void Steps()
        {
            if (walking && footstepCooldown <= 0)
            {
                if (footstepSfx)
                {
                    SoundManager.instance.PlayClip(footstepSfx, transform.position);
                }
                float angle = Mathf.Atan2(shootDir.x, -shootDir.y) * Mathf.Rad2Deg;

                leftFootprint.startRotation = Mathf.Deg2Rad * -(angle - 90);
                rightFootprint.startRotation = leftFootprint.startRotation;
                leftFootprint.Emit(1);
                rightFootprint.Emit(1);

                footstepCooldown = 0.15f;
            }
            else if (footstepCooldown > 0)
            {
                footstepCooldown -= Time.deltaTime;
            }
        }
    }
}
