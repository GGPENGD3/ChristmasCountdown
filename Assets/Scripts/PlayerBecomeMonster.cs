using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBecomeMonster : MonoBehaviour
{
    public bool becomeMonster;
    public bool becomePlayer;
    public GameObject playerModel;
    public GameObject monsterModel;
    public Transform playerCam;
    public Vector3 monsterCamPos;

    [Header("Black Screen Varibles")]
    public Image blackScreen;
    public bool fadeIn;
    public bool fadeOut;

    // Update is called once per frame
    void Update()
    {
        if (becomeMonster)
        {
            //disable player model, enable monster model
            playerModel.SetActive(false);
            monsterModel.SetActive(true);

            //set playerCam to monsterCam position
            //playerCam.position = monsterCamPos;

            fadeIn = true;
            becomeMonster = false;
        }

        if (becomePlayer)
        {
            //disable monster model, enable player model
            playerModel.SetActive(true);
            monsterModel.SetActive(false);

            becomePlayer = false;
        }

        #region Black Screen
        if (fadeIn)
        {
            var color = blackScreen.color;
            color.a += Time.deltaTime;
            blackScreen.color = color;

            if (color.a >= 1)
            {
                fadeIn = false;
                fadeOut = true;
            }
        }

        if (fadeOut)
        {
            var newColor = blackScreen.color;
            newColor.a -= Time.deltaTime / 2;
            blackScreen.color = newColor;

            if (newColor.a <= 0)
            {
                fadeOut = false;
            }
        }
        #endregion
    }
}
