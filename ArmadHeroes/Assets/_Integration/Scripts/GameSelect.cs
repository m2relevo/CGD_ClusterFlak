using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using ArmadHeroes;

public class GameSelect : MonoBehaviour {

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject leftHighlight;
    public GameObject rightHighlight;

    public static  BaseEventData pointer;

    public EventSystem eventSystem = EventSystem.current;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (controller.boostButton.JustPressed() && eventSystem.currentSelectedGameObject == leftButton )
        {
            loadLeftGame();
        }else if (controller.boostButton.JustPressed() && eventSystem.currentSelectedGameObject == rightButton)
                {
                    loadRightGame();
                }

        pointer = new BaseEventData(eventSystem);

        if (controller.moveX.GetValue() != 0)
        {
            if (controller.moveX.JustPressed())
            {
                int changeDirection = controller.moveX.GetValue() > 0 ? 1 : -1;
                ChangeLevel(changeDirection);
                Debug.Log(changeDirection);
            }
        }


        if (eventSystem.currentSelectedGameObject == leftButton)
        {
            leftHighlight.SetActive(true);
            rightHighlight.SetActive(false);
           

        }
        else if (eventSystem.currentSelectedGameObject == rightButton)
        {
            rightHighlight.SetActive(true);
            leftHighlight.SetActive(false);
        }
    }

    public void loadLeftGame()
    {
        Debug.Log("CLICKED");
        SceneManager.LoadScene("MainMenu");
    }

    public void loadRightGame()
    {
        SceneManager.LoadScene("Prototype");
    }

 

    private Controller controller
    {
        get { return ControllerManager.instance.GetController(controllerIndex); }
        set { }
    }

    private int controllerIndex = 0;

    //Set an controller to control this character file
    public void AssignController(int _controllerIndex)
    {
        controllerIndex = _controllerIndex;
    }

    public  void ChangeLevel(int direction)
    {
        // Selectable newSelectable = leftButton.FindSelectable()

         
        if (eventSystem.currentSelectedGameObject == leftButton)
        {
            eventSystem.SetSelectedGameObject(rightButton, pointer);
          
        }else if (eventSystem.currentSelectedGameObject == rightButton)
                {
                    eventSystem.SetSelectedGameObject(leftButton, pointer);
                }
    }
    //need to check which level is selected, and when A is pressed, call the level function.
}
