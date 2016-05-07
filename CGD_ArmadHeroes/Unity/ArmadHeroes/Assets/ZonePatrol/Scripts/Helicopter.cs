using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZonePatrol
{
    public class Helicopter : MonoBehaviour
    {
        private static Helicopter instance = null;
        public static Helicopter getInstance() { return instance; }

        public Vector3 randomPosition;
        public List<GameObject> dropItems;

        // Use this for initialization
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
