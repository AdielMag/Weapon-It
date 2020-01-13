using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public int coins;

    // Declare all your item types.
    public enum ItemTypes { Weapon, Character, Base }

    [HideInInspector]
    public StoreItem currentItem;

    GameManager gMan;
    public StoreUIManager uiManager;

    private void Start()
    {
        gMan = GameManager.instance;

        gMan.SMan = this;

        LoadStoreData();

        uiManager.sManager = this;
        uiManager.Init();
    }

    // Get data from json file and set bought and equiped items
    void LoadStoreData()
    {
        gMan.DataManager.LoadData();

        /* 
         * ! Need to have data for each item type - check 'jsonDataManager.StoreData'
         * ! Need to do this for each item type
         */

        coins = gMan.DataManager.storeData.Coins;

        // Check each item in 'Weapons window'
        for (int i = 0; i < uiManager.weaponsWindow.transform.childCount; i++)
        {
            // Check each item in data manager
            for (int y = 0; y < gMan.DataManager.storeData.WeaponsBought.Length; y++)
            {
                if (i == gMan.DataManager.storeData.WeaponsBought[y])
                    uiManager.weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().bought = true;
            }

            // Check if the weapon equipped in data manager equals this one
            if (i == gMan.DataManager.storeData.EquippedWeapon)
                uiManager.weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().equipped = true;

            // Update gun upgrades parameters
            Gun gun = uiManager.weaponsWindow.transform.GetChild(i).GetComponent<Gun>();
            StoreData sData = gMan.DataManager.storeData;

            gun.damageUpgradeCount =    sData.weaponsDamageUpgradesCount[i];
            gun.rangeUpgradeCount =     sData.weaponsRangeUpgradesCount[i];
            gun.fireRateUpgradeCount =  sData.weaponsFireRateUpgradesCount[i];
        }
        // Check each item in 'Character window'
        for (int i = 0; i < uiManager.charactersWindow.transform.childCount; i++) 
        {
            // Check each item in data manager
            for (int y = 0; y < gMan.DataManager.storeData.CharactersBought.Length; y++)
            {
                // If the data manager num matches this item - it means its bought
                if (i == gMan.DataManager.storeData.CharactersBought[y])
                    uiManager.charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().bought = true;
            }

            // Check if the character equipped in data manager equals this one
            if (i == gMan.DataManager.storeData.EquippedCharacter)
                uiManager.charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().equipped = true;
        }
        // Check each item in 'Character window'
        for (int i = 0; i < uiManager.basesWindow.transform.childCount; i++)
        {
            // Check each item in data manager
            for (int y = 0; y < gMan.DataManager.storeData.BasesBought.Length; y++)
            {
                // If the data manager num matches this item - it means its bought
                if (i == gMan.DataManager.storeData.BasesBought[y])
                    uiManager.basesWindow.transform.GetChild(i).GetComponent<StoreItem>().bought = true;
            }

            // Check if the character equipped in data manager equals this one
            if (i == gMan.DataManager.storeData.EquippedBase)
                uiManager.basesWindow.transform.GetChild(i).GetComponent<StoreItem>().equipped = true;
        }
    }

    void SaveStoreData()
    {
        gMan.DataManager.storeData.Coins = coins;

        // Need to it for each item type

        // Characters 
        List<int> characterList = new List<int>();  // Make list to add the bought item nums
        for (int i = 0; i < uiManager.charactersWindow.transform.childCount; i++) // Go through window objects
        {
            if (uiManager.charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().bought) // If bought
            {
                characterList.Add(i);         // Add to list
            }
        }
        gMan.DataManager.storeData.CharactersBought = characterList.ToArray(); // Transform list to array

        // Weapons 
        List<int> weaponsList = new List<int>();    // Make list to add the bought item nums
        for (int i = 0; i < uiManager.weaponsWindow.transform.childCount; i++) // Go through window objects
        {
            if (uiManager.weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().bought) // If bought
            {
                weaponsList.Add(i);         // Add to list
            }

            // Update gun upgrades parameters
            Gun gun = uiManager.weaponsWindow.transform.GetChild(i).GetComponent<Gun>();
            StoreData sData = gMan.DataManager.storeData;

            sData.weaponsDamageUpgradesCount[i] =   gun.damageUpgradeCount;
            sData.weaponsRangeUpgradesCount[i] =    gun.rangeUpgradeCount;
            sData.weaponsFireRateUpgradesCount[i] = gun.fireRateUpgradeCount;
        }

        // Bases
        List<int> basesList = new List<int>();    // Make list to add the bought item nums
        for (int i = 0; i < uiManager.basesWindow.transform.childCount; i++) // Go through window objects
        {
            if (uiManager.basesWindow.transform.GetChild(i).GetComponent<StoreItem>().bought) // If bought
            {
                basesList.Add(i);         // Add to list
            }

            // Update gun upgrades parameters
            Base _base = uiManager.basesWindow.transform.GetChild(i).GetComponent<Base>();
            StoreData sData = gMan.DataManager.storeData;

            sData.baseHealthUpgradesCount[i] = _base.healthUpgradeCount;
        }

        gMan.DataManager.storeData.WeaponsBought = weaponsList.ToArray();  // Transform list to array

        gMan.DataManager.SaveData();
    }

    public void NextItem()
    {
        // Check if cant do it
        if (uiManager.currentItemNum + 1 == uiManager.currentWindow.childCount)
            return;

        uiManager.currentWindow.GetChild(uiManager.currentItemNum).gameObject.SetActive(false);
        uiManager.currentItemNum++;
        uiManager.currentWindow.GetChild(uiManager.currentItemNum).gameObject.SetActive(true);

        currentItem =
            uiManager.currentWindow.transform.GetChild(uiManager.currentItemNum).GetComponent<StoreItem>();

        uiManager.UpdateMainUI(currentItem);
    }
    public void PreviousItem()
    {
        // Check if cant do it
        if (uiManager.currentItemNum == 0)
            return;

        uiManager.currentWindow.GetChild(uiManager.currentItemNum).gameObject.SetActive(false);
        uiManager.currentItemNum--;
        uiManager.currentWindow.GetChild(uiManager.currentItemNum).gameObject.SetActive(true);

        currentItem =
            uiManager.currentWindow.transform.GetChild(uiManager.currentItemNum).GetComponent<StoreItem>();

        uiManager.UpdateMainUI(currentItem);
    }

    public void BuyOrEquipWeapon()
    {
        // Check if has enough money
        if (currentItem.cost > coins)
            return;

        coins -= currentItem.cost;

        // Check if bought
        Debug.Log(currentItem);
        if (currentItem.bought)
        {
            // Equip item
            switch (currentItem.type)
            { 
                // ! Need to have all item types!

                case ItemTypes.Weapon:
                    // Uneqip equipped item
                    uiManager.weaponsWindow.transform.GetChild(gMan.DataManager.storeData.EquippedWeapon) 
                        .GetComponent<StoreItem>().equipped = false;
                    // Equip current item
                    currentItem.equipped = true;
                    // Set equipped weapon in data manager
                    gMan.DataManager.storeData.EquippedWeapon = uiManager.currentItemNum;
                    break;
                case ItemTypes.Character:
                    // Uneqip equipped item
                    uiManager.charactersWindow.transform.GetChild(gMan.DataManager.storeData.EquippedCharacter)    
                        .GetComponent<StoreItem>().equipped = false;
                    // Equip current item
                    currentItem.equipped = true;
                    // Set equipped character in data manager
                    gMan.DataManager.storeData.EquippedCharacter = uiManager.currentItemNum;
                    break;
            }
        }
        else
        {
            // Buy item
            currentItem.bought = true;

            // Add to item num to store data
        }

        uiManager.UpdateMainUI(currentItem);

        SaveStoreData();
    }

    public void ExitStore()
    {
        gMan.LoadScene("Game");
    }

    //Weapon Upgrades Variables
    Gun currentGun;

    public int TarDmgUpCount   { get; set; }
    public int TarRngUpCount    { get; set; }
    public int TarFRUpCount { get; set; }

    // Weapon Upgrades Methods

    public float CalculateWeaponUpgradeCosts()
    {
        currentGun = currentItem.GetComponent<Gun>();

        // Get the upgrades count from the parametere bars
        TarDmgUpCount = 
            uiManager.damageBar.upgradeCount == currentGun.damageUpgradeCount ?
            0 : uiManager.damageBar.upgradeCount;

        TarRngUpCount = 
            uiManager.rangeBar.upgradeCount == currentGun.rangeUpgradeCount ?
            0:  uiManager.rangeBar.upgradeCount;

        TarFRUpCount = 
            uiManager.fireRateBar.upgradeCount == currentGun.fireRateUpgradeCount ?
            0 : uiManager.fireRateBar.upgradeCount;

        // Calculate the currentUpgrades Costs
        // Cost = round(baseCost * power(costMultiplier,upgradeCount)
        float dmgUpCost =
            Mathf.Ceil(currentItem.baseCost *
            Mathf.Pow(currentItem.costMultiplier, TarDmgUpCount) - currentItem.baseCost);
        float rngUpCost =
            Mathf.Ceil(currentItem.baseCost *
            Mathf.Pow(currentItem.costMultiplier, TarRngUpCount) - currentItem.baseCost);
        float frUpCost =
            Mathf.Ceil(currentItem.baseCost *
            Mathf.Pow(currentItem.costMultiplier, TarFRUpCount) - currentItem.baseCost);

        return Mathf.RoundToInt(dmgUpCost + rngUpCost + frUpCost);

    }

    public void BuyWeaponUpgrades()
    {
        if (coins < CalculateWeaponUpgradeCosts())
        {
            Debug.Log("Not enough money!");
            return;
        }

        coins -= (int)CalculateWeaponUpgradeCosts();

        gMan.DataManager.storeData.Coins = coins;

        // Save the upgrade data on the json files
        gMan.DataManager.storeData.weaponsDamageUpgradesCount[currentGun.gunNum] = uiManager.damageBar.upgradeCount;
        gMan.DataManager.storeData.weaponsRangeUpgradesCount[currentGun.gunNum] = uiManager.rangeBar.upgradeCount;
        gMan.DataManager.storeData.weaponsFireRateUpgradesCount[currentGun.gunNum] = uiManager.fireRateBar.upgradeCount;

        currentGun.UpdateGunParameters();

        uiManager.SetBaseBarsBaseParameters();
        uiManager.UpdateWeaponUpgradeMenu();

        gMan.DataManager.SaveData();
    }

    //Weapon Upgrades Variables
    Base currentBase;

    public int TarHlthUpCount { get; set; }

    // Base Upgrade Methods
    public float CalculateBaseUpgradeCosts()
    {
        currentBase = currentItem.GetComponent<Base>();

        // Get the upgrades count from the parametere bars
        TarHlthUpCount =
            uiManager.healthBar.upgradeCount == currentBase.healthUpgradeCount ?
            0 : uiManager.healthBar.upgradeCount;

        // Calculate the currentUpgrades Costs
        // Cost = round(baseCost * power(costMultiplier,upgradeCount)
        float hlthUpCost =
            Mathf.Ceil(currentItem.baseCost *
            Mathf.Pow(currentItem.costMultiplier, TarHlthUpCount) - currentItem.baseCost);

        return Mathf.RoundToInt(hlthUpCost);
    }

    public void BuyBaseUpgrades()
    {
        if (coins < CalculateWeaponUpgradeCosts())
        {
            Debug.Log("Not enough money!");
            return;
        }

        coins -= (int)CalculateWeaponUpgradeCosts();

        gMan.DataManager.storeData.Coins = coins;

        // Save the upgrade data on the json files
        gMan.DataManager.storeData.weaponsDamageUpgradesCount[currentGun.gunNum] = uiManager.damageBar.upgradeCount;
        gMan.DataManager.storeData.weaponsRangeUpgradesCount[currentGun.gunNum] = uiManager.rangeBar.upgradeCount;
        gMan.DataManager.storeData.weaponsFireRateUpgradesCount[currentGun.gunNum] = uiManager.fireRateBar.upgradeCount;

        currentGun.UpdateGunParameters();

        uiManager.SetBaseBarsBaseParameters();
        uiManager.UpdateWeaponUpgradeMenu();

        gMan.DataManager.SaveData();
    }
}
