using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : Enemy
{
    private void Start()
    {
        LevelCon = LevelController.instance;
        Rigidbody = GetComponent<Rigidbody>();

        Init();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleAttacking();
    }

}
