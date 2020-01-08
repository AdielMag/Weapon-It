using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    // The gun location as a child of the weapons parent (Used to get the correct data from data manager)
    public int baseNum;

    [Header("In Game Parameters")]
    public int health = 1;

    [HideInInspector]
    public int healthUpgradeCount;

    GameManager gMan;

    [Space]
    public float healthLogMultiplier;

    public float MinHealth { get; private set; }
    public float MaxHealth { get; private set; }


    private void Start()
    {
        gMan = GameManager.instance;

        baseNum--;

        CalculateBaseParameters();

        GetComponent<Upgrades>().UpdateUpgradesAppearance(this);
    }

    void CalculateBaseParameters()
    {
        MinHealth = health;

        MaxHealth = Mathf.Log(15, healthLogMultiplier) + MinHealth;


        UpdateParameters();
    }

    public void UpdateParameters()
    {
        healthUpgradeCount = gMan.DataManager.storeData.baseHealthUpgradesCount[baseNum];

        health = Mathf.RoundToInt(Mathf.Log(healthUpgradeCount, healthLogMultiplier) + MinHealth);
    }
}
