using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventTrigger : MonoBehaviour
{
    public Animator playerAnim;
    public Animator monsterAnim;
    [Header("Other Variables")]
    public FurbyPicker furbyPickerScript;

    #region Player Animation Booleans
    public void SetRun(bool option)
    {
        playerAnim.SetBool("Run", option);
        monsterAnim.SetBool("Run", option);
  
    }
    public void SetCarryRun(bool option)
    {
        playerAnim.SetBool("CarryRun", option);
    }

    public void SetPickUp(bool option)
    {
        playerAnim.SetBool("PickUp", option);
    }

    public void SetCrouch(bool option)
    {
        playerAnim.SetBool("Crouch", option);
    }

    public void SetCarry(bool option)
    {
        playerAnim.SetBool("Carry", option);
    }

    public void SetCrouchWalk(bool option)
    {
        playerAnim.SetBool("CrouchWalk", option);
    }

    public void SetStartShaking(bool option)
    {
        playerAnim.SetBool("StartShaking", option);
    }

    public void SetShaking(bool option)
    {
        playerAnim.SetBool("Shaking", option);
    }
    #endregion

    public void ChangePickUpToCarry()
    {
        furbyPickerScript.FinishPickUp();
    }
}
