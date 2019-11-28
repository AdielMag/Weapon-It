using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    int targetHealth;
    public virtual int lifePoints() { return 0; }

    public virtual float speed() { return 0f; }

    public virtual void TakeDamage(int damage)
    {
        targetHealth = lifePoints();
        targetHealth -= damage;

        if (targetHealth >= 0)
            TargetDestroyed();
    }

    public virtual void TargetDestroyed()
    {
        gameObject.SetActive(false);
    }
}
