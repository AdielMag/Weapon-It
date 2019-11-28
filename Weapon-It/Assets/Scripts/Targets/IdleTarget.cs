using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTarget : Target
{
    int health = 1;
    public override int lifePoints() { return health; }

    float movingSpeed = 3;
    public override float speed() { return movingSpeed; }


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = -Vector3.forward * speed() * 5;
    }
}
