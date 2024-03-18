using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool isPressed;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P1 Y"))
        {
            isPressed = true;
        }

        if (Input.GetButtonUp("P1 Y"))
        {
            isPressed = false;
            timer = 0;
        }

        if (isPressed)
        {
            timer += 0.1f;
        }

        if (timer > 5f)
        {
            Debug.Log("Charging Time");
        }
    }
}
