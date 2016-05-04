/// <summary>
/// ProjectileDynamic (formally Assualt ) is derived from the ProjectileBase class
/// ProjectileDynamic is used for dynamic projectile types
/// Created and implemented by Daniel Weston - 10/01/16
/// </summary>
using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public class ProjectileDynamic : ProjectileBase
    {
        protected override void Update()
        {
            switch (GameManager.instance.state)
            {
                case GameStates.game:
                    //update base class first for count down ect
                    base.Update();
                    //update bullet movement
                    transform.Translate((direction.normalized * moveSpeed) * Time.deltaTime, Space.World);
                    break;
                case GameStates.pause:
                    break;
                case GameStates.gameover:
                    break;
                default:
                    break;
            }           
        }
    }
}