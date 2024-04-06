using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    public List<Transform> plushiesToFill;
    public List<string> slotTaken;
    public List<GameObject> plushies;
    public bool completed;
    Transform currentEmptyPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForEmptySpot(GameObject Furby)
    {
        for (int i = 0; i < slotTaken.Count; i++)
        {
            if (slotTaken[i] != null)
            {
                slotTaken[i] = Furby.name;
                currentEmptyPos = plushiesToFill[i];

                PlaceFurby(Furby.name);
                completed = true;
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
            }
        }
    }
}
