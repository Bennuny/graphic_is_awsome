using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SLOT_TYPE {
    BAG,
    WEAPON,
    ARMOR,
    ACTION,
}

public class SlotHolder : MonoBehaviour
{
    public SLOT_TYPE slotType;

    public ItemUI itemUI;

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SLOT_TYPE.BAG:
                {
                    itemUI.Bag = InventoryManager.Instance.inventoryData;
                }
                break;
            case SLOT_TYPE.WEAPON:
                {
                    itemUI.Bag = InventoryManager.Instance.equipmentData;

                    if (itemUI.Bag.items[itemUI.Index].ItemData != null)
                    {
                        GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.Index].ItemData);
                    }
                    else
                    {
                        GameManager.Instance.playerStats.UnEquipWeapon();
                    }
                }
                break;
            case SLOT_TYPE.ARMOR:
                {
                    itemUI.Bag = InventoryManager.Instance.equipmentData;
                }
                break;
            case SLOT_TYPE.ACTION:
                {
                    itemUI.Bag = InventoryManager.Instance.actionData;
                }
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.ItemData, item.Amount);

    }

}
