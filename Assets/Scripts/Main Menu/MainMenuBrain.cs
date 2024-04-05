using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuBrain : MonoBehaviour
{
    public string currentMenu; //MainMenu, OptionMenu, HowToPlay, Credits

    [Header("Option Menu Variables")]
    public Button OptionBtn;

    [Header("How To Play Variables")]
    public GameObject HowToPlayUI;

    [Header("Main Menu Variables")]
    public List<Button> MainMenuButtons;
    public string currentBttn;
    public bool calledFunction;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = "MainMenu";
        
        FindObjectOfType<AudioManager>().Play("bgm", "title");
    }

    // Update is called once per frame
    void Update()
    {
        #region Main Menu Stuff
        //Controller Input For MainMenu
        if (currentMenu == "MainMenu")
        {
            if (Input.GetAxis("P1 Left ThumbStick Horizontal") < 0)
            {
                //go down the list of buttons
                if (!calledFunction)
                {
                    GoDownMainMenuButtons();
                }
            }
            else if (Input.GetAxis("P1 Left ThumbStick Horizontal") > 0)
            {
                //go up the list of buttons
                if (!calledFunction)
                {
                    GoUpMainMenuButtons();
                }
            }
            else if (Input.GetAxis("P1 Left ThumbStick Horizontal") == 0)
            {
                //go up the list of buttons
                if (calledFunction)
                {
                    calledFunction = false;
                }
            }

            if (Input.GetButtonDown("P1 A"))
            {
                if (currentBttn == "Option")
                {
                    MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.selectedSprite;
                    OptionsMenu();
                }
                else if (currentBttn == "HowToPlay")
                {
                    MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.selectedSprite;
                    HowToPlay();
                }
                else if (currentBttn == "Quit")
                {
                    MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.selectedSprite;
                    ExitGame();
                }
                else if (currentBttn == "Start")
                {
                    MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.selectedSprite;
                    StartGame();
                }
            }
        }
        #endregion
    }

    #region button functions
    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("ui", "confirm");
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().StopBundle("bgm");
        FindObjectOfType<AudioManager>().Play("bgm", "normal");
    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Play("ui", "confirm");
        Application.Quit();
    }

    public void OptionsMenu()
    {
        FindObjectOfType<AudioManager>().Play("ui", "confirm");
        //open up option menu
    }

    public void CreditsMenu()
    {
        FindObjectOfType<AudioManager>().Play("ui", "confirm");
        //open up credits menu
    }

    public void HowToPlay()
    {
        FindObjectOfType<AudioManager>().Play("ui", "confirm");
        //open up How To Play
        HowToPlayUI.SetActive(true);
        currentMenu = "HowToPlay";
    }
    #endregion

    void GoDownMainMenuButtons()
    {
        FindObjectOfType<AudioManager>().Play("ui", "shift");
        if (currentBttn == "")
        {
            calledFunction = true;
            currentBttn = "Start";
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "Start")
        {
            calledFunction = true;
            currentBttn = "Option";
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.highlightedSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "Option")
        {
            calledFunction = true;
            currentBttn = "HowToPlay";
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "HowToPlay")
        {
            calledFunction = true;
            currentBttn = "Quit";
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
        }
        else if (currentBttn == "Quit")
        {
            calledFunction = true;
            currentBttn = "Start";
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
    }

    void GoUpMainMenuButtons()
    {
        FindObjectOfType<AudioManager>().Play("ui", "shift");
        if (currentBttn == "")
        {
            calledFunction = true;
            currentBttn = "Quit";
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
        }
        else if (currentBttn == "Quit")
        {
            calledFunction = true;
            currentBttn = "HowToPlay";
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.highlightedSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "HowToPlay")
        {
            calledFunction = true;
            currentBttn = "Option";
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.highlightedSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "Option")
        {
            calledFunction = true;
            currentBttn = "Start";
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.disabledSprite;
        }
        else if (currentBttn == "Start")
        {
            calledFunction = true;
            currentBttn = "Quit";
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.highlightedSprite;
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.disabledSprite;
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.disabledSprite;
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.disabledSprite;
        }
    }
}
