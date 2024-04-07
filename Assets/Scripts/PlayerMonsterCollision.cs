using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterCollision : MonoBehaviour
{
    public WhoIsMonster whoIsMonsterScript;
    public PlayerBecomeMonster becomeMonsterScript;
    bool checkedMonster;
    public GameObject caughtPlayer;
    public Transform cameraLookAt;
    public PlayerEventTrigger eventTrigger;
    [Header("Immunity Variables")]
    public float immunityTimer;
    public float resetTimer;
    public bool immune;
    public string player_A_Bttn;
    public GameObject player;


    private void Start()
    {
        player = this.gameObject;
        #region Find Player Number
        //assigns a string to variable num based on the player assigned to this script
        //num will be then used to determine which joystick is being used for input
        if (player.name == "Player 1")
        {
            player_A_Bttn = "P1 A";
        }
        else if (player.name == "Player 2")
        {
            player_A_Bttn = "P2 A";
        }
        else if (player.name == "Player 3")
        {
            player_A_Bttn = "P3 A";
        }
        else if (player.name == "Player 4")
        {
            player_A_Bttn = "P4 A";
        }
        #endregion

        eventTrigger = GetComponentInChildren<PlayerEventTrigger>();
        becomeMonsterScript = GetComponent<PlayerBecomeMonster>();
        Debug.Log("assigned PMC script");
    }
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

        if (becomeMonsterScript.isMonster)
        {
            if (Input.GetButtonDown(player_A_Bttn))
            {
                eventTrigger.SetMonsterAttack();

                if (caughtPlayer!=null)
                {
                    if (!caughtPlayer.GetComponent<PlayerBecomeMonster>().isMonster)
                    {
                        StartCoroutine(Capture());
                    }
                }
            }
        }
       

    }

    #region AI Catch Player Code
    private void OnCollisionEnter(Collision collision)
    {
        #region Setting which player to become the monster based on who the player who catched them

        if (gameObject != null)
        {
            if (gameObject.tag == "Monster" && collision.gameObject.GetComponent<PlayerMonsterCollision>().immune == false)
            {
                FindObjectOfType<AudioManager>().Play("sfx", "player_die");
                if (collision.gameObject.tag == "P1")
                {
                    caughtPlayer = collision.gameObject;
                    //whoIsMonsterScript.currentMonsterPlayer = "P1";
                    //immune = true;

                    ////find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
                    //collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
                    StartCoroutine(Capture());
                }
                else if (collision.gameObject.tag == "P2")
                {
                    caughtPlayer = collision.gameObject;
                    //whoIsMonsterScript.currentMonsterPlayer = "P2";
                    //immune = true;

                    ////find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
                    //collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
                    StartCoroutine(Capture());
                }
                else if (collision.gameObject.tag == "P3")
                {
                    caughtPlayer = collision.gameObject;
                    //whoIsMonsterScript.currentMonsterPlayer = "P3";
                    //immune = true;

                    ////find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
                    //collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
                    StartCoroutine(Capture());
                }
                else if (collision.gameObject.tag == "P4")
                {
                    caughtPlayer = collision.gameObject;
                    //whoIsMonsterScript.currentMonsterPlayer = "P4";
                    //immune = true;

                    ////find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
                    //collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
                    StartCoroutine(Capture());
                }
            }
        }
        //if (gameObject.tag == "Monster" && collision.gameObject.GetComponent<PlayerMonsterCollision>().immune == false)
        //{
        //    if (collision.gameObject.tag == "P1")
        //    {
        //        whoIsMonsterScript.currentMonsterPlayer = "P1";
        //        immune = true;

        //        //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
        //        collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
        //    }
        //    else if (collision.gameObject.tag == "P2")
        //    {
        //        whoIsMonsterScript.currentMonsterPlayer = "P2";
        //        immune = true;

        //        //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
        //        collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
        //    }
        //    else if (collision.gameObject.tag == "P3")
        //    {
        //        whoIsMonsterScript.currentMonsterPlayer = "P3";
        //        immune = true;

        //        //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
        //        collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
        //    }
        //    else if (collision.gameObject.tag == "P4")
        //    {
        //        whoIsMonsterScript.currentMonsterPlayer = "P4";
        //        immune = true;

        //        //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
        //        collision.gameObject.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;
        //    }
        //}
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1") || other.CompareTag("P2") || other.CompareTag("P3") || other.CompareTag("P4"))
        {
            caughtPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1") || other.CompareTag("P2") || other.CompareTag("P3") || other.CompareTag("P4"))
        {
            caughtPlayer = null;
        }
    }
    public IEnumerator Capture()
    {
        LookAt(caughtPlayer.transform);
        //eventTrigger.SetMonsterAttack();
        caughtPlayer.GetComponent<FPS_Controller>().LookAt(transform);
        caughtPlayer.GetComponent<FPS_Controller>().CameraLookAt(cameraLookAt);
        caughtPlayer.GetComponent<FPS_Controller>().playerCanMove = false;
        yield return new WaitForSeconds(1f);
        eventTrigger.SetMonsterPossess();
        yield return new WaitForSeconds(3f);
        PlayerToMonster();
    }
    public void LookAt(Transform target)
    {
        transform.LookAt(target);
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = 0; // Set x-axis rotation to 0
        eulerRotation.z = 0; // Set z-axis rotation to 0
        transform.eulerAngles = eulerRotation;
        Debug.Log("looking at");
    }

    void PlayerToMonster()
    {
        if (caughtPlayer.gameObject.tag == "P1")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P1";
            immune = true;

            //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
            caughtPlayer.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;

        }
        else if (caughtPlayer.gameObject.tag == "P2")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P2";
            immune = true;

            //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
            caughtPlayer.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;

        }
        else if (caughtPlayer.gameObject.tag == "P3")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P3";
            immune = true;

            //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
            caughtPlayer.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;

        }
        else if (caughtPlayer.gameObject.tag == "P4")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P4";
            immune = true;

            //find the child object called "Furby Collider?" then access the "Furby Picker" script in it to make drop true
            caughtPlayer.transform.Find("Furby Collider?").GetComponent<FurbyPicker>().drop = true;

        }
    }
    #endregion
}
