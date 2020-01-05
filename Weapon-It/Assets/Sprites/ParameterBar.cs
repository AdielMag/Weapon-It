using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBar : MonoBehaviour
{
    public float minValue, maxValue, value;


    // Used to not the player subtract from this value!
    public float gunBaseValue { get; set; }

    public float logMultilpier { get; set; }

    // Used to determine the block count that need to be shown.
    public int upgradeCount { get; set; }

    public Transform blocksParent;
    int maxBlocksCount;
    float valueBlockSize;

    [HideInInspector]
    public StoreManager sManager;

    private void Start()
    {
        blocksParent = GetComponentInChildren<GridLayoutGroup>().transform;

        maxBlocksCount = blocksParent.childCount;

        UpdateBar();
    }

    public void UpdateBar()
    {
        for(int i = 0; i< blocksParent.childCount; i++)
        {
            if (i < upgradeCount)
                blocksParent.GetChild(i).gameObject.SetActive(true);
            else
                blocksParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Add()
    {
        float logValue = Mathf.Log(upgradeCount + 1, logMultilpier) + minValue;

        if (logValue > maxValue)
        {
            Debug.Log("Too high");
            return;
        }

        upgradeCount++;
        value = logValue;

        UpdateBar();
    }

    public void Subtract()
    {
        float logValue = Mathf.Log(upgradeCount - 1, logMultilpier) + minValue;

        if (logValue < gunBaseValue)
        {
            Debug.Log("Trying to get lower that the current gun value - ERROR");
            return;
        }

        upgradeCount--;
        value = logValue;

        UpdateBar();
    }

    public void MaxAmountThatCanBuy()
    {
        // Check the max Amount that can upgrade
    }

}
