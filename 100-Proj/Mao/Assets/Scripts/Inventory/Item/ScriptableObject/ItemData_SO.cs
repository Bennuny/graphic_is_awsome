using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    USEABLE,
    WEAPON,
    ARMOR,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ITEM_TYPE Type;

    public string Name;

    public Sprite Icon;

    public int Amount;

    [TextArea]
    public string Description;

    public bool stackable;

    [Header("Weapon")]
    public GameObject WeaponPrefab;

    public Combat_SO WeaponData;

    [Header("Userable Item")]
    public UseableItemData_SO useableData;
}
