using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementController
{

    public static Camera playerCamera;

    protected override void Start()
    {
        base.Start();
        playerCamera = camera;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CameraMovement()
    {
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        rotatePitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotatePitch = Mathf.Clamp(rotatePitch, -pitchRange, pitchRange);
        camera.transform.localRotation = Quaternion.Euler(rotatePitch, 0, 0);
    }


    protected override void Movement()
    {
        Vector3 direction = DirectionCurrentlyMoving();

        //vertical velocity calculations
        if (cc.isGrounded)
            verticalVelocity = 0;
        verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        if (Input.GetButtonDown("Jump") & cc.isGrounded)
            verticalVelocity = jumpHeight;
        
        if (!moving)
        {
            direction = Vector3.zero;
        }

        direction.y = verticalVelocity;
        direction = transform.rotation * direction;
        cc.Move(direction * Time.deltaTime);
    }
    private Vector3 DirectionCurrentlyMoving()
    {
        Vector3 direction = new Vector3(strafeDirection, 0, forwardDirection);
        direction = direction.normalized * movementSpeed;
        return direction;
    }

}
