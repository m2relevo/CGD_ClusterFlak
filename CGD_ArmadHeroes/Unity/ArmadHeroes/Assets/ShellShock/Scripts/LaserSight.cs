using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class LaserSight : MonoBehaviour
    {
        LineRenderer mLaserLine;
        Vector2 P1;//, P2;

        Vector2 mAimVector;

        void Start()
        {
            mLaserLine = GetComponent<LineRenderer>();
            mLaserLine.SetVertexCount(2);
        }

        void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(P1, mAimVector);
            if (hit.collider != null && hit.collider.tag != "Bullet")
            {
                mLaserLine.SetPosition(1, hit.point);

                // Debug.Log(hit.collider.gameObject.name);
                //P2 = hit.point;
            }

            //Debug.DrawLine(P1, P2, Color.red);
        }

        public void UpdateOrientation(float angle)
        {
            mAimVector = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        public void UpdateOrientation(Vector2 aimVector)
        {
            mAimVector = aimVector;
        }

        public void UpdatePosition(Vector2 pos)
        {
            P1 = pos;
            mLaserLine.SetPosition(0, pos);
        }
    }
}
