using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Level : MonoBehaviour
{
    // Get all spawners

    public TargetSpawner[] spawners;

    public void GetAllSpawners()
    {
        spawners = new TargetSpawner[transform.childCount];

        for (int i = 0; i < spawners.Length; i++)
        {
            if (transform.GetChild(i).GetComponent<IdleTarget>())
                SetSpawner(i, TargetSpawner.TargetType.Regular);

            else if (transform.GetChild(i).GetComponent<MovingTarget>())
                SetSpawner(i, TargetSpawner.TargetType.Moving);
        }

        Debug.Log("Got them all");
    }

    void SetSpawner(int spawnerNum, TargetSpawner.TargetType type)
    {
        GameObject newObj = new GameObject("Target Spawner " + spawnerNum);
        newObj.transform.SetParent(transform);
        newObj.transform.position = transform.GetChild(spawnerNum).position;
        newObj.AddComponent<TargetSpawner>().type = type;

        spawners[spawnerNum] = newObj.GetComponent<TargetSpawner>();
    }

    public void SpawnLevel()
    {
        // Iterate Through all spawners and spawn objects.
        foreach (TargetSpawner spawner in spawners)
            spawner.SpawnObj();

        // When finish Spawning - Disable Game object
        gameObject.SetActive(false);
    }
}
