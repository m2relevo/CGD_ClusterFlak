using UnityEngine;
using System.Collections;
namespace ArmadHeroes
{
    public class Projectile_Laser : Projectile
    {
        protected override void Start()
        {
            base.Start();
            moveSpeed = 10;
        }
      
    }
}