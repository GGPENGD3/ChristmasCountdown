using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    public List<GameObject> toys;
    public List<Transform> dropPoints;
    public int collectedToys;
    public GameObject currentToy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toy"))
        {
            currentToy = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Toy"))
        {
            currentToy = null;
        }
    }

    public void CollectToy()
    {
        if (currentToy!=null)
        {
            toys.Add(currentToy);
 
        }
        
    }
}
