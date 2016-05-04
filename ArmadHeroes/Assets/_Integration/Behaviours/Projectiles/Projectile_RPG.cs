using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArmadHeroes
{
    public class Projectile_RPG : Projectile
    {
       
      protected override void Start()
      {
          base.Start();
          moveSpeed = 10;
         
      }
      
    }
}