using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Level : MonoBehaviour
{
    // Get all spawners

    public TargetSpawner[] spawners;

    /*
    private void Awake()
    {
        GetAllSpawners();
    }
    */

    private void GetAllSpawners()
    {
        spawners = new TargetSpawner[transform.childCount];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).GetComponent<TargetSpawner>();
        }

        Debug.Log("Got them all");
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
