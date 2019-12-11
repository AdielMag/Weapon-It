using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMethods : MonoBehaviour
{
    Transform levelsButtonParent;
    [HideInInspector]
    public GameObject levelCompleted, lostIndicator;

    public GameManager gMan;

    private void Awake()
    {
        levelsButtonParent = transform.GetChild(0);

        levelCompleted = transform.GetChild(1).gameObject;
        lostIndicator = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        gMan = GameManager.instance;

        gMan.UIManager = this;

        UpdateLevelsButtons(gMan.DataManager.gamePlayData.playerHighestLevel);
    }

    public void UpdateLevelsButtons(int highestLevel)
    {
        for (int i = 0; i < levelsButtonParent.childCount; i++)
        {
            if (i <= highestLevel)
                levelsButtonParent.GetChild(i).gameObject.SetActive(true);
            else
                levelsButtonParent.GetChild(i).gameObject.SetActive(false);
        }

    }

}