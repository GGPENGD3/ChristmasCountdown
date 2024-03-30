using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuBrain : MonoBehaviour
{
    public Button OptionBtn;
    // Start is called before the first frame update
    void Start()
    {
        OptionBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
