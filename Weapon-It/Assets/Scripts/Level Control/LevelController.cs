using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public bool currentlyPlaying;

    public int currentLevel;

    float levelMaxTime, levelStartTime, remainingTimePrecentage;

    public InGameUIManager uIManager;

    CircleSlider timeLeftSlider;

    // Used to control active targets (Count them, disable them)
    public List<Enemy> currentLevelTargets = new List<Enemy>();
    public int activeLevelObjects;

    [Space]
    public Fortress fortress;

    GameManager gMan;

    private void Start()
    {
        gMan = GameManager.instance;

        timeLeftSlider = uIManager.timeLeft;

        currentLevel = gMan.CurrentLevel;
        StartLevel(currentLevel);
    }

    private void Update()
    {
        if (!currentlyPlaying)
            return;

        remainingTimePrecentage =
                1 - ((levelMaxTime - (Time.time - levelStartTime)) / levelMaxTime);

        if (remainingTimePrecentage < 1) // Level time didn't ran out
        {
            timeLeftSlider.value = remainingTimePrecentage;
        }

        else 
            LevelWon();

    }

    public void StartLevel(int levelNum)
    {
        if (levelNum <= 0)
        {
            Debug.LogWarning("Level num too low - change it!");
            return;
        }

        activeLevelObjects = 0;

        levelStartTime = Time.time;
        levelMaxTime = levelTime(levelNum);

        currentLevel = levelNum;    // Set current level 

        currentlyPlaying = true;
    }

    float levelTime(int levelNum)
    {
        // Get the level last obejct
        // Transform lastObject =
        //     levelsParent.transform.GetChild(levelNum - 1).
        //     GetChild(levelsParent.transform.GetChild(levelNum - 1).childCount -1);

        // time = distance / speed

        //  float time =
        //      Vector3.Distance(transform.position, lastObject.position)
        //      / 17;

        // return time;
        return 0;
    }

    public void LevelWon()
    {
        HideAllTargets();

        if (currentLevel + 1 >= gMan.DataManager.gamePlayData.playerHighestLevel)
        {
            gMan.DataManager.gamePlayData.playerHighestLevel = currentLevel + 1;
            gMan.DataManager.SaveData();
        }
        
        uIManager.LevelCompleted();

        levelStartTime = -100;

        currentlyPlaying = false;
    }

    public void LevelLost()
    {
        HideAllTargets();
        uIManager.LevelLost();

        levelStartTime = -100;

        currentlyPlaying = false;
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
            LevelWon();
    }

    public static LevelController instance;
    private void Awake()
    {
        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;
    }
}
