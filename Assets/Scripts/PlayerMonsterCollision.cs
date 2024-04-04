using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterCollision : MonoBehaviour
{
    public WhoIsMonster whoIsMonsterScript;
    bool checkedMonster;

    [Header("Immunity Variables")]
    public float immunityTimer;
    public float resetTimer;
    public bool immune;

    private void Update()
    {
        if (immune)
        {
            if (!checkedMonster)
            {
                whoIsMonsterScript.ChangeMonster();
                checkedMonster = true;
            }

            if (immunityTimer >= 0)
            {
                immunityTimer -= Time.deltaTime;
            }
            else
            {
                immune = false;
                immunityTimer = resetTimer;
                checkedMonster = false;
            }
        }
    }

    #region AI Catch Player Code
    private void OnCollisionEnter(Collision collision)
    {
        #region Setting which player to become the monster based on who the player who catched them
        if (gameObject.tag == "Monster" && collision.gameObject.GetComponent<PlayerMonsterCollision>().immune == false)
        {
            if (collision.gameObject.tag == "P1")
            {
                whoIsMonsterScript.currentMonsterPlayer = "P1";
                immune = true;
            }
            else if (collision.gameObject.tag == "P2")
            {
                whoIsMonsterScript.currentMonsterPlayer = "P2";
                immune = true;
            }
            else if (collision.gameObject.tag == "P3")
            {
                whoIsMonsterScript.currentMonsterPlayer = "P3";
                immune = true;
            }
            else if (collision.gameObject.tag == "P4")
            {
                whoIsMonsterScript.currentMonsterPlayer = "P4";
                immune = true;
            }
        }
        #endregion
    }
    #endregion
}
