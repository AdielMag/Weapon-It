using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    int currentLevel;

    float levelMaxTime, levelStartTime;

    public Slider levelProgressIndicator;

    [Space]
    // Array of level gameobejcts that will store the levels prefabs
    public GameObject[] levelsPrefabs;

    // Spawned level parent - used to call them when needed.
    GameObject levelsParent;

    // Used to control active targets (Count them, disable them)
    public List<Target> currentLevelTargets = new List<Target>();
    public int activeLevelObjects;

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
        // 1 - (maxTime -(time - startTime)) / maxTime
        levelProgressIndicator.value =
            1 - ((levelMaxTime - (Time.time - levelStartTime)) / levelMaxTime);
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

        activeLevelObjects = 0;

        levelStartTime = Time.time;
        levelMaxTime = levelTime(levelNum);

        levelsParent.transform.GetChild(levelNum - 1).gameObject.SetActive(true);
        levelsParent.transform.GetChild(levelNum - 1).GetComponent<Level>().SpawnLevel();

        currentLevel = levelNum;    // Set current level 
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

    public void LevelFinished()
    {
        HideAllTargets();

        if (currentLevel >= gMan.DataManager.gamePlayData.playerHighestLevel)
        {
            gMan.DataManager.gamePlayData.playerHighestLevel = currentLevel;
            gMan.DataManager.SaveData();


            // PrototypeUI - TEMPORARY
            gMan.uiManager.UpdateLevelsButtons(currentLevel);
        }

        gMan.uiManager.levelCompleted.SetActive(true);

        levelStartTime = -100;
    }

    public void LostLevel()
    {
        HideAllTargets();
        gMan.uiManager.lostIndicator.SetActive(true);
    }

    public void ResetLevel()
    {

    }

    public void HideAllTargets()
    {        
        //Disable all target
        for (int i = 0; i < currentLevelTargets.Count; i++)
        {
            currentLevelTargets[i].gameObject.SetActive(false);
        }
        currentLevelTargets.Clear();
    }

    // Called when a target is destroyed - to check if won the level
    public void TargetDestroyed()
    {
        activeLevelObjects--;

        if (activeLevelObjects < 1)
            LevelFinished();
    }
}
