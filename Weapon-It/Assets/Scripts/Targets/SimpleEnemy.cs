using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    public float stopDistance = 2;
    float distanceToStopPos;

    int health = 1;
    public override int lifePoints() { return health; }

    float movingSpeed = 4;
    public override float forwardMovSpeed() { return movingSpeed; }

    #region Set gMan
    public override LevelController LevelCon => targetLevelCon;

    LevelController targetLevelCon;
    #endregion

    Rigidbody rb;

    private void Start()
    {
        targetLevelCon = LevelController.instance;

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Check if theres any object withing stoping ditance
        Ray ray = new Ray(transform.position, -Vector3.forward);
        if (Physics.SphereCast(ray,2,1))
        {
            rb.velocity = Vector3.zero;
            return;
        }
        // Check if reached stoping distance
        else if (transform.position.z - stopDistance < stopDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Debug.DrawRay(transform.position, -Vector3.forward * 3, Color.yellow);

        // Check the distance to the stoping distnce
        distanceToStopPos = transform.position.z - stopDistance;
        if (distanceToStopPos < stopDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = -Vector3.forward * forwardMovSpeed() * 5;
    }
}