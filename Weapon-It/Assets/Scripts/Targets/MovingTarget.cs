﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : Target
{
    public enum Axis { X, Y, Both }
    public Axis axis;

    int health = 1;
    public override int lifePoints() { return health; }

    float movingSpeed = 3;
    public override float speed() { return movingSpeed; }

    // Used to control which direction to move (on x,y axis).
    Vector2 movementDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();


        // Random first dir
        switch (axis)
        {
            case Axis.X:
                movementDirection = Random.Range(0, 2) > 0 ? Vector3.right : -Vector3.right;
                break;
            case Axis.Y:
                movementDirection = Random.Range(0, 2) > 0 ? Vector3.up : -Vector3.up;
                break;
            case Axis.Both:
                movementDirection = 
                    Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.up;
                break;
        }
    }

    private void FixedUpdate()
    {
        // Check if still above the floor
        if (transform.position.x > 10)        // Right
        {
            ChangeDir();
        }
        else if (transform.position.x < -10)   // Left
        {
            ChangeDir();
        }

        // Check if not too high or low
        if (transform.position.y > 10)       // Up
        {
            ChangeDir();
        }
        else if (transform.position.y < 3)   // Down
        {
            ChangeDir();
        }

        rb.velocity = -Vector3.forward * speed() * 5 + (Vector3)movementDirection * speed();
    }

    void ChangeDir()
    {
        // Check for pos to see which direction to choose.
        // If is more right - go left, too high - go down.

        switch (axis)
        {
            case Axis.X:    // If too left - go right and reverse.
                movementDirection = transform.position.x < 0 ? Vector3.right : -Vector3.right;
                break;
            case Axis.Y:    // If too high - go down and reverse.
                movementDirection = transform.position.y > 10 ? -Vector3.up : Vector3.up;
                break;
            case Axis.Both:
                Vector2 xConstraints = transform.position.x < 0 ? new Vector2(0, 1) : new Vector2(-1,0);
                Vector2 yConstraints = transform.position.y > 10 ? new Vector2(-1, 0) : new Vector2(0, 1);
                movementDirection =
                   Random.Range(xConstraints.x, xConstraints.y) * Vector3.right
                   +
                   Random.Range(yConstraints.x, yConstraints.y) * Vector3.up;
                break;
        }
    }
}