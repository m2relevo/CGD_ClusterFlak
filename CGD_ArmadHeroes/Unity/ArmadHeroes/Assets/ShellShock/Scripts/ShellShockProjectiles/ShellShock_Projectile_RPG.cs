using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class ShellShock_Projectile_RPG : ShellShock_Projectile_Bullet
    {
        void Awake()
        {
            mRigidBody2D = GetComponent<Rigidbody2D>();
            mMaximumBounces = 1;
        }
        //// Use this for initialization
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}
