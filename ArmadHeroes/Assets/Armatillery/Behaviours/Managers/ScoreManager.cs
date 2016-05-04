using UnityEngine;
using System.Collections;

namespace Armatillery {
    public class ScoreManager : MonoBehaviour {

        #region Singleton
        private static ScoreManager m_instance;
        public static ScoreManager instance {get {return m_instance;} protected set{m_instance = value;}}
        #endregion

        // Use this for initialization
        void Awake()
        {
            instance = this;
        }

        public void AddScore(int playerID, int score)
        {
            //playerScores[playerID] += score;
            ArmaPlayerManager.instance.m_spawnedPlayers[playerID].chevron_score += score;
        }

        public int GetScore(int playerID)
        {
            return ArmaPlayerManager.instance.m_spawnedPlayers[playerID].chevron_score;
        }
    }
}
