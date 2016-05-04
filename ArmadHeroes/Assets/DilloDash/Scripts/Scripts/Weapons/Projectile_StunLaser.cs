using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class Projectile_StunLaser : ArmadHeroes.Projectile_Laser
    {
        protected void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.tag == "Player")
            {
                if (_other.GetComponent<ArmadHeroes.PlayerActor>().playerNumber != callerID)
                {
                    _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position, true);
                }
            }
        }
        protected override void Update()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                base.Update();
            }
        }
    }
}


