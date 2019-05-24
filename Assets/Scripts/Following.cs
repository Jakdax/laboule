using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{
    //Find Player and bouboule
    GameObject thePlayerController;
    public GameObject thePlayer;
    public GameObject bouboule;

    //Handle Bouboule gravity
    Rigidbody rb;

    //Aiming and shooting variables
    public GameObject shootingPoint;
    public float boubouleStrength;
    Vector3 lookP;
    Vector3 f;
    Plane m_Plane;

    //Following and orbiting around Variables
    public float targetDistance;
    public float allowedDistance;
    public float rotationSpeed;

    //Following and orbiting around Variables
    public float followSpeed;
    public RaycastHit Shot;

    //States managing variables
    private bool followingStance = true;
    private bool shootingStance = false;
    public bool freeStance = false;
    public bool isThrowable = true;



    void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        thePlayerController = GameObject.Find("Player");
        m_Plane = new Plane(Vector3.forward, Vector3.zero);
    }

    void OrbitAround()
    {
        //transform.position.y = thePlayer.transform.position.y;
        transform.RotateAround(thePlayer.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        isThrowable = true;
    }

    void FreeBouboule()
    {
        rb.useGravity = true;
    }

    void NormalBoubouleBehavior()
    {
        transform.LookAt(thePlayer.transform);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
        {
            targetDistance = Shot.distance;
            if (targetDistance >= allowedDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position, followSpeed * Time.deltaTime);
            }
            else
            {
                OrbitAround();
            }
        }
    }

    void PlaneRaycast()
    {
        transform.position = shootingPoint.transform.position;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 100.0f;

        if (m_Plane.Raycast(camRay, out enter))
        {
            lookP = camRay.GetPoint(enter);
            thePlayerController.GetComponent<PlayerController>().lookPos = lookP;
            f = (lookP - transform.position).normalized * boubouleStrength;
        }
        Debug.DrawLine(transform.position, f, Color.black);
    }

    void BoubouleBehaviorRoutine()
    {

    }

    void Update()
    {

        if (followingStance == true && shootingStance == false && freeStance == false)
        {
            NormalBoubouleBehavior();

            if (Input.GetButton("Fire1") && isThrowable == true)
            {
                freeStance = false;
                followingStance = false;
                shootingStance = true;
            }
        }
        else if (shootingStance == true && followingStance == false && freeStance == false)
        {
            PlaneRaycast();

            if (Input.GetButtonUp("Fire1"))
            {
                rb.AddForce(f.x, f.y, 0f);
                followingStance = false;
                shootingStance = false;
                freeStance = true;
                isThrowable = false;
            }
        }
        else if (freeStance == true && followingStance == false && shootingStance == false)
        {
            FreeBouboule();

            if (Input.GetButtonDown("Fire2"))
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                freeStance = false;
                followingStance = true;
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                freeStance = false;
                followingStance = true;
            }
        }
    }

    void FixedUpdate()
    {

    }
}



