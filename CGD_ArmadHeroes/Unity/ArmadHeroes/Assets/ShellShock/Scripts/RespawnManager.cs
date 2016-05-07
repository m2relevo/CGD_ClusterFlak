using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace ShellShock
{
    public class RespawnManager : MonoBehaviour
    {
        public static RespawnManager Instance;

        public Dictionary<GameObject, float> mObjectWaitTimes;

        //public List<Vector2> mSpawnPoints;
        public GameObject[] spawnPoints;

       
        private float lateStartTimer = 0f;
        private float lateStartThreshold = 0.1f;
        private bool callLateStart = false;

        void Start()
        {
            Instance = this;
            mObjectWaitTimes = new Dictionary<GameObject, float>();
           
            
        }
        void LateStart()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("ShellShock/RESPAWNPOINT");
        }
        void ProcessLateStart()
        {
            if (!callLateStart)
            {
                lateStartTimer += Time.deltaTime;
                if (lateStartTimer >= lateStartThreshold)
                {
                    lateStartTimer = 0;
                    callLateStart = true;
                    LateStart();
                }
            }
        }

        public void SetNewSpawnPoints()
        {
            for(int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i] = null;
            }
            spawnPoints = GameObject.FindGameObjectsWithTag("ShellShock/RESPAWNPOINT");
        }

        public void Kill(GameObject go, float timeToWait)
        {
            mObjectWaitTimes.Add(go, timeToWait);
            go.SetActive(false);
        }

        void Update()
        {
            ProcessLateStart();
            foreach (GameObject deadGo in mObjectWaitTimes.Keys.ToList())
            {
                if (mObjectWaitTimes[deadGo] > 0)
                {
                    mObjectWaitTimes[deadGo] -= Time.deltaTime;
                }
                else
                {
                    deadGo.SetActive(true);
                    deadGo.GetComponent<PlayerLogic>().Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position);
                    mObjectWaitTimes.Remove(deadGo);
                }
            }
        }
    }
}
