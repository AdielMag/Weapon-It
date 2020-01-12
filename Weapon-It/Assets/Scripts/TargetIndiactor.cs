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

    Collider targetColldier;
    public void SetLocation(Transform targetTransform)
    {
        targetColldier = targetTransform.GetComponent<Collider>();

        float zOffset = targetColldier.bounds.extents.z;
        transform.position = targetColldier.bounds.center - (Vector3.forward * zOffset);
    }
}
