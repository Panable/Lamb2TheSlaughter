using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class PlayerRBController : MonoBehaviour //Dhan
{
    [SerializeField] public LayerMask groundLayer;

    public RaycastHit groundHit;
    [SerializeField] private float groundDistanceForGrounded;
    [SerializeField] private float maxGroundDistance = 10.00f;
    [SerializeField] private float groundDistance = 0.00f;
    [SerializeField] private float groundPosition;
    public bool isGrounded = false;

    //camera movement variables
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float pitchRange;
    private float RotateY;

    //movement variables
    private Vector2 input;
    private Vector2 additiveInput = Vector2.zero;
    [Range(0, 1)]
    public float damping;
    [Range(0, 1)]
    public float dampingWhenChangingDirection;
    [Range(0, 1)]
    public float dampingWhenStopping;
    public bool changingDirections;
    public float speed;
    public float movementSpeed;
    public float maxSpeed;

    public Camera playerCamera;
    public CapsuleCollider capsuleCollider;
    public Rigidbody rb;

    void Start()
    {
        LockAndHideCursor();
    }


    void Update()
    {
        CameraMovement();
        CheckGroundDistance();
        GetInputs();
    }

    private void FixedUpdate()
    {
        ApplyDamping();
    }

    public void Movement()
    {
        float verticalSpeed = Input.GetAxis("Vertical");
        float horizontalSpeed = Input.GetAxis("Horizontal");
        if (verticalSpeed != 0 || horizontalSpeed != 0)
        {
            speed += movementSpeed * Time.fixedDeltaTime;
            speed = Mathf.Clamp(speed, 0f, movementSpeed);
        }
        else
        {
            speed -= movementSpeed * Time.fixedDeltaTime;
            speed = Mathf.Clamp(speed, 0f, Mathf.Infinity);
        }

        Vector3 inputVector = new Vector3(-verticalSpeed, 0, horizontalSpeed).normalized;

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

    private void GetInputs()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
    }

    private void ApplyDamping()
    {
        if (additiveInput.magnitude <= 0.05f)
            additiveInput = Vector2.zero;

        additiveInput += input * movementSpeed;
        speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);
        if ((!Input.GetButton("Horizontal") && !Input.GetButton("Vertical")) ||
            (Input.GetKey("left") && Input.GetKey("right")) ||
            (Input.GetKey("up") && Input.GetKey("down")) || (Input.GetKey("a") && Input.GetKey("d")) ||
            (Input.GetKey("w") && Input.GetKey("s")))
        {
            additiveInput *= Mathf.Pow(1f - dampingWhenStopping, Time.deltaTime * 10f);
        }
        else
        {

            if ((Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(additiveInput.x) || Mathf.Sign(Input.GetAxisRaw("Vertical")) != Mathf.Sign(additiveInput.y)) && speed <= 0.1f)
            {
                additiveInput *= Mathf.Pow(1f - dampingWhenChangingDirection, Time.deltaTime * 10f);
            }
            else
            {
                additiveInput *= Mathf.Pow(1f - damping, Time.deltaTime * 10f);
            }

            additiveInput = ExtensionMethods.Round(additiveInput);
            additiveInput = ExtensionMethods.Clamp(additiveInput, -maxSpeed, +maxSpeed);

        }


        Vector3 movement = new Vector3(additiveInput.x, 0, additiveInput.y);


        movement = transform.rotation * movement;
        rb.velocity = movement;
    }


    public void IsChangingDirections()
    {

        speed = Mathf.Clamp(speed, 0, 1f);

        if (speed < 1f)
        {
        }

    }


    void CheckGroundDistance()
    {
        float sphereCastDistance = maxGroundDistance;
        float rayCastDistance = maxGroundDistance;

        float radiusForSphereCast = capsuleCollider.radius * 0.9f;

        Vector3 positionForRayCast = transform.position + new Vector3(0, capsuleCollider.height / 2, 0);
        Ray groundDistanceForRayCast = new Ray(positionForRayCast, Vector3.down);

        //the base of the player + a little bit further up so it doesn't 
        Vector3 positionForSphereCast = transform.position + (Vector3.up * capsuleCollider.radius);
        Ray groundDistanceSpherecast = new Ray(positionForSphereCast, Vector3.down);

        if (Physics.Raycast(groundDistanceForRayCast, out groundHit, capsuleCollider.height / 2 + 2f, groundLayer))
        {
            rayCastDistance = transform.position.y - groundHit.point.y;
        }

        if (Physics.SphereCast(groundDistanceSpherecast, radiusForSphereCast, out groundHit, maxGroundDistance))
        {
            sphereCastDistance = groundHit.distance - capsuleCollider.radius * 0.1f;
            if (rayCastDistance > sphereCastDistance)
            {
                groundDistance = (float)System.Math.Round(sphereCastDistance - 1, 2);
            }
            else
            {
                groundDistance = (float)System.Math.Round(rayCastDistance - 1, 2);
            }

        }

        if (groundDistance <= groundDistanceForGrounded)
        {
            isGrounded = true;
        }

    }
}
