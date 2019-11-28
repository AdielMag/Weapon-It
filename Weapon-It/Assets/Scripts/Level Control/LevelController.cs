using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int currentLevel;

    // Array of level gameobejcts that will store the levels prefabs
    public GameObject[] levelsPrefabs;

    // Spawned level parent - used to call them when needed.
    GameObject levelsParent;

    private void Awake()
    {
        levelsParent = new GameObject("Levels Parent");

        // TEMPORARY! NEED TO THIS FROM MANAGER
        Debug.LogWarning("TEMPORARY!");
        SpawnAllLevels();
    }

    public void SpawnAllLevels()
    {
        Vector3 levelStartPos = levelsParent.transform.position = Vector3.forward * 80;

        foreach (GameObject level in levelsPrefabs)
            Instantiate(level, levelsParent.transform).SetActive(false);
    }

    public void StartLevel(int levelNum)
    {
        if(levelNum <= 0)
        {
            Debug.LogWarning("Level num too low - change it!");
            return;
        }
        else if(levelNum > levelsPrefabs.Length)
        {
            Debug.LogWarning("Level num too High - change it!");
            return;
        }

        levelsParent.transform.GetChild(levelNum - 1).gameObject.SetActive(true);
        levelsParent.transform.GetChild(levelNum - 1).GetComponent<Level>().SpawnLevel();
    }

    public void ResetLevel() { }
    public void HideAllTargets() { }
}
