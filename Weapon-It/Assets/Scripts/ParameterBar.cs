using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBar : MonoBehaviour
{
    public float minValue, maxValue, value;

    // Used to determine the block count that need to be shown.
    public int upgradeCount;

    public RectTransform blockIndicator;
    float blockSize;

    public Text valueIndicator;
    public int decimals = 1;

    // Used to not the player subtract from this value!
    public float baseValue { get; set; }

    // Get this value from the current Gun
    public float logMultilpier { get; set; }

    [HideInInspector]
    public StoreManager sManager;

    private void Awake()
    {
        blockSize = blockIndicator.rect.width / 15;
    }

    private void Start()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        // Determine if update the current upgrade indicator or just the cost one
        for (int i = 0; i < upgradeCount +1; i++)
        {
            float baseWidth = blockIndicator.rect.position.x + blockSize * i;

            float logValue = Mathf.Log(i, logMultilpier) + minValue;
            if (logValue > baseValue)
                blockIndicator.GetChild(0).GetComponent<RectTransform>()
                    .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseWidth);
            else
            {
                blockIndicator.GetChild(0).GetComponent<RectTransform>()
                    .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseWidth);
                blockIndicator.GetChild(1).GetComponent<RectTransform>()
                    .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseWidth);
            }
        }

        float targetValue = (float)System.Math.Round((double)value,decimals);
        valueIndicator.text = targetValue.ToString();

        sManager.uiManager.UpdateUpgradeCostAndAppearance();
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

        if (logValue < baseValue)
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

        int startingUpgradeCount = upgradeCount;

        for(int i = startingUpgradeCount; i < 15; i++)
        {
            upgradeCount++;
            if(sManager.coins < sManager.CalculateWeaponUpgradeCosts())
            {
                upgradeCount--;
                break;
            }
        }

        value = Mathf.Log(upgradeCount, logMultilpier) + minValue;

        UpdateBar();
    }
}
