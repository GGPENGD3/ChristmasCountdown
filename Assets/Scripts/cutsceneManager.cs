using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceneManager : MonoBehaviour
{
    public void SkiptoNextScene()
    {
        SceneManager.LoadScene("Gameplay Scene");
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
