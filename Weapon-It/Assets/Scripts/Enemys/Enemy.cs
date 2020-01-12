using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Main

    #region Variables
    [Header("Main Variables")]
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
    [Header("Movement Variables")]
    public bool movingAside;
    public float movementSpeed;
    public float stopDistance;
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

        // Check if Going to collide with object
        if (Physics.SphereCast(ray, 2, 1))
            return true;

        // Check if reached stoping distance
        else if (transform.position.z - stopDistance < stopDistance)
        {
            inAttackRange = true;
            return true;
        }        

        inAttackRange = false;
        return false;
    }
    #endregion

    #endregion

    #region Attacking

    #region Variables
    [Header("Attack Variables")]
    public int damage;
    public float attackPerMin;
    public bool ranged;
    #endregion

    #region Methods
    public void HandleAttacking()
    {
        if (!CanAttack())
            return;

        if (ranged)
            return;

        Attack();
    }

    void Attack()
    {
        // If not attacking via range
        if (ranged)
            return;

        LevelCon.fortress.TakeDamage(damage);
        attackTime = Time.time;

        Debug.Log(LevelCon.fortress.currentBase.health);
    }

    float attackTime;
    bool inAttackRange;
    bool CanAttack()
    {
        if (inAttackRange && attackTime + (60 / attackPerMin) < Time.time)
            return true;
        else
            return false;
    }
    #endregion

    #endregion

    public void Init()
    {
        if (movingAside)
            sideDirection = Random.Range(0f, 1f) > .5f ?
                Vector3.right : Vector3.left;
    }
}
