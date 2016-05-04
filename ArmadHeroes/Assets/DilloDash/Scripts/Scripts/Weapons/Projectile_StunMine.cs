using UnityEngine;
using System.Collections;

namespace DilloDash
{
    public class Projectile_StunMine : ArmadHeroes.Projectile_Mine
    {
        protected override void OnTriggerStay2D(Collider2D _other)
        {
            if (detonate)
            {
                if (_other.tag == "Player")
                {
                    _other.GetComponent<DilloDashPlayer>().InitiateStun(transform.position,true);
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
