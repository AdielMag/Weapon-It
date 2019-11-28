using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [HideInInspector]
    // Used to check if level is currently running
    public bool levelInProgress;

    ObjectPooler objPooler;
    LevelController levelCon;

    private void Start()
    {
        objPooler = ObjectPooler.instance;
        levelCon = GetComponentInChildren<LevelController>();

        objPooler.InstantiatePools();
        levelCon.SpawnAllLevels();
    }
}
