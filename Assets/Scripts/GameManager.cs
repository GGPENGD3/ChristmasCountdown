using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public ChristmasTree christmasTree;
    public GameObject player1, player2, player3, player4;
    public GameObject PauseMenu;
    public GameObject puffyCollected;
    // Start is called before the first frame update
    void Start()
    {
        
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
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Ending_cutscene");
    }

    IEnumerator FlashPuffy()
    {
        puffyCollected.SetActive(true);
        yield return new WaitForSeconds(2f);
        puffyCollected.SetActive(false);
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
    }
}
