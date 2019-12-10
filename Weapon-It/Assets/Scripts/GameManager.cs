using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton
    public static GameManager instance;
    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);

        instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    ObjectPooler objPooler;

    public UIMethods uiManager;
    public LevelController LevelCon { get; private set; }
    public JsonDataManager DataManager { get; private set; }

    private void Start()
    {
        objPooler = ObjectPooler.instance;
        LevelCon = GetComponentInChildren<LevelController>();
        DataManager = GetComponentInChildren<JsonDataManager>();


        objPooler.InstantiatePools();
        LevelCon.SpawnAllLevels();
        DataManager.LoadData();
        uiManager.UpdateLevelsButtons(DataManager.gamePlayData.playerHighestLevel);
    }

    
}
