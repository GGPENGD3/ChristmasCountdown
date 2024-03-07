using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LookAtPlayer : MonoBehaviour
{
    public Transform playerCam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + playerCam.rotation * Vector3.forward, playerCam.rotation * Vector3.up);
    }
}
