using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SLOT_TYPE {
    BAG,
    WEAPON,
    ARMOR,
    ACTION,
}

public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SLOT_TYPE slotType;

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();    
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() == null)
        {
            return;
        }

        if (itemUI.GetItem().Type == ITEM_TYPE.USEABLE && itemUI.Bag.items[itemUI.Index].Amount > 0)
        {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);

            itemUI.Bag.items[itemUI.Index].Amount -= 1;
        }
    }

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

                    if (itemUI.GetItem() != null)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        //InventoryManager
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
