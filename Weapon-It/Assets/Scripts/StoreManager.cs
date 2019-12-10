using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public int coins;

    [Space]
    // Add the window objects for each type.
    public GameObject weaponsWindow;
    public GameObject charactersWindow;
    // Declare all your item types.
    public enum ItemTypes { Weapon, Character }

    Transform currentWindow;
    int currentItemNum;

    GameManager gMan;

    private void Start()
    {
        gMan = GameManager.instance;

        LoadStoreData();

        CheckWhichWindowIsOpen();
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

        // Set first item to be bought!
        charactersWindow.transform.GetChild(0).GetComponent<StoreItem>().bought = true;
        weaponsWindow.transform.GetChild(0).GetComponent<StoreItem>().bought = true;

        // Check each item in 'Weapons window'
        for (int i = 0; i< weaponsWindow.transform.childCount; i++)     
        {
            // Check each item in data manager
            for (int y = 0; y < gMan.DataManager.storeData.WeaponsBought.Length; y++)
            {
                if (i == gMan.DataManager.storeData.WeaponsBought[y])
                    weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().bought = true;
            }

            // Check if the weapon equipped in data manager equals this one
            if (i == gMan.DataManager.storeData.EquippedWeapon)
                weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().equipped = true;
        }
        // Check each item in 'Character window'
        for (int i = 0; i < charactersWindow.transform.childCount; i++) 
        {
            // Check each item in data manager
            for (int y = 0; y < gMan.DataManager.storeData.CharactersBought.Length; y++)
            {
                // If the data manager num matches this item - it means its bought
                if (i == gMan.DataManager.storeData.CharactersBought[y])
                    charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().bought = true;
            }

            // Check if the character equipped in data manager equals this one
            if (i == gMan.DataManager.storeData.EquippedCharacter)
                charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().equipped = true;
        }
    }

    void SaveStoreData()
    {
        gMan.DataManager.storeData.Coins = coins;

        // Need to it for each item type

        // Characters 
        List<int> characterList = new List<int>();  // Make list to add the bought item nums
        for (int i = 0; i < charactersWindow.transform.childCount; i++) // Go through window objects
        {
            if (charactersWindow.transform.GetChild(i).GetComponent<StoreItem>().bought) // If bought
            {
                characterList.Add(i);         // Add to list
            }
        }
        gMan.DataManager.storeData.CharactersBought = characterList.ToArray(); // Transform list to array

        // Weapons 
        List<int> weaponsList = new List<int>();    // Make list to add the bought item nums
        for (int i = 0; i < weaponsWindow.transform.childCount; i++) // Go through window objects
        {
            if (weaponsWindow.transform.GetChild(i).GetComponent<StoreItem>().bought) // If bought
            {
                weaponsList.Add(i);         // Add to list
            }
        }
        gMan.DataManager.storeData.WeaponsBought = weaponsList.ToArray();  // Transform list to array

        gMan.DataManager.SaveData();
    }

    // Used at the start to determine which window is open
    void CheckWhichWindowIsOpen()
    {
        // Need to add all windows!
        if (weaponsWindow.activeSelf)
            currentWindow = weaponsWindow.transform;
        else if (charactersWindow.activeSelf)
            currentWindow = charactersWindow.transform;

        // If no one is active.
        else
        {
            // Activate the first window (in this case - 'Weapons')
            weaponsWindow.SetActive(true);
            currentWindow = weaponsWindow.transform;
        }

        GetCurrentItemNum();
    }

    public void OpenWindow(string windowTypeName)
    {
        // Disable openedWindow
        currentWindow.gameObject.SetActive(false);

        // Open wanted window - need to check for each item type.
        if (windowTypeName == ItemTypes.Weapon.ToString())
        {
            currentWindow = weaponsWindow.transform;
            weaponsWindow.SetActive(true);
            GetCurrentItemNum();
        }
        else if (windowTypeName == ItemTypes.Character.ToString())
        {
            currentWindow = charactersWindow.transform;
            charactersWindow.SetActive(true);
            GetCurrentItemNum();
        }
        else
            Debug.LogWarning("Misspelled the type name! check button string.");
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

    public void NextItem()
    {
        // Check if cant do it
        if (currentItemNum + 1 == currentWindow.childCount)
            return;
        
        currentWindow.GetChild(currentItemNum).gameObject.SetActive(false);
        currentItemNum++;
        currentWindow.GetChild(currentItemNum).gameObject.SetActive(true);
    }
    public void PreviousItem()
    {
        // Check if cant do it
        if (currentItemNum == 0)
            return;

        currentWindow.GetChild(currentItemNum).gameObject.SetActive(false);
        currentItemNum--;
        currentWindow.GetChild(currentItemNum).gameObject.SetActive(true);
    }

    public void BuyOrEquipWeapon()
    {
        StoreItem currentItem = 
            currentWindow.transform.GetChild(currentItemNum).GetComponent<StoreItem>();

        // Check if has enough money
        if (currentItem.cost > coins)
            return;

        coins -= currentItem.cost;

        // Check if bought
        if (currentItem.bought)
        {
            // Equip item
            switch (currentItem.type)
            { 
                // ! Need to have all item types!

                case ItemTypes.Weapon:
                    // Uneqip equipped item
                    weaponsWindow.transform.GetChild(gMan.DataManager.storeData.EquippedWeapon) 
                        .GetComponent<StoreItem>().equipped = false;
                    // Equip current item
                    weaponsWindow.transform.GetChild(currentItemNum)
                        .GetComponent<StoreItem>().equipped = true;
                    // Set equipped weapon in data manager
                    gMan.DataManager.storeData.EquippedWeapon = currentItemNum;
                    break;
                case ItemTypes.Character:
                    // Uneqip equipped item
                    charactersWindow.transform.GetChild(gMan.DataManager.storeData.EquippedCharacter)    
                        .GetComponent<StoreItem>().equipped = false;
                    // Equip current item
                    charactersWindow.transform.GetChild(currentItemNum)
                        .GetComponent<StoreItem>().equipped = true;
                    // Set equipped character in data manager
                    gMan.DataManager.storeData.EquippedCharacter = currentItemNum;
                    break;
            }
        }
        else
        {
            // Buy item
            currentItem.bought = true;

            // Add to item num to store data
        }

        SaveStoreData();
    }

    public void ExitStore()
    {
        gMan.LoadScene("Game");
    }
}
