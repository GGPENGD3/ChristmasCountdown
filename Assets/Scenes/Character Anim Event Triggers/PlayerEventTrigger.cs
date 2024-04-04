using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventTrigger : MonoBehaviour
{
    public Animator playerAnim;

    [Header("Other Variables")]
    public FurbyPicker furbyPickerScript;

    public void SetPickUp(bool option)
    {
        playerAnim.SetBool("PickUp", option);
    }

    public void SetCarry(bool option)
    {
        playerAnim.SetBool("Carry", option);
    }

    public void ChangePickUpToCarry()
    {
        furbyPickerScript.FinishPickUp();
    }
}
