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
        GameObject spawnedObject;
        switch (type)
        {
            case TargetType.Moving:
                spawnedObject = ObjectPooler.instance.SpawnFromPool
                    ("Moving_Target", transform.position, Quaternion.identity);
                spawnedObject.GetComponent<MovingTarget>().axis = axis;
                break;
            default:
                spawnedObject = ObjectPooler.instance.SpawnFromPool
                    ("Regular_Target", transform.position, Quaternion.identity);
                break;
        }

        GameManager.instance.LevelCon.currentLevelTargets.Add(spawnedObject.GetComponent<Target>());
        GameManager.instance.LevelCon.activeLevelObjects++;
    }
}
