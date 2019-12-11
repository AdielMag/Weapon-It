using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressIndicator : MonoBehaviour
{
    Slider slider;

    float levelMaxTime, levelStartTime;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    
    private void Update()
    {   
        // Update level progress indicator
        // 1 - (maxTime -(time - startTime)) / maxTime
        slider.value =
            1 - ((levelMaxTime - (Time.time - levelStartTime)) / levelMaxTime);
    }

    public void SetLevelTimes(float startTime, float maxTime)
    {
        levelStartTime = startTime;
        levelMaxTime = maxTime;
    }
}
