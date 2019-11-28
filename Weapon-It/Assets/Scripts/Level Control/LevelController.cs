using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int currentLevel;
    float levelMaxTime, levelStartTime;

    public Slider levelProgressIndicator;

    [Space]
    // Array of level gameobejcts that will store the levels prefabs
    public GameObject[] levelsPrefabs;

    // Spawned level parent - used to call them when needed.
    GameObject levelsParent;

    GameManager gMan;

    private void Awake()
    {
        levelsParent = new GameObject("Levels Parent");
    }

    private void Start()
    {
        gMan = GameManager.instance;
    }

    private void Update()
    {
        // Update level progress indicator
        // 1 - (maxTime -(time - startTime)) / maxTime.
        if (gMan.levelInProgress)
            levelProgressIndicator.value =
               1 - ((levelMaxTime - (Time.time - levelStartTime)) /levelMaxTime) ;
    }

    public void SpawnAllLevels()
    {
        Vector3 levelStartPos = levelsParent.transform.position = Vector3.forward * 150;

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

        levelStartTime = Time.time;
        levelMaxTime = levelTime(levelNum);

        levelsParent.transform.GetChild(levelNum - 1).gameObject.SetActive(true);
        levelsParent.transform.GetChild(levelNum - 1).GetComponent<Level>().SpawnLevel();

        gMan.levelInProgress = true;
    }

    float levelTime(int levelNum)
    {
        // Get the level last obejct
        Transform lastObject =
            levelsParent.transform.GetChild(levelNum - 1).
            GetChild(levelsParent.transform.GetChild(levelNum - 1).childCount -1);

        // time = distance / speed

        float time =
            Vector3.Distance(transform.position, lastObject.position)
            /
            13
            ;

        return time;
    }

    public void LostLevel()
    { }

    public void ResetLevel() { }
    public void HideAllTargets() { }
}
