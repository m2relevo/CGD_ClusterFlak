using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ArmadHeroes
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] GameObject[] singlePlayerUI;
        [SerializeField] GameObject[] splitPlayerUI;
        public Text Timer;
        [SerializeField] bool useTimer;

        private static CanvasManager m_instance = null;
        public static CanvasManager instance { get { return m_instance; } }

        ArmadHeroes.PlayerData[] pData;

       

        
        

        void Awake()
        {
            m_instance = this;

            Timer.gameObject.transform.parent.gameObject.SetActive(useTimer);
        }

       

        /// <summary>
        /// Initialise the canvas for the number of players
        /// </summary>
        public void init()
        {
            if (ArmadHeroes.GlobalPlayerManager.instance != null)
            {
                pData = ArmadHeroes.GlobalPlayerManager.instance.playerData;

                for (int i = 0; i < pData.Length; ++i)
                {
                    if (i < 4)
                    {
                        if (pData[i].activePlayer && pData[i + 4].activePlayer)
                            splitPlayerUI[i].SetActive(true);
                        else if (pData[i].activePlayer)
                            singlePlayerUI[i].SetActive(true); // will change to greyscale later
                    }
                    setPlayerSprites(i);
                }
            }
        }

        /// <summary>
        /// Set the correct player sprites on the UI
        /// </summary>
        /// <param name="playerNo"></param>
        public void setPlayerSprites(int playerNo)
        {
            if (playerNo < 4)
            {
                getPlayerHUD(playerNo).GetComponentInChildren<Image>().overrideSprite = CharacterProfiles.instance.GetProfile(pData[playerNo].character).characterFileImage_head;
            }
            else
            {
                Image[] heads = getPlayerHUD(playerNo).GetComponentsInChildren<Image>();
                heads[1].overrideSprite = CharacterProfiles.instance.GetProfile(pData[playerNo].character).characterFileImage_head;
            }
        }

        /// <summary>
        /// Use this to display the value you are tracking (value must be tracked in your own scripts)
        /// </summary>
        /// <param name="playerNo"></param>
        /// <param name="wins"></param>
        public void setPlayerValue(int playerNo, int wins)
        {
            if (playerNo < 4)
            {
                getPlayerHUD(playerNo).GetComponentInChildren<Text>().text = wins.ToString();
            }
            else
            {
                Text[] texts = getPlayerHUD(playerNo).GetComponentsInChildren<Text>();
                texts[1].text = wins.ToString();
            }
        }

        // used to identify which HUD element represents the playerNumber
        GameObject getPlayerHUD(int playerNo)
        {
            if (playerNo < 4)
            {
                if (pData[playerNo].activePlayer && pData[playerNo + 4].activePlayer)
                {
                    return splitPlayerUI[playerNo];
                }
                else
                {
                    return singlePlayerUI[playerNo];
                }
            }
            else
            {
                return splitPlayerUI[playerNo - 4];
            }
        }

        /// <summary>
        /// Used to pass in a float variable and display the time in minutes:seconds
        /// </summary>
        /// <param name="inTime"></param>
        /// <param name="countDown"></param>
        public void updateTime(float inTime, bool countDown = true)
        {
            //If counting down display an offset due to rounding down
            float time = inTime;
            if (countDown)
            {
                time += 0.999f;
            }

            //Setup to display the minutes
            int minutes = (int)Mathf.Floor(time / 60.0f);
            string display;
            if (minutes > 9)
            {
                display = minutes.ToString() + ":";
            }
            else
            {
                display = "0" + minutes.ToString() + ":";
            }
            //Setup to display the seconds
            int seconds = Mathf.FloorToInt((time) % 60.0f);
            if (seconds > 9)
            {
                display += seconds.ToString();
            }
            else
            {
                display += "0" + seconds.ToString();
            }
            Timer.text = display;
        }
    }
}
