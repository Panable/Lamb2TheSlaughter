using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCCAcceleration : MonoBehaviour //NEEDS COMMENTING
{

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float pitchRange;
    public Camera playerCamera;
    private float RotateY;


    [Header("Movement")]
    public float jumpHeight = 10f;
    public float walkSpeed = 5f;
    public float movementSpeed;
    public float gravityMultiplier = 4.0f;
    private bool moving;

    float forwardDirection;
    float strafeDirection;
    float verticalVelocity = 0.0f;

    public CharacterController cc;

    public bool jumping = true;

    // Start is called before the first frame update
    void Start()
    {
        LockAndHideCursor();
        if (cc == null)
            cc = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponent<Camera>();

        movementSpeed = walkSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        Inputs();
        Movement();
    }

    public void Inputs()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Jump"))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }


        forwardDirection = Input.GetAxis("Vertical");
        strafeDirection = Input.GetAxis("Horizontal");
    }

    void Movement()
    {
        Vector3 direction = new Vector3(strafeDirection, 0, forwardDirection);
        direction = direction.normalized * movementSpeed;
        if (cc.isGrounded)
        {
            verticalVelocity = 0;
        }
        verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        if (Input.GetButtonDown("Jump") & cc.isGrounded & jumping)
        {
            Jump();
        }
        if (!moving)
        {
            direction = Vector3.zero;
        }

        direction.y = verticalVelocity;
        direction = transform.rotation * direction;
        cc.Move(direction * Time.deltaTime);
    }
    public void Jump()
    {

        verticalVelocity = jumpHeight;
    }

    #region Camera Movement Code
    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CameraMovement()
    {
        RotatePlayerOnMouseX();
        RotateCameraOnMouseY();
    }


    private float GetMouseX()
    {
        return Input.GetAxis("Mouse X") * mouseSensitivity;
    }
    private float GetMouseY()
    {
        return Input.GetAxis("Mouse Y") * mouseSensitivity;
    }

    private void RotatePlayerOnMouseX()
    {
        transform.Rotate(0, GetMouseX(), 0);
    }
    private void RotateCameraOnMouseY()
    {
        //rotates camera based on mouse y
        RotateY += -GetMouseY();
        //clamps mouse y
        RotateY = Mathf.Clamp(RotateY, -pitchRange, pitchRange);
        //applies rotation to camera
        playerCamera.transform.localRotation = Quaternion.Euler(RotateY, 0, 0);
    }

    #endregion

}
