using System;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager 
{
    GameManager gMan;

    StoreData storeData;

    StoreItem weapon;
    StoreItem character;
    StoreItem _base;

    public void Init()
    {
        // Get all the needed variables
        gMan = GameManager.instance;

        storeData = gMan.dataManager.storeData;

        weapon = gMan.LCon.pCon.weaponsItemsParent.GetChild(storeData.EquippedWeapon).GetComponent<StoreItem>();
        character = gMan.LCon.pCon.charactersItemsParent.GetChild(storeData.EquippedCharacter).GetComponent<StoreItem>();
        _base = gMan.LCon.fortress.baseParent.GetChild(storeData.EquippedBase).GetComponent<StoreItem>();
    }

    public float UpgradeCost(StoreItem currentItem, int upgradeCount)
    {
        return Mathf.Ceil(currentItem.cost *
                 Mathf.Pow(currentItem.costMultiplier, upgradeCount) - currentItem.cost);
    }

    public int LevelCoinsReward()
    {
        // Update Store data
        storeData = gMan.dataManager.storeData;

        /* 
        Current weapon cost 35%
            Weapon upgrades Costs 20%
                Hightest cost  10%
                Second          7%
                Third           3%

        Current character cost 15%

        Current base cost 20%
            Base upgrades Costs 10%
                Health  10%
         */

        // Items weighted costs
        float weaponWC = weapon.cost * .35f;
        float characterWC = weapon.cost * .15f;
        float baseWC = weapon.cost * .20f;

        // Upgrades weighted costs
        Gun currentGun = weapon.GetComponent<Gun>();

        // Get the costs
        float[] weaponUpgrades = new float[3];
        weaponUpgrades[0] = UpgradeCost(weapon, storeData.weaponsDamageUpgradesCount[currentGun.gunNum - 1] + 1);
        weaponUpgrades[1] = UpgradeCost(weapon, storeData.weaponsFireRateUpgradesCount[currentGun.gunNum - 1] + 1);
        weaponUpgrades[2] = UpgradeCost(weapon, storeData.weaponsRangeUpgradesCount[currentGun.gunNum - 1] + 1);

        // Set calculated values via checking the highest cost ones
        Array.Sort(weaponUpgrades);
        Array.Reverse(weaponUpgrades);

        foreach (float num in weaponUpgrades)
            Debug.Log(num);

        return 0;
    }
}
