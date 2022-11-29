using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName ="Character Stat/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]

    public int MaxHealth;

    public int CurrentHealth;

    public int MaxDefence;

    public int CurrentDefence;

    [Header("Kill Point")]
    public int KillPoint;

    [Header("Level")]
    public int CurrentLevel;

    public int MaxLevel;

    public int CurrentExp;

    public int BaseExp;

    public float LevelBuff;


    private float LevelMultiplier
    {
        get
        {
            return 1 + (CurrentLevel - 1) * LevelBuff;
        }
    }


    public void UpdateExp(int point)
    {
        CurrentExp += point;

        if (CurrentExp >= BaseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // TODO: property

        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        CurrentExp = 0;

        BaseExp += (int)(BaseExp * LevelMultiplier);

        MaxHealth = (int)(MaxHealth * LevelMultiplier);

        CurrentHealth = MaxHealth;

        // Debug
        Debug.Log("Current Level " + CurrentLevel + ", Max Health: " + MaxHealth);
    }
}
