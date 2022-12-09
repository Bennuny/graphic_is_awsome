using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStat : MonoBehaviour
{
    // Current, Total
    public event Action<int, int> UpdateHealthBarOnAttack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public Combat_SO combat;

    private Combat_SO baseCombat;

    [Header("Weapon")]
    public Transform weaponSlot;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }

        baseCombat = Instantiate(combat);
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

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }

        // TODO: update ui
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        // TODO: update exp
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.KillPoint);
        }
    }

    public void TakeDamage(int damage, CharacterStat defener)
    {
        var realDamage = Math.Max(damage - defener.CurrentDefence, 0);

        CurrentHealth = Math.Max(CurrentHealth - realDamage, 0);

        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.KillPoint);
        }
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

    #region
    public int CurrentExp
    {
        get
        {
            return characterData.CurrentExp;
        }
    }

    public int BaseExp
    {
        get
        {
            return characterData.BaseExp;
        }
    }

    public int CurrentLevel
    {
        get
        {
            return characterData.CurrentLevel;
        }
    }

    #endregion


    #region Equip Weapon

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.WeaponPrefab != null)
        {
            Instantiate(weapon.WeaponPrefab, weaponSlot);
        }

        combat.ApplyWeaponData(weapon.WeaponData);
        // TODO: animation

        InventoryManager.Instance.UpdateStatsText(CurrentHealth, (int)(MinDamage), (int)MaxDamage);
    }

    //
    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }

        combat.ApplyWeaponData(baseCombat);

        // TODO: animation
    }

    #endregion

    #region apply data change

    public void ApplyHealth(int amount)
    {
        CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
    }

    #endregion
}
