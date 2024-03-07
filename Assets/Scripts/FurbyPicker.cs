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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!added)
                {
                    furbyCount = furbyCount + 1;
                    added = true;
                    Destroy(currentFurby);
                }
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
}
