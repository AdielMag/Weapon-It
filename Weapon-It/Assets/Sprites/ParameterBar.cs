using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBar : MonoBehaviour
{
    public  float minValue, maxValue,value;

    [Range(0,1)]
    public float precentage;

    public Transform blocksParent;
    int maxBlocksCount;
    float precentageBlockSize;

    private void Start()
    {
        blocksParent = GetComponentInChildren<GridLayoutGroup>().transform;

        maxBlocksCount = blocksParent.childCount;

        precentageBlockSize = 1f / maxBlocksCount;

        UpdateBar();
    }

    public void UpdateBar()
    {
        UpdatePrecentageViaValue();

        for(int i = 0; i< maxBlocksCount; i++)
        {
            if (i <= precentage * maxBlocksCount)
                blocksParent.GetChild(i).gameObject.SetActive(true);
            else
                blocksParent.GetChild(i).gameObject.SetActive(false);
        }

        UpdatePrecentageViaValue();
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
        // Dont let it add more the the max value

        precentage += precentageBlockSize;
        precentage = Mathf.Clamp(precentage, 0, 1);

        UpdateBar();
    }

    public void Subtract()
    {
        // Dont let it subtract less than the current value
        precentage -= precentageBlockSize;
        precentage = Mathf.Clamp(precentage, 0, 1);

        UpdateBar();
    }

    public void MaxAmountThatCanBuy()
    {
        // Check the max Amount that can upgrade
    }
}
