using System.IO;
using UnityEngine;

public class JsonDataManager : MonoBehaviour
{
    private void Awake()
    {
        storeDataPath = Path.Combine(Application.persistentDataPath, "StoreData.json");
        gameplayDataPath = Path.Combine(Application.persistentDataPath, "GameplayData.json");

        LoadData();
    }

    string storeDataPath, gameplayDataPath;

    public StoreData storeData;
    public GamePlayData gamePlayData;

    public void LoadData()
    {
        if (File.Exists(storeDataPath))
        {
            storeData = JsonUtility.FromJson<StoreData>(File.ReadAllText(storeDataPath));
        }
        else
        {
            Debug.Log("Store data dosent exist - creating file");

            CreateNewStoreData();

            File.WriteAllText(storeDataPath, JsonUtility.ToJson(storeData));
        }
        if (File.Exists(gameplayDataPath))
        {
            gamePlayData = JsonUtility.FromJson<GamePlayData>(File.ReadAllText(gameplayDataPath));
        }
        else
        {
            Debug.Log("Gameplay data dosent exist - creating file");
            gamePlayData.playerHighestLevel = 1;
            File.WriteAllText(gameplayDataPath, JsonUtility.ToJson(gamePlayData));
        }
    }

    private void CreateNewStoreData()
    {
        //Temporary - to have lots of money
        storeData.Coins = 1000000;

        // Make new items bought arrays for the first item to be bought 
        storeData.CharactersBought = new int[1];
        storeData.WeaponsBought = new int[1];
        storeData.BasesBought = new int[1];

        //(Change it if the number of total weapons changes)
        storeData.weaponsDamageUpgradesCount = new int[6];
        storeData.weaponsRangeUpgradesCount = new int[6];
        storeData.weaponsFireRateUpgradesCount = new int[6];

        for (int i = 0; i < storeData.weaponsDamageUpgradesCount.Length; i++)
            storeData.weaponsDamageUpgradesCount[i] = 1;
        for (int i = 0; i < storeData.weaponsRangeUpgradesCount.Length; i++)
            storeData.weaponsRangeUpgradesCount[i] = 1;
        for (int i = 0; i < storeData.weaponsFireRateUpgradesCount.Length; i++)
            storeData.weaponsFireRateUpgradesCount[i] = 1;

        storeData.baseHealthUpgradesCount = new int[1];

        for (int i = 0; i < storeData.baseHealthUpgradesCount.Length; i++)
            storeData.baseHealthUpgradesCount[i] = 1;
    }

    public void SaveData()
    {
        File.WriteAllText(storeDataPath, JsonUtility.ToJson(storeData));
        File.WriteAllText(gameplayDataPath, JsonUtility.ToJson(gamePlayData));
    }
}

[System.Serializable]
public class StoreData
{
    public int Coins;

    // Store Items
    public int[] WeaponsBought;
    public int EquippedWeapon;

    public int[] CharactersBought;
    public int EquippedCharacter;

    public int[] BasesBought;
    public int EquippedBase;

    // Upgrades

    // Weapons
    public int[] weaponsDamageUpgradesCount;
    public int[] weaponsRangeUpgradesCount;
    public int[] weaponsFireRateUpgradesCount;

    // Bases
    public int[] baseHealthUpgradesCount;

}

[System.Serializable]
public class GamePlayData
{
    public int playerHighestLevel;
}
