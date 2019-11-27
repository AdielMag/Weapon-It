using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon :MonoBehaviour
{
    public virtual void Attack()
    {
    }

    public virtual bool canAttack()
    {
        return false;
    }

    public virtual float weaponRange()
    {
        return 0;
    }
}
