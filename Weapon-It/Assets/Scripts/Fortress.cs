﻿using UnityEngine;
using UnityEngine.UI;

public class Fortress : MonoBehaviour
{
    public float maxLife = 10;
    float life = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if (life <= 0)
            FortressDestroyed();

        indiactor.value = 1 -( (maxLife - life) / maxLife);
    }

    void FortressDestroyed()
    {
        levelCon.LostLevel();
    }

    public Slider indiactor;

    LevelController levelCon;

    private void Start()
    {
        levelCon = LevelController.instance;

        life = maxLife;
    }
}
