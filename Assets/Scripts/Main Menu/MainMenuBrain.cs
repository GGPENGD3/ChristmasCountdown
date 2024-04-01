using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
        OptionBtn.interactable = false;
        currentMenu = "MainMenu";
    }

    // Update is called once per frame
    void Update()
    {
        #region Main Menu Stuff
        //Controller Input For MainMenu
        if (currentMenu == "MainMenul")
        {
            if (Input.GetAxis("P1 Right ThumbStick Horizontal") < 0)
            {
                //go down the list of buttons
                if (!calledFunction)
                {
                    SelectingMainMenuBttn();
                }
            }
            else if (Input.GetAxis("P1 Right ThumbStick Horizontal") > 0)
            {
                //go up the list of buttons
                if (!calledFunction)
                {
                    SelectingMainMenuBttn();
                }
            }
            else if (Input.GetAxis("P1 Right ThumbStick Horizontal") == 0)
            {
                calledFunction = true;
            }
        }
        #endregion
    }

    #region button functions
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        //open up option menu
    }

    public void CreditsMenu()
    {
        //open up credits menu
    }

    public void HowToPlay()
    {
        //open up How To Play
        HowToPlayUI.SetActive(true);
        currentMenu = "HowToPlay";
    }
    #endregion

    void SelectingMainMenuBttn()
    {
        if (currentBttn == "")
        {
            calledFunction = false;
            currentBttn = "Start";
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.highlightedSprite;
        }
        else if (currentBttn == "Start")
        {
            calledFunction = false;
            currentBttn = "Option";
            MainMenuButtons[1].image.sprite = MainMenuButtons[1].spriteState.highlightedSprite;
        }
        else if (currentBttn == "Option")
        {
            calledFunction = false;
            currentBttn = "HowToPlay";
            MainMenuButtons[2].image.sprite = MainMenuButtons[2].spriteState.highlightedSprite;
        }
        else if (currentBttn == "HowToPlay")
        {
            calledFunction = false;
            currentBttn = "Quit";
            MainMenuButtons[3].image.sprite = MainMenuButtons[3].spriteState.highlightedSprite;
        }
        else if (currentBttn == "Quit")
        {
            calledFunction = false;
            currentBttn = "Start";
            MainMenuButtons[0].image.sprite = MainMenuButtons[0].spriteState.highlightedSprite;
        }
    }
}
