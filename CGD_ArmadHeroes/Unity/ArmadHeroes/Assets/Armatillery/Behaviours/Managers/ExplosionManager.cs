/// <summary>
/// Fed up with th amount of objects spawning explosions
/// Created and implemented by Daniel Weston - 05/04/16
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Armatillery
{
    public class ExplosionManager : MonoBehaviour
    {
        #region Singleton
        private static ExplosionManager m_instance;
        public static ExplosionManager instance { get { return m_instance; } }
        #endregion

        #region public memebers
        public GameObject ExplosionPrefab;
        public GameObject BigExplosionPrefab;
        public GameObject SmallExplosionPrefab;
        public int m_spawnAmount = 10;
        public int m_BigspawnAmount = 25;
        public int m_SmallspawnAmount = 25;
        #endregion

        #region private Memebers
        private List<GameObject> m_ExplosionPool = new List<GameObject>();
        private List<GameObject> m_BigExplosionPool = new List<GameObject>();
        private List<GameObject> m_SmallExplosionPool = new List<GameObject>();
        private int m_explosionPoolHead = 0;
        private int m_BigexplosionPoolHead = 0;
        private int m_SmallexplosionPoolHead = 0;
        #endregion

        #region Unity Behaviours
        void Awake()
        {
            m_instance = this;
            PoolExplosions();
        }
        #endregion

        #region ExplosionManager Behaviours
        private void PoolExplosions()
        {
            for (int i = 0; i < m_spawnAmount; i++)
            {
                GameObject _temp = Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
                _temp.SetActive(false);
                _temp.transform.parent = this.gameObject.transform;
                m_ExplosionPool.Add(_temp);
            }
            for (int i = 0; i < m_BigspawnAmount; i++)
            {
                GameObject _temp2 = Instantiate(BigExplosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
                _temp2.SetActive(false);
                _temp2.transform.parent = this.gameObject.transform;
                m_BigExplosionPool.Add(_temp2);
            }

            for (int i = 0; i < m_SmallspawnAmount; i++)
            {
                GameObject _temp3 = Instantiate(SmallExplosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
                _temp3.SetActive(false);
                _temp3.transform.parent = this.gameObject.transform;
                m_SmallExplosionPool.Add(_temp3);
            }
        }
        public GameObject GetExplosion()
        {
            //check head
            m_explosionPoolHead = m_explosionPoolHead == m_ExplosionPool.Count ? 0 : m_explosionPoolHead;
            //return explosion 
            return m_ExplosionPool[m_explosionPoolHead++];
        }

        public GameObject GetBigExplosion()
        {
            //check head
            m_BigexplosionPoolHead = m_BigexplosionPoolHead == m_BigExplosionPool.Count ? 0 : m_BigexplosionPoolHead;
            //return explosion 
            return m_BigExplosionPool[m_BigexplosionPoolHead++];
        }

        public GameObject GetSmallExplosion()
        {
            //check head
            m_SmallexplosionPoolHead = m_SmallexplosionPoolHead == m_SmallExplosionPool.Count ? 0 : m_SmallexplosionPoolHead;
            //return explosion 
            return m_SmallExplosionPool[m_SmallexplosionPoolHead++];
        }
        #endregion
    }
}