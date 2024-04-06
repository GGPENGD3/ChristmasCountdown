using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    public List<GameObject> plushiesToFill;
    public List<string> slotTaken;
    public List<GameObject> plushies;
    public bool completed;
    Transform currentEmptyPos;
    public int plushieCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(plushiesToFill[plushieCounter].name);
        }
    }

    public void CheckForEmptySpot(GameObject Furby)
    {
        for (int i = 0; i < slotTaken.Count; i++)
        {
            if (slotTaken[i] != null)
            {
                slotTaken[i] = Furby.name;
               // currentEmptyPos = plushiesToFill[i];

                //PlaceFurby(Furby.name);
                //PlaceToy(Furby);
                break;
            }
        }
    }

  
    public void PlaceFurby(string FurbyName)
    {
        for (int i = 0; i < plushies.Count; i++)
        {
            if (plushies[i].name == FurbyName)
            {
                Instantiate(plushies[i], currentEmptyPos);

                FindObjectOfType<AudioManager>().Play("sfx", "plush_get");
            }
        }
    }

    public void PlaceToy(GameObject toy)
    {
        if (plushieCounter <=7)
        {
            toy.transform.SetParent(plushiesToFill[plushieCounter].transform) ;
            toy.transform.position = (plushiesToFill[plushieCounter]).transform.position;
            plushieCounter++;
        }
   
    }

   
}
