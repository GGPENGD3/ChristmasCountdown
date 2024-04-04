using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyPicker : MonoBehaviour
{
    public bool canInteract;
    public GameObject interactUI;
    public int furbyCount;

    GameObject currentFurby;
    bool added;

    [Header("Player")]
    public GameObject player;
    public PlayerEventTrigger eventTrigger;
    public string player_A_Bttn;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetButtonDown(player_A_Bttn))
            {
                //play pickup animation
                eventTrigger.SetPickUp(true);
                player.GetComponent<FPS_Controller>().playerCanMove = false;

                //place furby on player?
            }
        }
        else
        {
            added = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Furby")
        {
            canInteract = true;
            interactUI.SetActive(true);
            currentFurby = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Furby")
        {
            canInteract = false;
            interactUI.SetActive(false);
            currentFurby = null;
        }
    }

    public void FinishPickUp()
    {
        if (canInteract)
        {
            if (!added)
            {
                furbyCount = furbyCount + 1;
                added = true;
                canInteract = false;

                //once pickup animation is done, set carry to true
                eventTrigger.SetPickUp(false);
                eventTrigger.SetCarry(true);
                player.GetComponent<FPS_Controller>().playerCanMove = true;
                Destroy(currentFurby);
            }
        }
    }
}
