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

    // Needed to calculate the target future pos for accurate projectiles
    public Rigidbody rigidbody { get; set; }
    #endregion

    #region Methods
    float currentSpeed;
    public void HandleMovement()
    {
        currentSpeed = StopMoving() ? 0 : movementSpeed;

        rigidbody.velocity = -Vector3.forward * currentSpeed;
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
}
