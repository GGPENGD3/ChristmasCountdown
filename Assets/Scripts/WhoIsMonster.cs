using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoIsMonster : MonoBehaviour
{
    public GameObject currentMonsterPlayer;
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    // Update is called once per frame
    void Update()
    {
        if (currentMonsterPlayer == playerOne)
        {
            currentMonsterPlayer.tag = "Monster";
            playerTwo.tag = "P2";
            playerThree.tag = "P3";
            playerFour.tag = "P4";
        }
        else if (currentMonsterPlayer == playerTwo)
        {
            currentMonsterPlayer.tag = "Monster";
            playerOne.tag = "P1";
            playerThree.tag = "P3";
            playerFour.tag = "P4";
        }
        else if (currentMonsterPlayer == playerThree)
        {
            currentMonsterPlayer.tag = "Monster";
            playerOne.tag = "P1";
            playerTwo.tag = "P2";
            playerFour.tag = "P4";
        }
        else if (currentMonsterPlayer == playerFour)
        {
            currentMonsterPlayer.tag = "Monster";
            playerOne.tag = "P1";
            playerTwo.tag = "P2";
            playerThree.tag = "P3";
        }
    }
}
