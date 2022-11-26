using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public Combat_SO combat;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }

    #region Read&Write From DATA_SO
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.MaxHealth;
            }
            return 0;
        }
        set
        {
            characterData.MaxHealth = value;
        }
    }


    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.CurrentHealth;
            }
            return 0;
        }
        set
        {
            characterData.CurrentHealth = value;
        }
    }

    public int MaxDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.MaxDefence;
            }
            return 0;
        }
        set
        {
            characterData.MaxDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.CurrentDefence;
            }
            return 0;
        }
        set
        {
            characterData.CurrentDefence = value;
        }
    }

    #endregion

    #region Read&Write Combat Data
    public float AttackRange
    {
        get
        {
            if (combat)
            {
                return combat.AttackRange;
            }
            return 0;
        }

        set
        {
            combat.AttackRange = value;
        }
    }

    public float SkillRange
    {
        get
        {
            if (combat)
            {
                return combat.SkillRange;
            }
            return 0;
        }

        set
        {
            combat.SkillRange = value;
        }
    }

    public float CoolDown
    {
        get
        {
            if (combat)
            {
                return combat.CoolDown;
            }
            return 0;
        }

        set
        {
            combat.CoolDown = value;
        }
    }

    public float MinDamage
    {
        get
        {
            if (combat)
            {
                return combat.MinDamage;
            }
            return 0;
        }

        set
        {
            combat.MinDamage = value;
        }
    }

    public float MaxDamage
    {
        get
        {
            if (combat)
            {
                return combat.MaxDamage;
            }
            return 0;
        }

        set
        {
            combat.MaxDamage = value;
        }
    }

    public float CriticalMultiplier
    {
        get
        {
            if (combat)
            {
                return combat.CriticalMultiplier;
            }
            return 0;
        }

        set
        {
            combat.CriticalMultiplier = value;
        }
    }

    public float CriticalChance
    {
        get
        {
            if (combat)
            {
                return combat.CriticalChance;
            }
            return 0;
        }

        set
        {
            combat.CriticalChance = value;
        }
    }
    #endregion

    #region

    public void TakeDamage(CharacterStat attacker, CharacterStat defener)
    {
        int damage = Math.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);

        CurrentHealth = Math.Max(CurrentHealth - damage, 0);

        if (isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }

        // TODO: update ui
        // TODO: update exp
        
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(MinDamage, MaxDamage);

        if (isCritical)
        {
            coreDamage *= CriticalMultiplier;
            Debug.Log("Critical Damage!" + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion
}
