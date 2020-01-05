﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUIManager : MonoBehaviour
{
    // Change this if the gun equation changes
    public float MinDamage => 1;
    public float MaxDamage => 55;

    public float MinRange => 60;
    public float MaxRange => 180;

    public float MinFireRate => .9f;
    public float MaxFireRate => 6;

    [HideInInspector]
    public Transform currentWindow;

    [HideInInspector]
    public int currentItemNum;

    public GameObject weaponsWindow;
    public GameObject charactersWindow;

    public GameObject mainStoreUI, upgradesUI;

    public GameObject upgradesButton;
    public GameObject buyOrEquipButton;

    // Upgrades stuff
    [Header("Upgrades Varaibles")]
    public ParameterBar damageBar;
    public ParameterBar fireRateBar;
    public ParameterBar rangeBar;

    [HideInInspector]
    public StoreManager sManager;

    public void Init()
    {
        OpenItemWindow("Character");

        GetCurrentItemNum();
    }

    public void OpenItemWindow(string windowTypeName)
    {
        // Open wanted window - need to check for each item type.
        if (windowTypeName == StoreManager.ItemTypes.Weapon.ToString())
        {
            currentWindow = weaponsWindow.transform;
            weaponsWindow.SetActive(true);
            charactersWindow.SetActive(false);

            upgradesButton.SetActive(true);

            GetCurrentItemNum();
        }
        else if (windowTypeName == StoreManager.ItemTypes.Character.ToString())
        {
            currentWindow = charactersWindow.transform;
            charactersWindow.SetActive(true);
            weaponsWindow.SetActive(false);

            upgradesButton.SetActive(false);

            GetCurrentItemNum();
        }
        else
            Debug.LogWarning("Misspelled the type name! check button string.");

        // Update Current item
        sManager.currentItem =
            currentWindow.transform.GetChild(currentItemNum).GetComponent<StoreItem>();
        UpdateBuyOrEquipButton(sManager.currentItem.bought);
    }

    public void UpdateBuyOrEquipButton(bool equipedd)
    {
        if (equipedd)
        {
            buyOrEquipButton.transform.GetChild(1).gameObject.SetActive(true);
            buyOrEquipButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            buyOrEquipButton.transform.GetChild(0).gameObject.SetActive(true);
            buyOrEquipButton.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OpenUpgrades()
    {
        upgradesUI.SetActive(true);
        mainStoreUI.SetActive(false);

        UpdateUpgradeMenu();
    }

    public void CloseUpgrades()
    {
        upgradesUI.SetActive(false);
        mainStoreUI.SetActive(true);
    }

    void UpdateUpgradeMenu()
    {
        Gun currentGun = sManager.currentItem.GetComponent<Gun>();

        UpdateBar(damageBar, MinDamage, MaxDamage, currentGun.damage);
        UpdateBar(fireRateBar, MinFireRate, MaxFireRate, currentGun.fireRate);
        UpdateBar(rangeBar, MinRange, MaxRange, currentGun.range);
    }

    void UpdateBar(ParameterBar bar,float minValue,float maxValue,float currentValue)
    {
        bar.minValue = minValue;
        bar.maxValue = maxValue;
        bar.value = currentValue;

        bar.UpdateBar();
    }

    // Check the current window for active obejcts
    void GetCurrentItemNum()
    {
        if (currentWindow.childCount == 0)
        {
            Debug.LogWarning("No items in window");
            return;
        }

        bool once = false; // used to check if there is more than 1 active object
        for (int i = 0; i < currentWindow.childCount; i++)
        {
            if (currentWindow.GetChild(i).gameObject.activeSelf)
            {
                if (once)
                {
                    Debug.LogWarning("More than 1 active item - check your window!");
                    return;
                }

                currentItemNum = i;
                once = true;
            }
        }
    }
}