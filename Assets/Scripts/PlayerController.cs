using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bouboule;
    public GameObject thePlayer;

    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    private bool jumped;

    public Vector3 moveDirection;
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

    void Teleport()
    {
        Vector3 teleportPosition = new Vector3(bouboule.transform.position.x, bouboule.transform.position.y, 0);
        Debug.Log(teleportPosition);
        Debug.Log(thePlayer.transform.position);
        controller.Move(teleportPosition - transform.position);
        Debug.Log(thePlayer.transform.position + "2");
        moveDirection.y = teleportPosition.y + (Physics.gravity.y * gravityScale);
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire3") && bouboule.GetComponent<Following>().freeStance)
        {
            Debug.Log(bouboule.GetComponent<Following>().freeStance);
            Teleport();
        }

        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, 0);

        if (Input.GetButton("Fire1"))
        {
            HandleRotation();
        }
        else
        {
            lookDirection = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f));
        }

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
    }
}
