using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour //NEEDS COMMENTING
{

    public static bool cameraLocked = false;

    [Header("Movement")]
    public float mouseSensitivity = 2.0f;
    public float jumpHeight = 10f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float movementSpeed;
    public float gravityMultiplier = 4.0f;


    //Movement Variables
    protected float rotatePitch;
    protected float pitchRange = 60.0f;
    protected float forwardDirection;
    protected float strafeDirection;
    protected float verticalVelocity = 0;
    protected bool moving = false;
    public Camera camera;
    protected CharacterController cc;


    protected virtual void Update()
    {
        if (!cameraLocked)
            CameraMovement();
        Inputs();
        Movement();
    }

    protected virtual void Start()
    {
        movementSpeed = walkSpeed;
        cc = GetComponent<CharacterController>();

        LockAndHideCursor();
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected virtual void Inputs()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Jump"))
            moving = true;
        else
            moving = false;
        forwardDirection = Input.GetAxis("Vertical");
        strafeDirection = Input.GetAxis("Horizontal");
    }

    public enum CameraType
    {
        FirstPerson, ThirdPerson
    }

    protected abstract void CameraMovement();
    protected abstract void Movement();
}
