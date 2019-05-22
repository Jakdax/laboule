using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    private bool jumped;

    private Vector3 moveDirection;
    private Vector3 lookDirection;
    public float gravityScale;
    private float lastDirection;

    public Vector3 lookPos;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void HandleRotation()
    {
        Vector3 directionToLook = lookPos - transform.position;
        directionToLook.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15);
    }

    private void Update()
    {

        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y,0 );

        lookDirection = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f));

        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (controller.isGrounded)
            {
                moveDirection.y = jumpForce;
                jumped = true;
            }
            else if (jumped)
            {
                moveDirection.y = jumpForce;
                jumped = false;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
        controller.Move(moveDirection * Time.deltaTime);
        HandleRotation();
    }
}
