using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol
{
    public class CarryOverScript : MonoBehaviour
    {
        static public List<int> s_scores = new List<int>();
        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
