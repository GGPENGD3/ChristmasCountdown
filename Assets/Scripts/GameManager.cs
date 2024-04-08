using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public ChristmasTree christmasTree;
    public GameObject PauseMenu;
    public GameObject puffyCollected;
    public bool gameEnd;
    // Start is called before the first frame update
    void Start()
    {
        puffyCollected.SetActive(false);
        PauseMenu.SetActive(false);
        FindObjectOfType<AudioManager>().Play("bgm", "normal");
        gameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (christmasTree.completed)
        {
            StartCoroutine(EndGame());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (Cursor.lockState == CursorLockMode.Locked)
            {   
                Cursor.lockState = CursorLockMode.None;
                Debug.Log(Cursor.lockState.ToString());
                PauseMenu.SetActive(true);
            }

           else if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState=CursorLockMode.Locked;
                if (PauseMenu.activeSelf)
                {
                    PauseMenu.SetActive(false);
                }
            }
        }

   
    }

    IEnumerator EndGame()
    {
       if (!gameEnd)
        {
            FindObjectOfType<AudioManager>().Play("bgm", "timeup");
            gameEnd = true;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Ending_cutscene");
  
        }
        
    }

    public IEnumerator FlashPuffy()
    {
        FindObjectOfType<AudioManager>().Play("sfx", "plush_get");    
        puffyCollected.SetActive(true);
        yield return new WaitForSeconds(2f);
        puffyCollected.SetActive(false);
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
