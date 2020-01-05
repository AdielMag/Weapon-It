using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBar : MonoBehaviour
{
    public  float minValue, maxValue,value;

    //[HideInInspector]
    public float gunBaseValue;

    [Range(0,1)]
    public float precentage;

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
        UpdatePrecentageViaValue();

        for (int i = 0; i < maxBlocksCount; i++)
        {
            if (i <= precentage * maxBlocksCount)
                blocksParent.GetChild(i).gameObject.SetActive(true);
            else
                blocksParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    void UpdateValueViaPrecentage()
    {
        value = minValue + ((maxValue - minValue) * precentage);
        value = Mathf.Clamp(value, minValue, maxValue);
    }

    void UpdatePrecentageViaValue()
    {
        precentage = (value - minValue) / (maxValue - minValue);
    }

    public void Add()
    {
        valueBlockSize = (maxValue - minValue) / maxBlocksCount;

        if (value + valueBlockSize > maxValue)
        {
            Debug.Log("Too high");
            return;
        }

        value += valueBlockSize;

        UpdateBar();
    }

    public void Subtract()
    {
        valueBlockSize = (maxValue - minValue) / maxBlocksCount;

        if (value - valueBlockSize < gunBaseValue)
        {
            Debug.Log("Trying to get lower that the current gun value - ERROR");
            return;
        }

        value -= valueBlockSize;

        UpdateBar();
    }

    public void MaxAmountThatCanBuy()
    {
        // Check the max Amount that can upgrade
    }
}
