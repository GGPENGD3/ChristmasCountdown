using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterCollision : MonoBehaviour
{
    public WhoIsMonster whoIsMonsterScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region AI Catch Player Code
    private void OnCollisionEnter(Collision collision)
    {
        #region Setting which player to become the monster based on who the player who catched them
        if (collision.gameObject.tag == "P1")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P1";
            whoIsMonsterScript.ChangeMonster();
        }
        else if (collision.gameObject.tag == "P2")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P2";
            whoIsMonsterScript.ChangeMonster();
        }
        else if (collision.gameObject.tag == "P3")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P3";
            whoIsMonsterScript.ChangeMonster();
        }
        else if (collision.gameObject.tag == "P4")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P4";
            whoIsMonsterScript.ChangeMonster();
        }
        #endregion
    }
    #endregion
}
