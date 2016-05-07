using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class ShellShock_Projectile_Bullet : ArmadHeroes.Projectile
    {

        private int mCurrentBounces;
        public int mMaximumBounces;
       // private bool isActive = false;
       public Rigidbody2D mRigidBody2D;
        private bool bulletDestroyed = false;
        public AudioSource richochetOffObject;

       // new void Start()
       // {
          
       // }

        void Awake()
        {
            mRigidBody2D = GetComponent<Rigidbody2D>();
            mMaximumBounces = 3;
            richochetOffObject = GameObject.FindGameObjectWithTag("ShellShock/RICOCHETOBJECT").GetComponent<AudioSource>();
        }
       

        public override void Fire(int _playerId, float height, Collider2D _ignoreCollider)
        {

        
             base.Fire(_playerId, height, _ignoreCollider);

            //Sets initial position to the exit position of the weapon
            //transform.position = position;
            //Converts angle to Vector2

            //apply force to the bullet so it propels outward.
            //must use AddForce, to make the bullet fire outwards. Setting the velocity doesn't work.
            mRigidBody2D.AddForce(new Vector2(direction.x, direction.y)* moveSpeed,ForceMode2D.Impulse);


           UpdateRotation();
        }
        protected override void Update()
        {
            //if (mCurrentBounces == mMaximumBounces)
            //{
            //    // ContactPoint2D contact = bulletCollider.contacts[0];
            //    //thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

            //    //Destroy(thisExplosion, 0.2f);
               
            //    Destroy(gameObject);
            //}
              UpdateRotation();

        }

        void UpdateRotation()
        {
            Vector3 dir = GetComponent<Rigidbody2D>().velocity.normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            // Debug.Log("Collided");
             if (collision.gameObject.tag != "Bullet")
             {
                 mCurrentBounces++;
             }
           
                if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<RewiredController>().isBallin == false)
                {
                    // thisExplosion = Instantiate(explosion, gameObject.transform.position/*contact.point*/ /*+ (contact.normal * 5.0f)*/, Quaternion.identity);

                    // Destroy(thisExplosion, 0.2f);
                    HitPlayer(collision.gameObject);
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                }
                if (mCurrentBounces == mMaximumBounces)
                {
                    bulletDestroyed = true;
                    ExplodeOffObject(collision.gameObject);
                    //Destroy(gameObject);
                    gameObject.SetActive(false);

                }
           // }
            if (!bulletDestroyed)
            {
                if (collision.gameObject.tag == "ShellShock/FENCE")
                {
                    richochetOffObject.Play();
                    BounceOffFence(collision.gameObject);
                }
                if (collision.gameObject.tag == "Dynamic")
                {
                    richochetOffObject.Play();
                    BounceOffCover(collision.gameObject);
                }
            }
            
        }

        //void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponentInParent<RewiredController>().playerNumber == callerID)
        //    {
        //        Debug.Log("EXITED");
        //        isActive = true;
        //    }
        //}
        void HitPlayer(GameObject collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            Vector3 explosionPos = transform.position + (direction * 0.25f);
            GameType_ShellShock.instance.Explosion(explosionPos);
        }
        void ExplodeOffObject(GameObject collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            Vector3 explosionPos = transform.position + (direction * 0.25f);
            GameType_ShellShock.instance.Explosion(explosionPos, false);
        }
        void BounceOffFence(GameObject collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            Vector3 bouncePos = transform.position + (direction * 0.25f);
            GameType_ShellShock.instance.Sparks(bouncePos, false);
        }
        void BounceOffCover(GameObject collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            Vector3 bouncePos = transform.position + (direction * 0.25f);
            GameType_ShellShock.instance.Dust(bouncePos, false);
        }
    }
}
