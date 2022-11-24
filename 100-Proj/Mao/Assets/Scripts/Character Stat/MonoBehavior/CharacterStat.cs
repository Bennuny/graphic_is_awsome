using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public CharacterData_SO characterData;

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
}
