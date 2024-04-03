using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoIsMonster : MonoBehaviour
{
    public string currentMonsterPlayer;
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    public PlayerBecomeMonster p1_PBM_Script;
    public PlayerBecomeMonster p2_PBM_Script;
    public PlayerBecomeMonster p3_PBM_Script;
    public PlayerBecomeMonster p4_PBM_Script;

    public void ChangeMonster()
    {
        if (currentMonsterPlayer == "P1")
        {
            playerOne.tag = "Monster";
            playerTwo.tag = "P2";
            playerThree.tag = "P3";
            playerFour.tag = "P4";

            p1_PBM_Script.becomeMonster = true;
            //p2_PBM_Script.becomePlayer = true;
            //p3_PBM_Script.becomePlayer = true;
            //p4_PBM_Script.becomePlayer = true;
        }
        else if (currentMonsterPlayer == "P2")
        {
            playerTwo.tag = "Monster";
            playerOne.tag = "P1";
            playerThree.tag = "P3";
            playerFour.tag = "P4";

            //p2_PBM_Script.becomeMonster = true;

            p1_PBM_Script.becomePlayer = true;
            //p3_PBM_Script.becomePlayer = true;
            //p4_PBM_Script.becomePlayer = true;
        }
        else if (currentMonsterPlayer == "P3")
        {
            playerThree.tag = "Monster";
            playerOne.tag = "P1";
            playerTwo.tag = "P2";
            playerFour.tag = "P4";

            //p3_PBM_Script.becomeMonster = true;

            p1_PBM_Script.becomePlayer = true;
            //p2_PBM_Script.becomePlayer = true;
            //p4_PBM_Script.becomePlayer = true;
        }
        else if (currentMonsterPlayer == "P4")
        {
            playerFour.tag = "Monster";
            playerOne.tag = "P1";
            playerTwo.tag = "P2";
            playerThree.tag = "P3";

            //p4_PBM_Script.becomeMonster = true;

            p1_PBM_Script.becomePlayer = true;
            //p2_PBM_Script.becomePlayer = true;
            //p3_PBM_Script.becomePlayer = true;
        }
    }
}
