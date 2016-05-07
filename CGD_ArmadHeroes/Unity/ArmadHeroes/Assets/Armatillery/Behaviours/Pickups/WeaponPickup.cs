/// <summary>
/// Enumerator describing weapon types
/// Created and implemented by Daniel Weston - 10/01/16
/// </summary>

using UnityEngine;
using System;

namespace Armatillery
{
    public enum Weapons
    {
        MachineGun, 
        ShotGun,
        FlameThrower, 
        LaserGun
    };

    [RequireComponent (typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class WeaponPickup : MonoBehaviour
    {
        public Weapons m_type;
    }
}