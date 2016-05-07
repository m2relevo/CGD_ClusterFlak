/// <summary>
/// LaserManager.cs
/// Created and implemented by Daniel Weston on 23/03/2016
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Armatillery
{
    public class LaserManager : MonoBehaviour
    {
        #region Singleton
        private static LaserManager m_instance;
        public static LaserManager instance { get { return m_instance; } }
        #endregion

        #region Public Members
        public GameObject m_laser,
            m_charge;
        #endregion

        #region Private Members
        private List<GameObject> m_laserPool = new List<GameObject>();
        private List<GameObject> m_laserChargePool = new List<GameObject>();
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            m_instance = this;
            PoolLaserManagerComponents();
            Init();
        }
        #endregion

        #region LaserManager Behaviours
        /// <summary>
        /// Necessary func
        /// calls to reset/start
        /// manager
        /// </summary>
        public void Init()
        {
            ResetLaserManagerComponents();
        }

        /// <summary>
        /// Returns available laser beam 
        /// </summary>
        /// <returns></returns>
        public GameObject RequestLaser()
        {
            for (int i = 0; i < m_laserPool.Count; i++)
            {
                if (!m_laserPool[i].activeSelf)
                {
                    return m_laserPool[i];
                }
            }

            GameObject _laser = Instantiate(m_laser);
            _laser.SetActive(false);
            _laser.transform.position = this.transform.position;
            m_laserPool.Add(_laser);
            return _laser;
        }

        /// <summary>
        /// Returns available charge particle
        /// </summary>
        /// <returns></returns>
        public GameObject RequestCharge()
        {
            for (int i = 0; i < m_laserChargePool.Count; i++)
            {
                if (!m_laserChargePool[i].activeSelf)
                {
                    return m_laserChargePool[i];
                }
            }

            GameObject _charge = Instantiate(m_charge);
            _charge.SetActive(false);
            _charge.transform.position = this.transform.position;
            m_laserChargePool.Add(_charge);
            return _charge;
        }

        /// <summary>
        /// Iterates over the pool amount 
        /// and creates x amount of lasers
        /// </summary>
        private void PoolLaserManagerComponents()
        {
            for (int i = 0; i < 10; i++)
            {
                #region pool laser beams
                GameObject _laser = Instantiate(m_laser);
                _laser.SetActive(false);
                _laser.transform.parent = this.transform;
                m_laserPool.Add(_laser);
                #endregion

                #region pool laser charge particles
                GameObject _charge = Instantiate(m_charge);
                _charge.SetActive(false);
                _charge.transform.parent = this.transform;
                m_laserChargePool.Add(_charge);
                #endregion
            }
        }

        /// <summary>
        /// Iterates over all lasers 
        /// and turns them off
        /// </summary>
        private void ResetLaserManagerComponents()
        {
            for (int i = 0; i < m_laserPool.Count; i++)
            {
                m_laserPool[i].SetActive(false);
            }
            for (int i = 0; i < m_laserChargePool.Count; i++)
            {
                m_laserChargePool[i].SetActive(false);
            }
        }
        #endregion
    }
}