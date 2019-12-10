using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    int targetHealth;
    public virtual int lifePoints() { return 0; }

    public virtual float forwardMovSpeed() { return 0f; }

    // Needed to calculate the target future pos for accurate projectiles
    public virtual Vector2 sidesMoveDir() { return Vector3.zero; }

    public virtual void TakeDamage(int damage)
    {
        targetHealth = lifePoints();
        targetHealth -= damage;

        if (targetHealth >= 0)
            TargetDestroyed();
    }

    public virtual void TargetDestroyed()
    {
        GameManager.instance.LevelCon.TargetDestroyed();
        gameObject.SetActive(false);
    }
}
