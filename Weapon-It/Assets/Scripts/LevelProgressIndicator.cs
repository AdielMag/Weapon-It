using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressIndicator : MonoBehaviour
{
    Slider slider;

    public float targetValue;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    
    private void Update()
    {
        slider.value = targetValue;
        //    1 - ((levelMaxTime - (Time.time - levelStartTime)) / levelMaxTime);
    }
}
