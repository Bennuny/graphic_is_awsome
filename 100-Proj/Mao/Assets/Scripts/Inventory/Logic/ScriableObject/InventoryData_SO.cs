using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items;

    public void AddItem(ItemData_SO newItemData, int amount)
    {
        bool found = false;

        if (newItemData.stackable)
        {
            foreach (var item in items)
            {
                if (item.ItemData == newItemData)
                {
                    item.ItemData.Amount += amount;
                    found = true;
                    break;
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemData == null && !found)
            {
                items[i].ItemData = newItemData;
                items[i].Amount = amount;
                break;
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    
    public ItemData_SO ItemData;

    public int Amount;
}