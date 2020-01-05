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

            // Make new items bought arrays for the first item to be bought 
            storeData.CharactersBought = new int[1];
            storeData.WeaponsBought = new int[1];

            //(Change it if the number of total weapons changes)
            storeData.weaponsDamageUpgradesCount =      new int[6];
            storeData.weaponsRangeUpgradesCount =       new int[6];
            storeData.weaponsFireRateUpgradesCount =    new int[6];

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

    public int[] WeaponsBought;
    public int EquippedWeapon;

    public int[] CharactersBought;
    public int EquippedCharacter;

    public int[] weaponsDamageUpgradesCount;
    public int[] weaponsRangeUpgradesCount;
    public int[] weaponsFireRateUpgradesCount;
}

[System.Serializable]
public class GamePlayData
{
    public int playerHighestLevel;
}
