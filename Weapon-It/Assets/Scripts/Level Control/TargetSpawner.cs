using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    // Spawner obj type
    public enum TargetType { Regular , Moving }
    public TargetType type;
    [Header("If Moving")]
    public MovingTarget.Axis axis;

    public void SpawnObj()
    {
        switch (type)
        {
            case TargetType.Regular:
                ObjectPooler.instance.SpawnFromPool
                    ("Regular_Target", transform.position, Quaternion.identity);
                break;
            case TargetType.Moving:
                ObjectPooler.instance.SpawnFromPool
                    ("Moving_Target", transform.position, Quaternion.identity)
                    .GetComponent<MovingTarget>().axis = axis;
                break;
        }
    }
}
