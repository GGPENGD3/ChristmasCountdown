using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public bool canVent;
    public Transform destinationPoint;
    public GameObject player;
    public float timer;
    public float ventTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player!=null && canVent)
        {
            if (player.GetComponent<FPS_Controller>().isCrouched)
            {
                //do a button input check to enter the vent

                timer += Time.deltaTime;
                if (timer >= ventTime)
                {
                    player.transform.position = destinationPoint.position;
                }
            }
            else
                timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPS_Controller>()!=null)
        {
            player = other.gameObject;
            canVent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FPS_Controller>()!=null) 
        {
            player = other.gameObject;
            canVent = true;
        }
    }

   
}
