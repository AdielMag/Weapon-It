﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    public virtual GameManager GMan { get; set;}

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

    public void DealDamage(int damage)
    {
        GMan.LevelCon.fortress.TakeDamage(damage);
    }

    public virtual void TargetDestroyed()
    {
        GMan.LevelCon.TargetDestroyed();
        gameObject.SetActive(false);
    }
}