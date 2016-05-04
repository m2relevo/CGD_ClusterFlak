/// <summary>
/// PowerupType.cs created and implemented by Craig Tinney - 15/01/2016
/// Edited by Daniel Weston - 15/01/16
/// </summary>
using System;
namespace ArmadHeroes
{
    [Flags]
    public enum BulletModifier
    {
        vanilla = 0x00,
        explodeOnDeath = 0x01,
        rapidFire = 0x02,
        superSpeed = 0x04,
        teleport = 0x08,
    };
}