using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    public enum Axis { X, Y, Both }
    public Axis axis;

    public float stopDistance = 2;
    float distanceToStopPos;

    int health = 1;
    public override int lifePoints() { return health; }

    float zMovingSpeed = 4;
    public override float forwardMovSpeed() { return zMovingSpeed; }

    // Used to control which direction to move (on x,y axis).
    Vector2 movementDirection;
    public override Vector2 sidesMoveDir() { return movementDirection; }

    #region Set gMan
    public override LevelController LevelCon => targetLevelCon;

    LevelController targetLevelCon;
    #endregion

    Rigidbody rb;

    private void Start()
    {
        targetLevelCon = LevelController.instance;
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
        // Check if theres any object withing stoping ditance
        Ray ray = new Ray(transform.position, -Vector3.forward);
        if (Physics.SphereCast(ray, 2, 1)
            ||
            // else - Check if reached stoping distance
            transform.position.z - stopDistance < stopDistance) 
        {
            StopMoving();
            // Check if can attack
            if (CanAttack())
            { }
            return;
        }

        Debug.DrawRay(transform.position, -Vector3.forward * 3, Color.yellow);

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

        rb.velocity = -Vector3.forward * forwardMovSpeed() * 3 + (Vector3)movementDirection * forwardMovSpeed();
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

    void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    bool CanAttack()
    {
        return true;
    }

    void Attack()
    { }
}
