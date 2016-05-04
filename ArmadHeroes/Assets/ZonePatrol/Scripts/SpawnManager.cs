using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZonePatrol
{
    public class SpawnManager : MonoBehaviour
    {
        [System.Serializable]
        public class ListWrapper
        {
            public List<GameObject> spawns;
        }

        public List<ListWrapper> spawnsHolder = new List<ListWrapper>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
