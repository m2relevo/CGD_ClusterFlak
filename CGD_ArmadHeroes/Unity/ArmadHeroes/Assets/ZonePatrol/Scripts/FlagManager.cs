using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace ZonePatrol {

    public class FlagManager : MonoBehaviour
    {
        [System.Serializable]
        public class ListWrapper
        {
            public List<GameObject> flags;
        }

        public List<ListWrapper> flagsHolder = new List<ListWrapper>();

        // Use this for initialization
        void Start() { 
            //spawn flags
            int flagHolder = 0;
            int numOfPlayers = PlayerManager.getInstance().getNumberOfPlayers();

            if (numOfPlayers <= 2)
            {
                flagHolder = 0;
            }
            else if(numOfPlayers <= 3)
            {
                flagHolder = 1;
            }
            else
            {
                flagHolder = 2;
            }

            for (int i = 0;i < flagsHolder[flagHolder].flags.Count;i++)
            {
                flagsHolder[flagHolder].flags[i].SetActive(true); // enable all flags in flag list
            }

        }
    }
}
