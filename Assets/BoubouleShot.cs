using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoubouleShot : MonoBehaviour
{
    public GameObject shootingPoint;

    private Rigidbody rb;

    public float boubouleStrength;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            transform.position = shootingPoint.transform.position;

            rb.AddForce(Input.mousePosition * boubouleStrength);
        }
    }
}
