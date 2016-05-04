
using UnityEngine;
using System.Collections;

namespace ArmadHeroes
{
    [ExecuteInEditMode]
    public class EnvironmentSprite : MonoBehaviour
    {
        public bool EnableEdittingMode = false;
        PolygonCollider2D hitBox, spriteBox;
        public float Base, extendXY, height;
        public Vector2 colliderOffset;
        public Vector2 origin=Vector2.one;
        public enum ColliderType { occlusionTrigger, bulletCollision, playerCollison }

        private Color originalColour = Color.white;

        public ColliderType m_colliderType;

        float newOpacity = 1;
        SpriteRenderer sr;

        void Start()
        {
            if (GetComponent<SpriteRenderer>())
            {
                sr = GetComponent<SpriteRenderer>();
                originalColour = sr.color;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (EnableEdittingMode)
            {
                #region collider building
                if (sr == null && GetComponent<SpriteRenderer>())
                {
                    sr = GetComponent<SpriteRenderer>();
                }

                if (origin == Vector2.one)
                {
                    if (!GetComponent<SpriteRenderer>())
                    {
                        origin.y = -GetComponentInParent<SpriteRenderer>().sprite.bounds.extents.y;
                        origin.x = 0;// -GetComponentInParent<SpriteRenderer>().sprite.bounds.extents.x/2;
                    }
                    else
                    {
                        origin.y = -GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
                        origin.x = 0;//-GetComponent<SpriteRenderer>().sprite.bounds.extents.x/2;
                    }
                }

                if (m_colliderType != ColliderType.occlusionTrigger && !GetComponent<Rigidbody2D>())
                    gameObject.AddComponent<Rigidbody2D>().isKinematic = true;

                //Might not be necessary
                /*
                if (GetComponent<Rigidbody2D>() && m_colliderType == ColliderType.occlusionTrigger)
                    DestroyImmediate(GetComponent<Rigidbody2D>());
                */

                if (!GetComponent<PolygonCollider2D>())
                    gameObject.AddComponent<PolygonCollider2D>();
                else if (hitBox == null)
                    hitBox = GetComponent<PolygonCollider2D>();
                else
                {
                    if (height <= 0)
                    {
                        Vector2[] hitBoxPoints =
                        {
                        new Vector2(origin.x,origin.y),
                        new Vector2(origin.x+Base,origin.y+(Base/2)),
                        new Vector2(origin.x,origin.y+Base),
                        new Vector2(origin.x-Base,origin.y+(Base/2)),
                    };

                        //Depth
                        if (extendXY > 0)
                        {
                            hitBoxPoints[1] += new Vector2(extendXY, extendXY / 2);
                            hitBoxPoints[2] += new Vector2(extendXY, extendXY / 2);
                        }
                        else if (extendXY < 0)
                        {
                            hitBoxPoints[3] += new Vector2(extendXY, -extendXY / 2);
                            hitBoxPoints[2] += new Vector2(extendXY, -extendXY / 2);
                        }
                        hitBox.points = hitBoxPoints;
                    }
                    else
                    {
                        Vector2[] hitBoxPoints =
                            {
                            new Vector2(origin.x,origin.y),
                            new Vector2(origin.x+Base,origin.y+(Base/2)),
                            new Vector2(origin.x+Base,origin.y+(Base/2)+height),
                            new Vector2(origin.x,origin.y+Base+height),
                            new Vector2(origin.x-Base,origin.y+(Base/2)+height),
                            new Vector2(origin.x-Base,origin.y+(Base/2)),
                        };

                        //Depth
                        if (extendXY > 0)
                        {
                            hitBoxPoints[1] += new Vector2(extendXY, extendXY / 2);
                            hitBoxPoints[2] += new Vector2(extendXY, extendXY / 2);
                            hitBoxPoints[3] += new Vector2(extendXY, extendXY / 2);
                        }
                        else if (extendXY < 0)
                        {
                            hitBoxPoints[3] += new Vector2(extendXY, -extendXY / 2);
                            hitBoxPoints[4] += new Vector2(extendXY, -extendXY / 2);
                            hitBoxPoints[5] += new Vector2(extendXY, -extendXY / 2);
                        }
                        hitBox.points = hitBoxPoints;
                    }

                    hitBox.isTrigger = (m_colliderType != ColliderType.playerCollison) ? true : false;
                    hitBox.offset = colliderOffset;
                }
                #endregion
            }

            if (sr != null)
            {
                if (sr.color.a != newOpacity)
                {
                    originalColour.a = Mathf.Lerp(sr.color.a, newOpacity, Time.deltaTime * 10f);

                    sr.color = originalColour;
                }
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (m_colliderType == ColliderType.occlusionTrigger)
            {
                //if (GetComponent<SpriteRenderer>())
                //{
                    if ((col.tag == "Player" || col.tag == "Armatillery/Enemy") && sr.color.a != .5f)
                    {
                        newOpacity = .5f;
                    }
                //}
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (m_colliderType != ColliderType.occlusionTrigger)
            {
                if (col.tag == "Bullet")
                {
                    col.gameObject.SetActive(false);
                }
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (m_colliderType == ColliderType.occlusionTrigger)
            {
                if (col.tag == "Player" || col.tag == "Armatillery/Enemy")
                {
                    newOpacity = 1;
                }
            }
        }
    }
}