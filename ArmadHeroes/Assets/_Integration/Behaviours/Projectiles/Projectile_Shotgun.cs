using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    public class Projectile_Shotgun : Projectile
    {
        // Use this for initialization
        void Awake()
        {
            moveSpeed = 5.0f;
            destructTime = 0.3f;
        }

		public override void Fire (int _playerId, float height = 0, Collider2D _ignoreCollider = null)
		{
			base.Fire (_playerId, height, _ignoreCollider);
		}
    }
}