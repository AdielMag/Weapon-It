using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Main

    #region Variables
    public int health;

    // Used to get the current fortress
    public LevelController LevelCon { get; set; }
    #endregion

    #region Methods
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            TargetDestroyed();
    }

    public void DealDamageToBase(int damage)
    {
        LevelCon.fortress.TakeDamage(damage);
    }

    public virtual void TargetDestroyed()
    {
        LevelCon.TargetDestroyed();
        gameObject.SetActive(false);
    }
    #endregion

    #endregion

    #region Movement

    #region Variables
    public float movementSpeed;
    public float stopDistance;
    public bool movingAside;
    public float floorWidth;
    // Needed to calculate the target future pos for accurate projectiles
    public Rigidbody Rigidbody { get; set; }
    #endregion

    #region Methods
    float forwardSpeed, sideSpeed;
    Vector3 sideDirection;
    public void HandleMovement()
    {
        forwardSpeed = StopMoving() ? 0 : movementSpeed;
        sideSpeed = StopMoving() ? 0 : movementSpeed / 2;

        if (movingAside)
            HandleSideMovement();

        Rigidbody.velocity = -Vector3.forward * forwardSpeed + sideDirection * sideSpeed;

    }

    void HandleSideMovement()
    {
        // Moving Left
        if (sideDirection == Vector3.left)
        {
            // Check if reached the left end
            if (transform.position.x < -floorWidth)
                sideDirection = Vector3.right; // Change dir to right
        }
        // Moving Right
        else
        {
            // Check if reached the right end
            if (transform.position.x > floorWidth)
                sideDirection = Vector3.left; // Change dir to left
        }

        Ray ray = new Ray(transform.position, sideDirection);
        // Check collision
        if (Physics.SphereCast(ray, 2, 1))
            sideDirection = -sideDirection;
    }

    float distanceToStopPos;
    bool StopMoving()
    {
        Ray ray = new Ray(transform.position, -Vector3.forward);

        // Check if theres any object withing stoping ditance
        if (Physics.SphereCast(ray, 2, 1))
            return true;

        // Check if reached stoping distance
        else if (transform.position.z - stopDistance < stopDistance)
            return true;

        // Check the distance to the stoping distnce
        distanceToStopPos = transform.position.z - stopDistance;
        if (distanceToStopPos < stopDistance)
            return true;

        return false;
    }
    #endregion

    #endregion

    #region MonoBehaviour Methods
    public void Init()
    {
        if (movingAside)
            sideDirection = Random.Range(0f, 1f) > .5f ?
                Vector3.right : Vector3.left;
    }
    #endregion
}
