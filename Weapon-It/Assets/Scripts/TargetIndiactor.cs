using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndiactor : MonoBehaviour
{
    public bool onTarget;

    public void SetLocation(Vector3 targetLocation)
    {
        transform.position = targetLocation - (Vector3.forward * 2);
    }
}
