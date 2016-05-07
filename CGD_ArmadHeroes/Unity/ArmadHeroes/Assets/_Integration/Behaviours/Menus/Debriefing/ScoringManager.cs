using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace ArmadHeroes
{
	public enum AccoladeEnum
	{
		BULLETSPONGE = 0,
		CAMPER,
		ROADRUNNER,
		RAMBO,
		HIGHFLYER,
		STUNNER,
		FLAKCANNON,
		KINGOFRING,
		LEECH,
		ZONESTEALER,
		BALLER,
		MAX_ACCOLADES
	}

    [System.Serializable]
	public struct AccoladeInfo
	{
        public AccoladeEnum type;
        public Sprite accoladeSprite;
        public string accoladeName;
        public string description;
        public string afterDescription;
        public bool isFloat; //How to cast value for description
        public bool isHighest; //Takes the highest value as the win
	}

	public struct AccoladeValues
	{
        public float lastValue;
        public List<Vector3> accoladesPositions;
    }

    public class ScoringManager : MonoBehaviour
    {
        static private ScoringManager m_instance = null;
        static public ScoringManager instance { get { return m_instance; } }

        [SerializeField] private AccoladeInfo[] accolades;
        public AccoladeInfo AccoladeInfo(AccoladeEnum _type) { return accolades[(int)_type]; }
        [SerializeField] private Sprite[] chevrons;
        public Sprite ChevronSprite(int _chevron) { return chevrons[_chevron]; }
        [SerializeField] private CharacterScoreFile[] scoreFiles;
        [SerializeField] private AccoladeDisplay[] displays;

        [SerializeField] private GameObject pinPrefab;
        [SerializeField] private GameObject pinParent;

        public int[] awardAccolades = new int[5];

        private bool promotionsStarted = false;
        private bool promotionsFinished = false;
        private bool accoladesStarted = false;
        private bool accoladesFinished = false;


        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }

            for (int i = 0; i < 8; ++i)
            {
                scoreFiles[i].SetupFile(i);
                AssignChevrons(i);
                AssignPreviousAccolades(i);
            }
        }

        void Start()
        {
            
        }

        void Update()
        {
            if(!promotionsStarted)
            {
                DeterminePromotions();
                promotionsStarted = true;
            }
            else if (promotionsFinished)
            {
                if(!accoladesStarted)
                {
                    DetermineAccolades();
                    accoladesStarted = true;
                }
                else if (accoladesFinished)
                {
                    GameManager.instance.state = GameStates.game;
                    if (ControllerManager.instance.GetController(0).boostButton.JustPressed())
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalPlayerManager.instance.currentMission);
                    }
                    else if(ControllerManager.instance.GetController(0).pauseButton.JustPressed())
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelect");
                    }
                    //Allow movement to different menu
                }
                else
                {
                    accoladesFinished = true;
                    for (int i = 0; i < 8; ++i)
                    {
                        accoladesFinished = scoreFiles[i].AnimationsFinished();
                        if (!accoladesFinished)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                promotionsFinished = true;
                for(int i = 0; i < 8; ++i)
                {
                    promotionsFinished = scoreFiles[i].AnimationsFinished();
                    if (!promotionsFinished)
                    {
                        break;
                    }
                }
            }
        }

        void AssignChevrons(int _player)
        {
            GameObject _chevron = SetupPin(ScoreToChevron(GlobalPlayerManager.instance.playerData[_player].overallScore), transform.position, Vector3.one * 0.75f, pinParent);

            if (_chevron.GetComponent<Image>().sprite == null)
            {
                _chevron.SetActive(false);           
            }

            scoreFiles[_player].AssignChevron(_chevron);

        }

        Sprite ScoreToChevron(int _score)
        {
            if(chevrons.Length <= _score)
            {
                return chevrons[chevrons.Length - 1];
            }
            return chevrons[_score];
        }

        void AssignPreviousAccolades(int _player)
        {
            AccoladeValues[] _accolades = GlobalPlayerManager.instance.playerData[_player].accolades;
            for (int i = 0; i < _accolades.Length; ++i )
            {
                for (int j = 0; j < _accolades[i].accoladesPositions.Count; ++j)
                {
                    SetupPin(accolades[i].accoladeSprite, _accolades[i].accoladesPositions[j], Vector3.one * 0.9f, pinParent);
                }
            }
        }

        void DeterminePromotions()
        {
            List<int> _winners = new List<int>();
            int _bestScore = -1;
            for (int i = 0; i < 8; ++i)
            {
                PlayerData _data = GlobalPlayerManager.instance.playerData[i];
                //If better than current winners
                if (_data.activePlayer)
                {
                    if (_data.gameScore > _bestScore)
                    {
                        _winners.Clear();
                        _winners.Add(i);
                        _bestScore = _data.gameScore;
                    }
                    else if (_data.gameScore == _bestScore)
                    {
                        _winners.Add(i);
                    }
                }
                GlobalPlayerManager.instance.playerData[i].gameScore = 0;
            }
            //Give all winners a chevron
            for(int i = 0; i < _winners.Count; ++i)
            {
                ++GlobalPlayerManager.instance.playerData[_winners[i]].overallScore;
                GameObject _chevron = SetupPin(ScoreToChevron(GlobalPlayerManager.instance.playerData[_winners[i]].overallScore), scoreFiles[_winners[i]].transform.position, Vector3.one * 5.0f, pinParent);
                scoreFiles[_winners[i]].Promotion(_chevron);
            }
        }

        void DetermineAccolades()
        {
            DetermineAccoladeWinner(0, AccoladeEnum.BULLETSPONGE);
            DetermineAccoladeWinner(1, AccoladeEnum.CAMPER);
            DetermineAccoladeWinner(2, AccoladeEnum.ROADRUNNER);
            DetermineAccoladeWinner(3, AccoladeEnum.RAMBO);
            DetermineAccoladeWinner(4, GlobalPlayerManager.instance.currentGameAccolade);
        }

        void DetermineAccoladeWinner(int _display,AccoladeEnum _accoladeEnum)
        {
            //Initilise the first player as the winner
            List<int> _winners = new List<int>();
            _winners.Add(0);
            float _winnerValue = GlobalPlayerManager.instance.playerData[0].accolades[(int)_accoladeEnum].lastValue;

            //Loop through the players and determine how many winners there are
            for (int i = 1; i < 8; ++i)
            {
                PlayerData _data = GlobalPlayerManager.instance.playerData[i];
                //If better than current winners
                if (_data.activePlayer)
                {
                    float _value =_data.accolades[(int)_accoladeEnum].lastValue;
                    //Determine if this player has done better than the current
                    if ((_value > _winnerValue && accolades[(int)_accoladeEnum].isHighest) ||
                        (_value < _winnerValue && !accolades[(int)_accoladeEnum].isHighest))
                    {
                        _winners.Clear();
                        _winners.Add(i);
                        _winnerValue = _value;
                    }
                    else if (_data.gameScore == _winnerValue)
                    {
                        _winners.Add(i);
                    }
                }
                GlobalPlayerManager.instance.playerData[i].accolades[(int)_accoladeEnum].lastValue = 0;
            }

            //Create a description for the accolade
            string _description = accolades[(int)_accoladeEnum].description + "\n"; 
            if(_winners.Count == 1)
            {
                _description += "P" + (_winners[0]+1) + ": " + CharacterProfiles.instance.GetProfile(GlobalPlayerManager.instance.playerData[_winners[0]].character).characterName + "\n";
                _description += accolades[(int)_accoladeEnum].isFloat ? _winnerValue : (int)_winnerValue;
                _description += " " + accolades[(int)_accoladeEnum].afterDescription;
            }
            else
            {
                _description += "P" + (_winners[0]+1);
                for (int i = 1; i < _winners.Count; ++i)
                {
                    _description += "/" + "P" + (_winners[i]+1);
                }
                _description += "\n";
                _description += accolades[(int)_accoladeEnum].isFloat ? _winnerValue : (int)_winnerValue;
                _description += " " + accolades[(int)_accoladeEnum].afterDescription;
            }
                
            displays[_display].Init(accolades[(int)_accoladeEnum].accoladeName, _description);

            //Give all winners a chevron
            for (int i = 0; i < _winners.Count; ++i)
            {

                GameObject _accolade = SetupPin(accolades[(int)_accoladeEnum].accoladeSprite, displays[_display].gameObject.transform.position, Vector3.one * 2.75f, displays[_display].gameObject);
                if (_accoladeEnum == GlobalPlayerManager.instance.currentGameAccolade)
                {
                    ++awardAccolades[4];
                }
                else
                {
                    ++awardAccolades[(int)_accoladeEnum];
                }
                scoreFiles[_winners[i]].GiveAccolade(_accolade, _accoladeEnum, RemoveAccoladeDetails);
            }

            
        }

        void RemoveAccoladeDetails(AccoladeEnum _accoladeEnum, GameObject _accolade)
        {
            //An opacity fade may be better
            if(_accoladeEnum == GlobalPlayerManager.instance.currentGameAccolade)
            {
                --awardAccolades[4];
                if (awardAccolades[4] == 0)
                {
                    displays[4].FadeOut();
                }
            }
            else
            {
                --awardAccolades[(int)_accoladeEnum];
                if (awardAccolades[(int)_accoladeEnum] == 0)
                {
                    displays[(int)_accoladeEnum].FadeOut();
                }
            }
            _accolade.transform.SetParent(pinParent.transform);
        }

        GameObject SetupPin(Sprite _sprite, Vector3 _pos, Vector3 _scale, GameObject _parent)
        {
            GameObject _pin = Instantiate(pinPrefab);
            _pin.transform.SetParent(_parent.transform);
            _pin.hideFlags = HideFlags.HideInHierarchy;
            _pin.GetComponent<Image>().sprite = _sprite;
            _pin.transform.position = _pos;
            _pin.transform.localScale = _scale;
            return _pin;
        }
    }
}
