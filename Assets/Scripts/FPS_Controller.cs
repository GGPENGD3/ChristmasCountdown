using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Controller : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 targetVelocity;

    public GameObject player;
    string player_RightThumbStickHorizontal;
    string player_RightThumbStickVertical;
    string player_LeftThumbStickHorizontal;
    string player_LeftThumbStickVertical;
    string player_RightThumbstickBttn;
    string player_B_Bttn;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 2f;
    public float maxVelocityChange = 10f;
    public bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public float sprintSpeed = 3f;
    public float currentSprintSpeed;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Internal Variables
    public bool isSprinting = false;
    private float sprintRemaining;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Set Internal Variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    float camRotation;

    // Update is called once per frame
    void Update()
    {
        #region Find Player Number
        //assigns a string to variable num based on the player assigned to this script
        //num will be then used to determine which joystick is being used for input
        if (player.name == "Player 1")
        {
            player_RightThumbStickHorizontal = "P1 Right ThumbStick Horizontal";
            player_RightThumbStickVertical = "P1 Right ThumbStick Vertical";
            player_LeftThumbStickHorizontal = "P1 Left ThumbStick Vertical";
            player_LeftThumbStickVertical = "P1 Left ThumbStick Horizontal";
            player_RightThumbstickBttn = "P1 RB";
            player_B_Bttn = "P1 B";
        }
        else if (player.name == "Player 2")
        {
            player_RightThumbStickHorizontal = "P2 Right ThumbStick Horizontal";
            player_RightThumbStickVertical = "P2 Right ThumbStick Vertical";
            player_LeftThumbStickHorizontal = "P2 Left ThumbStick Vertical";
            player_LeftThumbStickVertical = "P2 Left ThumbStick Horizontal";
            player_RightThumbstickBttn = "P2 RB";
            player_B_Bttn = "P2 B";
        }
        else if (player.name == "Player 3")
        {
            player_RightThumbStickHorizontal = "P3 Right ThumbStick Horizontal";
            player_RightThumbStickVertical = "P3 Right ThumbStick Vertical";
            player_LeftThumbStickHorizontal = "P3 Left ThumbStick Vertical";
            player_LeftThumbStickVertical = "P3 Left ThumbStick Horizontal";
            player_RightThumbstickBttn = "P3 RB";
            player_B_Bttn = "P3 B";
        }
        else if (player.name == "Player 4")
        {
            player_RightThumbStickHorizontal = "P4 Right ThumbStick Horizontal";
            player_RightThumbStickVertical = "P4 Right ThumbStick Vertical";
            player_LeftThumbStickHorizontal = "P4 Left ThumbStick Vertical";
            player_LeftThumbStickVertical = "P4 Left ThumbStick Horizontal";
            player_RightThumbstickBttn = "P4 RB";
            player_B_Bttn = "P4 B";
        }
        #endregion

        #region Camera
        // Control camera movement
        if (cameraCanMove)
        {
            if (Input.GetAxis(player_RightThumbStickHorizontal) != 0 || Input.GetAxis(player_RightThumbStickVertical) != 0)
            {
                yaw = transform.localEulerAngles.y + Input.GetAxis(player_RightThumbStickHorizontal) * mouseSensitivity;

                if (!invertCamera)
                {
                    pitch -= mouseSensitivity * Input.GetAxis(player_RightThumbStickVertical);
                }
                else
                {
                    // Inverted Y
                    pitch += mouseSensitivity * Input.GetAxis(player_RightThumbStickVertical);
                }

                // Clamp pitch between lookAngle
                pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

                transform.localEulerAngles = new Vector3(0, yaw, 0);
                playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            }
        }

        #region Camera Zoom

        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toogle zoom
            if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
            {
                if (!isZoomed)
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (holdToZoom && !isSprinting)
            {
                if (Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if (Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed && !isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }

        #endregion
        #endregion

        #region Sprint

        if (enableSprint)
        {
            if (isSprinting)
            {
                isZoomed = false;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

                // Drain sprint remaining while sprinting
                if (!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Time.deltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                // Regain sprint while not sprinting
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }

            // Handles sprint cooldown 
            // When sprint remaining == 0 stops sprint ability until hitting cooldown
            if (isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }
        }

        #endregion

        #region Jump

        // Gets input and calls jump method
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch

        if (enableCrouch)
        {
            if (Input.GetButtonDown(player_B_Bttn))
            {
                if (!holdToCrouch)
                {
                    Crouch();
                }
                else if (holdToCrouch)
                {
                    isCrouched = false;
                    Crouch();
                }
            }

            if (Input.GetButtonUp(player_B_Bttn))
            {
                if (holdToCrouch)
                {
                    isCrouched = true;
                    Crouch();
                }
            }
        }

        #endregion

        CheckGround();

        if (enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        #region Movement

        if (playerCanMove)
        {
            if (Input.GetAxis(player_LeftThumbStickVertical) != 0 || Input.GetAxis(player_LeftThumbStickHorizontal) != 0)
            {
                // Calculate how fast we should be moving
                targetVelocity = new Vector3(Input.GetAxis(player_LeftThumbStickHorizontal), 0, Input.GetAxis(player_LeftThumbStickVertical));
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // Checks if player is walking and isGrounded
            // Will allow head bob
            //if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            //{
            //    isWalking = true;
            //}
            //else
            //{
            //    isWalking = false;
            //}

            // All movement calculations shile sprint is active
                if (enableSprint && Input.GetButton(player_RightThumbstickBttn) && sprintRemaining > 0f && !isSprintCooldown)
            {
                if (currentSprintSpeed < 8)
                {
                    currentSprintSpeed += 0.02f;
                }
                targetVelocity = transform.TransformDirection(targetVelocity) * currentSprintSpeed;
                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    if (isCrouched)
                    {
                        Crouch();
                    }
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;
                currentSprintSpeed = sprintSpeed;

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        #endregion
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if (isCrouched && !holdToCrouch)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }

    private void HeadBob()
    {
        if (isWalking)
        {
            // Calculates HeadBob speed during sprint
            if (isSprinting)
            {
                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            // Calculates HeadBob speed during crouched movement
            else if (isCrouched)
            {
                timer += Time.deltaTime * (bobSpeed * speedReduction);
            }
            // Calculates HeadBob speed during walking
            else
            {
                timer += Time.deltaTime * bobSpeed;
            }
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when player stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
}
