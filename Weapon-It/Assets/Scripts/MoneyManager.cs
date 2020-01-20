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

    public int LevelCoinsReward(int levelNum)
    {
        int rewardBase = 4;
        float rewradMultiplier = 1.15f;

        Debug.Log(Mathf.CeilToInt(rewardBase * Mathf.Pow(rewradMultiplier, levelNum) - rewardBase));
        return Mathf.CeilToInt(rewardBase * Mathf.Pow(rewradMultiplier, levelNum) - rewardBase);

        // לא פעיל
        // ||
        // \/
        #region ממוצע משוקלל
        // Update Store data
        storeData = gMan.dataManager.storeData;

        /* 
        Current weapon cost 35%
            Weapon upgrades Costs 20%
                Hightest cost  10%
                Second          7%
                Third           3%
                           Total = 55%

        Current base cost 20%
            Base upgrades Costs 10%
                Health  10%
                           Total = 30%

        Current character cost 15%

                           Total = 100%

         */

        #region Items
        // Items weighted costs
        float weaponWC = weapon.cost * .35f;
        float characterWC = weapon.cost * .15f;
        float baseWC = weapon.cost * .20f;
        #endregion

        #region Weapon Upgrades
        // Weapon upgrades weighted costs
        Gun currentGun = weapon.GetComponent<Gun>();

        // Get the costs
        float[] weaponUpgradesWC = new float[3];
        weaponUpgradesWC[0] = UpgradeCost(weapon, storeData.weaponsDamageUpgradesCount[currentGun.gunNum - 1] + 1);
        weaponUpgradesWC[1] = UpgradeCost(weapon, storeData.weaponsFireRateUpgradesCount[currentGun.gunNum - 1] + 1);
        weaponUpgradesWC[2] = UpgradeCost(weapon, storeData.weaponsRangeUpgradesCount[currentGun.gunNum - 1] + 1);

        // Sort the values by highest cost first
        Array.Sort(weaponUpgradesWC);
        Array.Reverse(weaponUpgradesWC);

        weaponUpgradesWC[0] *= .1f;
        weaponUpgradesWC[1] *= .07f;
        weaponUpgradesWC[2] *= .03f;
        #endregion

        #region Base Upgrades
        Base currentBase = _base.GetComponent<Base>();

        float baseHealthUpgradeWC = UpgradeCost(_base, storeData.baseHealthUpgradesCount[currentBase.baseNum - 1] + 1);

        baseHealthUpgradeWC *= .1f;
        #endregion

        Debug.Log(Mathf.RoundToInt(weaponWC + characterWC + baseWC + weaponUpgradesWC[0] + weaponUpgradesWC[1] + weaponUpgradesWC[2] + baseHealthUpgradeWC));
        return Mathf.RoundToInt(weaponWC + characterWC + baseWC + weaponUpgradesWC[0] + weaponUpgradesWC[1] + weaponUpgradesWC[2] + baseHealthUpgradeWC);
#endregion
    }
}
