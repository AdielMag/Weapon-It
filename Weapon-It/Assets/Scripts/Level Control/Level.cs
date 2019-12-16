﻿using System.Collections;
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
                SetSpawner(i);

            else if (transform.GetChild(i).GetComponent<MovingTarget>())
                SetSpawner(i, transform.GetChild(i).GetComponent<MovingTarget>().axis);
        }

        Debug.Log("Got them all");
    }

    void SetSpawner(int spawnerNum)
    {
        Transform newObj = transform.GetChild(spawnerNum);

        if (newObj.GetComponent<TargetSpawner>())
            newObj.gameObject.GetComponent<TargetSpawner>().type =
                TargetSpawner.TargetType.Regular;
        else
            newObj.gameObject.AddComponent<TargetSpawner>().type =
                TargetSpawner.TargetType.Regular;

        spawners[spawnerNum] = newObj.GetComponent<TargetSpawner>();
    }
    void SetSpawner(int spawnerNum, MovingTarget.Axis axis)
    {
        Transform newObj = transform.GetChild(spawnerNum);

        if (newObj.GetComponent<TargetSpawner>())
            newObj.gameObject.GetComponent<TargetSpawner>().type =
                TargetSpawner.TargetType.Moving;
        else
            newObj.gameObject.AddComponent<TargetSpawner>().type =
                TargetSpawner.TargetType.Moving;

        newObj.GetComponent<TargetSpawner>().axis = axis;

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
