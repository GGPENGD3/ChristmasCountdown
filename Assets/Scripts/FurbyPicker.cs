using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyPicker : MonoBehaviour
{
    public bool canInteract;
    public GameObject interactUI;

    public bool drop;
    public LayerMask groundLayer;
    public GameObject currentFurby;
    string currentFurbyName;
    bool added;
   
    [Header("Player")]
    public GameObject player;
    public GameObject currentlyHeldFurby;
    public Transform plushieHoldPos;
    public PlayerEventTrigger eventTrigger;
    public string player_A_Bttn;

    [Header("Plushie Prefabs")]
    public List<GameObject> plushies;
    
    [Header("Christmas Tree")]
    public bool canInteractwTree;
    public ChristmasTree myCTScript;
    public GameObject plushieTransform;
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

        //myCTScript = GameObject.Find("Christmas Tree").GetComponent<ChristmasTree>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && eventTrigger.playerAnim.GetBool("Run") == false && eventTrigger.playerAnim.GetBool("Crouch") == false)
        {
            if (Input.GetButtonDown(player_A_Bttn))
            {
                //play pickup animation
                eventTrigger.SetPickUp(true);
                player.GetComponent<FPS_Controller>().playerCanMove = false;
                currentFurbyName = currentFurby.name;
     
                //place furby on player?
            }
        }
        else
        {
            added = false;
        }

        if (canInteractwTree && eventTrigger.playerAnim.GetBool("Run") == false && eventTrigger.playerAnim.GetBool("Crouch") == false)
        {
            if (Input.GetButtonDown(player_A_Bttn))
            {
                Debug.Log("A button pressed");
                //drop furby
                if (!myCTScript.completed)
                {
                    //myCTScript.CheckForEmptySpot(currentlyHeldFurby);
                    //myCTScript.PlaceToy(currentlyHeldFurby);
                    plushieTransform = myCTScript.plushiesToFill[myCTScript.plushieCounter];
                    currentlyHeldFurby.transform.SetParent(plushieTransform.transform);
                    currentlyHeldFurby.transform.position = plushieTransform.transform.position;
                    currentlyHeldFurby = null;
                  
                }
            }
        }

        if (drop)
        {
            //drop furby
            if (currentlyHeldFurby != null)
            {
                
                currentlyHeldFurby.transform.SetParent(null);  
                DropFurby();        
            }
            else
            {
                drop = false;
            }
        }

        //insert code whereby player drops 
    }

    public void DropFurby()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentlyHeldFurby.transform.position,Vector3.down, out hit, Mathf.Infinity,groundLayer))
        {
            currentlyHeldFurby.transform.position = hit.point;
            currentlyHeldFurby = null;

            eventTrigger.SetCarry(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Furby")
        {
            //if ( other !=null)
            {
                canInteract = true;
                //interactUI.SetActive(true);
                currentFurby = other.gameObject;
                plushieTransform = myCTScript.plushiesToFill[myCTScript.plushieCounter];
            }

        }
        if (other.gameObject.name == "Christmas Tree" && currentlyHeldFurby != null)
        {
            canInteractwTree = true;
            Debug.Log("Collided w tree");
            plushieTransform = null;
            //Set Interact UI For Tree to be active;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Furby")
        {
            canInteract = false;
            //interactUI.SetActive(false);
            currentFurby = null;
        }

        if (other.gameObject.name == "Christmas Tree" && currentlyHeldFurby != null)
        {
            canInteractwTree = false;
            Debug.Log("triggerexit w tree");
            //Set Interact UI For Tree to be active;
        }
    }

    public void FinishPickUp()
    {
        if (canInteract)
        {
            if (!added)
            {
                added = true;
                canInteract = false;

                //once pickup animation is done, set carry to true
                eventTrigger.SetPickUp(false);
                eventTrigger.SetCarry(true);
                player.GetComponent<FPS_Controller>().playerCanMove = true;

                //spawn plushie onto player holding positiion

                for (int i = 0; i < plushies.Count; i++)
                {
                    if (plushies[i].name == currentFurbyName)
                    {
                        currentlyHeldFurby = Instantiate(plushies[i], plushieHoldPos);
                        currentlyHeldFurby.name = plushies[i].name;
                    }
                }

                Destroy(currentFurby);
                currentFurby = null;
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Christmas Tree" && currentlyHeldFurby != null)
    //    {
    //        canInteractwTree = true;
    //        Debug.Log("Collided w tree");
    //        //Set Interact UI For Tree to be active;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.name == "Christmas Tree" && currentlyHeldFurby != null)
    //    {
    //        canInteractwTree = false;
    //        //Set Interact UI For Tree to be active;
    //    }
    //}
}
