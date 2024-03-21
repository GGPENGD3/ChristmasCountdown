using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight")]
    public GameObject mylight;
    public bool isPressed;
    bool onLight;
    float timer;

    [Header("Battery")]
    [SerializeField] float batteryLife = 100;
    [SerializeField] float drainRate = 1;

    public Image batteryLifeImg;
    public List<Sprite> batteryLifeUI;

    [Header("Player")]
    public GameObject player;
    public string player_Y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Find Player Number
        //assigns a string to variable num based on the player assigned to this script
        //num will be then used to determine which joystick is being used for input
        if (player.name == "Player 1")
        {
            player_Y = "P1 Y";
        }
        else if (player.name == "Player 2")
        {
            player_Y = "P2  Y";
        }
        else if (player.name == "Player 3")
        {
            player_Y = "P3  Y";
        }
        else if (player.name == "Player 4")
        {
            player_Y = "P4 Y";
        }
        #endregion

        if (player.name == "Player 1")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isPressed = true;

                if (onLight)
                {
                    mylight.SetActive(false);
                    onLight = false;
                }
                else if (!onLight)
                {
                    mylight.SetActive(true);
                    onLight = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                isPressed = false;
                timer = 0;
            }
        }
        else
        {
            if (Input.GetButtonDown(player_Y))
            {
                isPressed = true;

                if (onLight)
                {
                    mylight.SetActive(false);
                    onLight = false;
                }
                else if (!onLight)
                {
                    mylight.SetActive(true);
                    onLight = true;
                }
            }

            if (Input.GetButtonUp(player_Y))
            {
                isPressed = false;
                timer = 0;
            }
        }

        if (isPressed)
        {
            timer += 0.1f;
        }

        if (timer > 5f)
        {
            Debug.Log("Charging Time");
        }

        if (onLight)
        {
            DecreaseBatteryLife();
        }
    }

    void DecreaseBatteryLife()
    {
        batteryLife -= drainRate;

        if (batteryLife <= 75 && batteryLife > 50)
        {
            batteryLifeImg.sprite = batteryLifeUI[1];
        }
        else if (batteryLife <= 50 && batteryLife > 25)
        {
            batteryLifeImg.sprite = batteryLifeUI[2];
        }
        else if (batteryLife <= 25 && batteryLife > 0)
        {
            batteryLifeImg.sprite = batteryLifeUI[3];
        }
        else if (batteryLife <= 0)
        {
            batteryLifeImg.sprite = batteryLifeUI[4];
        }
    }

    void ChargeBattery()
    {

    }
}
