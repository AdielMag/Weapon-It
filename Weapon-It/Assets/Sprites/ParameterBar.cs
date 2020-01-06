using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBar : MonoBehaviour
{
    public float minValue, maxValue, value;

    // Used to not the player subtract from this value!
    public float gunBaseValue { get; set; }

    // Get this value from the current Gun
    public float logMultilpier { get; set; }

    // Used to determine the block count that need to be shown.
    public int upgradeCount;

    public Transform blocksParent;
    Color origBlockColor;
    int maxBlocksCount;
    float valueBlockSize;

    public Text valueIndicator;

    [HideInInspector]
    public StoreManager sManager;

    private void Awake()
    {
        blocksParent = GetComponentInChildren<GridLayoutGroup>().transform;

        origBlockColor = blocksParent.GetChild(0).GetComponent<Image>().color;

        maxBlocksCount = blocksParent.childCount;
    }

    private void Start()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        for(int i = 0; i< blocksParent.childCount; i++)
        {
            if (i < upgradeCount)
            {
                blocksParent.GetChild(i).gameObject.SetActive(true);

                // Determine its color
                float logValue = Mathf.Log(i +1, logMultilpier) + minValue;
                if (logValue > gunBaseValue)
                {
                    Color newColor = origBlockColor;
                    newColor.g *= .6f;
                    blocksParent.GetChild(i).gameObject.GetComponent<Image>().color = newColor;
                    blocksParent.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().color = newColor;
                }
                else
                {
                    blocksParent.GetChild(i).gameObject.GetComponent<Image>().color = origBlockColor;
                    blocksParent.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().color = origBlockColor;
                }
            }
            else
                blocksParent.GetChild(i).gameObject.SetActive(false);
        }

        float targetValue = (float)System.Math.Round((double)value, 1);
        valueIndicator.text = targetValue.ToString();

        sManager.uiManager.UpdateUpgradeCosts();
    }

    public void Add()
    {
        float logValue = Mathf.Log(upgradeCount + 1, logMultilpier) + minValue;

        if (logValue > maxValue)
        {
            Debug.Log("Too high");
        }
        else
        {
            upgradeCount++;
            value = logValue;
        }

        UpdateBar();
    }

    public void Subtract()
    {
        float logValue = Mathf.Log(upgradeCount - 1, logMultilpier) + minValue;

        if (logValue < gunBaseValue)
        {
            Debug.Log("Trying to get lower that the current gun value - ERROR");
        }
        else
        {
            upgradeCount--;
            value = logValue;
        }

        UpdateBar();
    }

    public void MaxAmountThatCanBuy()
    {
        // Check the max Amount that can upgrade
    }
}
