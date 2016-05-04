using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace ArmadHeroes
{
    public class CharacterScoreFile : MonoBehaviour
    {
        [SerializeField] private Image mainCharacter;
        [SerializeField] private Text characterName;
        [SerializeField] private Image file;
        [SerializeField] private Image number;
        [SerializeField] private Shader greyscaleShader;
        [SerializeField] private Image MIA;

        private int playerID;
        private int controllerID;
       

        public Controller controller
        {
            private set { }
            get { return ControllerManager.instance.GetController(controllerID); }
        }

        [SerializeField] private GameObject chevronLocation = null;
        private GameObject currentChevron = null;
        //private GameObject newChevron = null;

        [SerializeField] private GameObject accoladeUpperLeftPivot = null;
        [SerializeField] private GameObject accoladeLowerRightPivot = null;

        private int isReady = 0;
        public bool AnimationsFinished() { return isReady > 0 ? false : true; }

        public void SetupFile(int _ID)
        {
            playerID = _ID;
            controllerID = GlobalPlayerManager.instance.playerData[playerID].controllerIndex;

            Material _material = new Material(greyscaleShader);
            _material.color = new Color(1, 1, 1, 1);

            if (GlobalPlayerManager.instance.playerData[playerID].activePlayer)
            {
                _material.SetFloat("_GrayscaleAmount", 0);
                CharacterProfile _profile = CharacterProfiles.instance.GetProfile(GlobalPlayerManager.instance.playerData[playerID].character);
                mainCharacter.sprite = _profile.characterFileImage_Debrief;
                characterName.text = _profile.characterName;
            }
            else
            {
                _material.SetFloat("_GrayscaleAmount", 1);
                MIA.gameObject.SetActive(true);
                MIA.gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(-30, 30));
            }
            mainCharacter.material = _material;
            file.material = _material;
            number.material = _material;

        }

        public void AssignChevron(GameObject _chevron)
        { 
            _chevron.transform.position = chevronLocation.transform.position;
            _chevron.transform.localScale = Vector3.one * 0.75f;
            currentChevron = _chevron;
        }

        public void Promotion(GameObject _newChevron)
        {
            _newChevron.GetComponent<Pin>().Init(controllerID, chevronLocation.transform.position, Vector3.one * 0.75f, 1.0f, AnimationFinish);
            ++isReady;
            //newChevron = _newChevron;
            if (currentChevron)
            {
                currentChevron.GetComponent<Pin>().DestroyPin(1.5f);
            }
        }

        public void GiveAccolade(GameObject _newAccolade, AccoladeEnum _accoladeEnum, Pin.WaitReached _reach)
        {
            float _x = Random.Range(accoladeUpperLeftPivot.transform.position.x, accoladeLowerRightPivot.transform.position.x);
            float _y = Random.Range(accoladeUpperLeftPivot.transform.position.y, accoladeLowerRightPivot.transform.position.y);
            Vector3 random = new Vector3(_x, _y, 0.0f);
            _newAccolade.GetComponent<Pin>().Init(controllerID, random, Vector3.one * 0.9f, 20.0f, AnimationFinish, _reach, _accoladeEnum);
            ++isReady;
            GlobalPlayerManager.instance.playerData[playerID].accolades[(int)_accoladeEnum].accoladesPositions.Add(random);
        }

        void AnimationFinish()
        {
            --isReady;
        }
    }
}


