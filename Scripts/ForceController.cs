using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    public float forceMagnitude = 10f;
    public float torqueMagnitude = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            rb.AddForce(Vector3.forward * forceMagnitude, ForceMode.Impulse);

        if (Input.GetKeyDown(KeyCode.S))
            rb.AddForce(Vector3.back * forceMagnitude, ForceMode.Impulse);

        if (Input.GetKeyDown(KeyCode.A))
            rb.AddTorque(Vector3.up * -torqueMagnitude, ForceMode.Impulse);

        if (Input.GetKeyDown(KeyCode.D))
            rb.AddTorque(Vector3.up * torqueMagnitude, ForceMode.Impulse);
    }
}
