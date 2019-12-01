using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon :MonoBehaviour
{
    public virtual void Attack() { }
    public virtual void Attack(Vector3 projectileForward) { }

    public PlayerController pCon;
    public virtual PlayerController PlayerCon() => pCon;

    public virtual bool CanAttack => false;

    public virtual float WeaponRange => 0;
}
