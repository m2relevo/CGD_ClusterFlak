using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class ShellShock_Bullet_MachineGun : ShellShock_Projectile_Bullet
    {

        //private int mCurrentBounces;
        //private int mMaximumBounces = 3;
        //private bool isActive = false;
        //Rigidbody2D mRigidBody2D;



        //void Awake()
        //{
        //    // Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        //    mRigidBody2D = GetComponent<Rigidbody2D>();
        //}

        //public override void Fire(int _playerId, float height, Collider2D _ignoreCollider)
        //{

        
        //     base.Fire(_playerId, height, _ignoreCollider);

        //    //Sets initial position to the exit position of the weapon
        //    //transform.position = position;
        //    //Converts angle to Vector2
        //    // mRigidBody2D.velocity = new Vector2(Mathf.Sin(direction.x * Mathf.Deg2Rad), Mathf.Cos(direction.y * Mathf.Deg2Rad)) * moveSpeed;


        //    //apply force to the bullet so it propels outward.
        //    //must use AddForce, to make the bullet fire outwards. Setting the velocity doesn't work.
        //    mRigidBody2D.AddForce(new Vector2(direction.x, direction.y)* moveSpeed,ForceMode2D.Impulse);

        //  //  Debug.Log(new Vector2(Mathf.Sin(direction.x * Mathf.Deg2Rad), Mathf.Cos(direction.y * Mathf.Deg2Rad)) * moveSpeed);
        //    //Debug.Log("BULLET VELOCITY: " +mRigidBody2D.velocity);
        //  //  Debug.Log("MOVESPEED" + moveSpeed);
        //    //Debug.Log("X:" + direction.x + "Y: " + direction.y);
        //   UpdateRotation();
        //}
        //protected override void Update()
        //{
        //   // mRigidBody2D.velocity = new Vector2(Mathf.Sin(direction.x * Mathf.Deg2Rad), Mathf.Cos(direction.y * Mathf.Deg2Rad)) * moveSpeed;
        //    //mRigidBody2D.velocity = Vector2.up * 10;

        //    //base.Update();

        //    if (mCurrentBounces == mMaximumBounces)
        //    {
        //        // ContactPoint2D contact = bulletCollider.contacts[0];
        //        //thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

        //        //Destroy(thisExplosion, 0.2f);
        //        Destroy(gameObject);
        //    }
        //      UpdateRotation();

        //}

        //void UpdateRotation()
        //{
        //    Vector3 dir = GetComponent<Rigidbody2D>().velocity.normalized;
        //    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        //}

        ////void OnTriggerEnter2D(Collider2D collider)
        ////{
        ////    if (isActive)
        ////    {
        ////        Debug.Log("CollideD");
        ////        mCurrentBounces++;

        ////        if (collider.tag == "Player" && collider.gameObject.GetComponent<RewiredController>().isBallin == false)
        ////        {
        ////            // thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

        ////            // Destroy(thisExplosion, 0.2f);
        ////            Destroy(gameObject);
        ////        }
        ////    }
        ////}

        //void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (isActive)
        //    {
        //        Debug.Log("Collided");
        //        mCurrentBounces++;

        //        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<RewiredController>().isBallin == false)
        //        {
        //            // thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

        //            // Destroy(thisExplosion, 0.2f);
        //            Destroy(gameObject);
        //        }
        //    }
        //}

        //void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponentInParent<RewiredController>().playerNumber == callerID)
        //    {
        //        Debug.Log("EXITED");
        //        isActive = true;
        //    }

        //}


        //void OnTriggerExit2D(Collider2D collider)
        //{
        //    if(collider.tag == "Player" && collider.GetComponentInParent<RewiredController>().playerNumber == callerID)
        //    {
        //        Debug.Log("EXTED");
        //        isActive = true;
        //    }
        //}

        
    }
}
