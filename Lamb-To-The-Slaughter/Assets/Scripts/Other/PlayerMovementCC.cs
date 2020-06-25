using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerMovementCC : MonoBehaviour //Dhan
{

    public static Transform player;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float pitchRange;
    public Camera playerCamera;
    private float RotateY;

    public UnityEngine.Rendering.Universal.ChromaticAberration tPcA;
    public UnityEngine.Rendering.VolumeProfile vp;

    [Header("Movement")]
    public float jumpHeight = 10f;
    public float walkSpeed = 5f;
    public float movementSpeed;
    public float gravityMultiplier = 4.0f;
    private bool moving;

    [SerializeField] float forwardDirection;
    [SerializeField] float strafeDirection;
    float verticalVelocity = 0.0f;

    public static CharacterController cc;
    public Animator anim;
    WeaponSelect ws;
    PlayerHealth ph;

    GameObject tpBomb;
    BombScript bombScript;
    public bool hasTeleported;
    public bool canTeleport = true;

    public bool jumping = true;

    // Start is called before the first frame update
    void Start()
    {
        player = transform;
        LockAndHideCursor();
        if (cc == null)
            cc = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponent<Camera>();

        ws = GetComponent<WeaponSelect>();
        ph = GetComponent<PlayerHealth>();
        movementSpeed = walkSpeed;

        ChromaticAberration cA;

        if (vp.TryGet<ChromaticAberration>(out cA))
        {
            tPcA = cA;
        }

        Color Lcolor = new Color(1f, 0f, 0f);
        tPcA.intensity.Override(0.161f);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("The player can teleport: " + canTeleport);

        if (Speed() > 0)
        {
            anim.SetFloat("Speed", 1f);
        }
        else if (Speed() < 0.1)
        {
            anim.SetFloat("Speed", 0f);
        }

        CameraMovement();
        Inputs();
        Movement();
        GPSmode();

        if (PlayerHealth.overDrive)
        {
            movementSpeed = 20;
            jumpHeight = 15;
        }
        else
        {
            movementSpeed = walkSpeed;
            jumpHeight = 10f;
        }

        if (Input.GetButton("PS"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public float GroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.TransformPoint(cc.center), -Vector3.up, out hit))
        {
            return hit.distance - cc.height;
        }
        return 10f;
    }

    public float Speed()
    {
        return new Vector3(cc.velocity.x, 0, cc.velocity.z).magnitude;
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

        if (Input.GetKey(KeyCode.P))
            UnlockAndShowCursor();


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

        if (Input.GetKey("a") && Input.GetKey("d"))
        {
            direction.x = 0f;
        }
        if (Input.GetKey("w") && Input.GetKey("s"))
        {
            direction.z = 0f;
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

    private void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public bool teleporting = false;

    void GPSmode()
    {
        if (Input.GetButton("GPS"))
        {
            anim.SetBool("GPSmode", true);
            ws.enabled = false;
            if (Input.GetButtonDown("Fire1") && canTeleport)
            {
                anim.SetBool("Teleport", true);
                tPcA.intensity.Override(1f);
                teleporting = true;
            } else
            {
                teleporting = false;
            }
        }
        else if (Input.GetButtonUp("GPS"))
        {
            anim.SetBool("GPSmode", false);
            ws.enabled = true;
        }
    }

    public void TeleportFunction(GameObject bomb)
    {
        if (!teleporting) 
            return;
        anim.SetBool("Teleport", false);
        cc.enabled = false;
        transform.position = bomb.transform.position;
        cc.enabled = true;
        hasTeleported = true;
        Invoke("ChromaticAberrationReset", 1f);
        Destroy(bomb);
    }

    void ChromaticAberrationReset()
    {
        tPcA.intensity.Override(0.161f);
    }
    #endregion
}