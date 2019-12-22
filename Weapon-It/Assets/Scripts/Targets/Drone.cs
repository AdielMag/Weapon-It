using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Vector3 rotationsMultiplier = Vector3.one;

    public float rotationLerpMultiplier = 8;

    public bool reverseRotation;


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        Rotation();
    }

    Quaternion targetRotation;
    void Rotation()
    {
        if (reverseRotation)
        {
            targetRotation =
                Quaternion.Euler(Vector3.zero + new Vector3(
                 rb.velocity.y * rotationsMultiplier.y,
                 -rb.velocity.x * rotationsMultiplier.x,
                 rb.velocity.x * rotationsMultiplier.z));
        }
        else
        {
            targetRotation =
                Quaternion.Euler(Vector3.zero + new Vector3(
                    rb.velocity.z + -rb.velocity.y * rotationsMultiplier.y,
                    -rb.velocity.x * rotationsMultiplier.x,
                    -rb.velocity.x * rotationsMultiplier.z));
        }

        transform.rotation = 
            Quaternion.Lerp(transform.rotation,
            targetRotation,
            Time.deltaTime * rotationLerpMultiplier);
    }
}
