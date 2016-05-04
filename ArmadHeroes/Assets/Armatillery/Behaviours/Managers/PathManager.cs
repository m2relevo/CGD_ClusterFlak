using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Armatillery
{
    [System.Serializable]
    public class Path
    {
        public List<GameObject> myList;
    }
}
namespace Armatillery
{
    public class PathManager : MonoBehaviour
    {
        public static PathManager instance;

        public List<Path> m_paths = new List<Path>();

        void Awake()
        {
            instance = this;
        }

        void Update()
        {

        }

        public GameObject NextNodeOnPath(int pathIndex, int currentPositionInPath)
        {
            if (currentPositionInPath + 1 < m_paths[pathIndex].myList.Count)
            {
                return m_paths[pathIndex].myList[currentPositionInPath + 1];
            }
            return m_paths[pathIndex].myList[m_paths[pathIndex].myList.Count - 1];
        }

        public GameObject FirstNodeOnPath(int pathIndex)
        {
            return m_paths[pathIndex].myList[0];
        }

        public int NodeCountInPath(int pathIndex)
        {
            return m_paths[pathIndex].myList.Count;
        }

        private int lastPathIndex = 0;

        public int RandomPath()
        {
            return Random.value > 0.5? Random.Range(0, lastPathIndex) : Random.Range(lastPathIndex+1, m_paths.Count);
        }
    }
}
