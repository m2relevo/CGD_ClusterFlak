using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class DropPoint : MonoBehaviour
    {
        public bool isDebug;

        void Start()
        {
            Helicopter.Instance.JoinDropPoints(GetComponent<Transform>());
        }

        void Update()
        {
            if (isDebug)
            {

            }
        }
    }
}