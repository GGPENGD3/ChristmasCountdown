using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceneManager : MonoBehaviour
{
    public Animator cutsceneAnim;
    public bool intro;
    private void Start()
    {
        if (intro)
        {
            cutsceneAnim.SetTrigger("Intro");
        }
        else

            cutsceneAnim.SetTrigger("End");
    }
    public void SkiptoNextScene()
    {
        SceneManager.LoadScene("Level_Input_level_1");
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
