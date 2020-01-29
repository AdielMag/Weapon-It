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
    // Needed to calculate the target future pos for accurate projectiles
    public Rigidbody Rigidbody { get; set; }
    #endregion

    #region Methods
    float forwardSpeed, sideSpeed;
    Vector3 sideDirection;
    Collider collider;
    public LayerMask collisionLayerMask;
    public void HandleMovement()
    {
        if (movingAside)
            HandleSideMovement();

        HandleCollisions();

        Rigidbody.velocity = -Vector3.forward * forwardSpeed + sideDirection * sideSpeed;
    }

    void HandleSideMovement()
    {
        // Check collision
        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents, Vector3.right, Quaternion.identity, .2f, collisionLayerMask))
            sideDirection = Vector3.left;
        else if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents, Vector3.left, Quaternion.identity, .2f, collisionLayerMask))
            sideDirection = Vector3.right;
    }

    float distanceToStopPos;
    void HandleCollisions()
    {
        // Check forward collision
        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents,
                -Vector3.forward, Quaternion.identity, .3f, collisionLayerMask))
        {
            forwardSpeed = 0;
        }

        // Check if reached stoping distance
        else if (transform.position.z - stopDistance < stopDistance)
        {
            inAttackRange = true;
            forwardSpeed = 0;
            sideSpeed = 0;
        }
        else
        {
            inAttackRange = false;
            forwardSpeed = movementSpeed;
            sideSpeed = movementSpeed / 2;
        }
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

        Debug.Log(transform.gameObject.name);
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
        collider = GetComponent<Collider>();
        if (movingAside)
            sideDirection = Random.Range(0f, 1f) > .5f ?
                Vector3.right : Vector3.left;
    }
}
