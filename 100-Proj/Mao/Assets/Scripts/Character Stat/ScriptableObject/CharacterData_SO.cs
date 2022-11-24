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
}
