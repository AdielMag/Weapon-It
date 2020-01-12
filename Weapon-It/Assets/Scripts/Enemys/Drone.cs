using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    private void Start()
    {
        LevelCon = LevelController.instance;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

}
