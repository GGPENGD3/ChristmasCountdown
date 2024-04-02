using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBecomeMonster : MonoBehaviour
{
    public bool becomeMonster;
    public GameObject playerModel;
    public GameObject monsterModel;
    public Transform playerCam;
    public Vector3 monsterCamPos;

    // Update is called once per frame
    void Update()
    {
        if (becomeMonster)
        {
            //disable player model, enable monster model
            playerModel.SetActive(false);
            monsterModel.SetActive(true);

            //set playerCam to monsterCam position
            playerCam.position = monsterCamPos;

            becomeMonster = false;
        }
    }
}
