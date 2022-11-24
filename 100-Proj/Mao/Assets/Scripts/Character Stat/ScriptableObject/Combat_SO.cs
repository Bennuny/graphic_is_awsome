using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack Data", menuName ="Character Stat/Attack")]
public class Combat_SO : ScriptableObject
{
    public float AttackRange;

    public float SkillRange;

    public float CoolDown;

    public float MinDamage;

    public float MaxDamage;

    // 暴击倍数
    public float CriticalMultiplier;

    // 暴击率
    public float CriticalChance;
}
