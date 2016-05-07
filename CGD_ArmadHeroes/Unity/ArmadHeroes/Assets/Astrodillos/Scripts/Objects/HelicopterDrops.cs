using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace ArmadHeroes
{
    /// <summary>
    /// get random start
    /// get random Drop zone
    /// drop package
    /// when package.y = shadow.y spawn pickup
    /// get exit target
    /// </summary>
    public class HelicopterDrops : MonoBehaviour
    {
       
        float randAxis;
        float coolDown = 5;
        float currentCoolDown;
        HelicopterDrops instance = null;
        SpriteRenderer spRend;
       // Animator ani;
        public GameObject shadow;
        public GameObject crate;
        Rigidbody2D body;
        Vector3 target;
        public GameObject[] targets;
        Tweener moveToDrop;
        Tweener exitDZ;
        bool gotTarget = false;
        bool flip = true;
       // bool dropOne = false;
        Vector3 DZ;
   
        // Use this for initialization
        void Start()
        {
            //there shall be only one!
            if (instance == null)
            {
                instance = this;
            }
            spRend = GetComponent<SpriteRenderer>();
          //  ani = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            RandomStart();
            ActivateSprites(false);
         }

        // Update is called once per frame
        void Update()
        {
            CooldownTime();
            MoveToDZ();
               
        }
      
        void RandomStart()
        {
            Vector3 randomPos;
            randAxis = Random.Range(-0.2f, 1.2f);

            flip = !flip;
            if (flip)
            {
                randomPos = new Vector3(randAxis, 1, 0);

            }
            else
            {
                randomPos = new Vector3(0, randAxis, 0);
            }

            Vector3 pos = Camera.main.ViewportToWorldPoint(randomPos);
            pos.z = 0;
            transform.position = pos;
            DZ = DropZone();
            gotTarget = true;
        }
        void CooldownTime()
        {
            if (currentCoolDown<coolDown)
            {
                currentCoolDown += Time.deltaTime;
                if (!gotTarget)
                {
                    RandomStart();
                }
            }
            else
            {
                ActivateSprites(true);
            }
        }
        void MoveToDZ()
        {
            moveToDrop = body.DOMove(DZ, 3);
            moveToDrop.OnStart(() => 
            {
                if (flip)
                {
                    target = new Vector3(-transform.position.x, -8, 0);
                }
                else
                {
                    target = new Vector3(13, -transform.position.y, 0);
                }
            });
            moveToDrop.OnComplete(()=>
            {
              //  DZ = target;
            });
        }
        void MoveToExit()
        {
            exitDZ = body.DOMove(target, 3);
            exitDZ.OnComplete(() => 
            {
                ActivateSprites(false);
                gotTarget = false;
                ResetCooldown();
            });
        }
        void ResetCooldown()
        {
            currentCoolDown = 0;
        }
        Vector3 DropZone()
        {
            return targets[Random.Range(0, 3)].transform.position;
        }
        void ActivateSprites(bool onOff)
        {
            spRend.enabled = onOff;
            shadow.GetComponent<SpriteRenderer>().enabled = onOff;
        }
    }
}
