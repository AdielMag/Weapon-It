using System.IO;
using UnityEngine;

public class JsonDataManager : MonoBehaviour
{
    private void Awake()
    {
        storeDataPath = Path.Combine(Application.persistentDataPath, "StoreData.json");
        gameplayDataPath = Path.Combine(Application.persistentDataPath, "GameplayData.json");
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
            File.WriteAllText(storeDataPath, JsonUtility.ToJson(storeData));
        }
        if (File.Exists(gameplayDataPath))
        {
            gamePlayData = JsonUtility.FromJson<GamePlayData>(File.ReadAllText(gameplayDataPath));
        }
        else
        {
            Debug.Log("Gameplay data dosent exist - creating file");
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
    public int[] CharactersBought;
    public int EquippedCharacter;

    public int[] WeaponsBought;
    public int EquippedWeapon;
}

[System.Serializable]
public class GamePlayData
{
    public int playerHighestLevel;
}
