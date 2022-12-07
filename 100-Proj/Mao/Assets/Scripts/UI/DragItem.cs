using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;

    SlotHolder currentHolder;

    SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();

        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // record drag item index

        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        transform.position = eventData.position;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();

        // Switch item

        // 指向UI物品
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckActionUI(eventData.position) ||
                InventoryManager.Instance.CheckEquipmentUI(eventData.position) ||
                InventoryManager.Instance.CheckInventoryUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }


                switch (targetHolder.slotType)
                {
                    case SLOT_TYPE.BAG:
                        {
                            SwapItem();
                        }
                        break;
                    case SLOT_TYPE.WEAPON:
                        {

                        }
                        break;
                    case SLOT_TYPE.ARMOR:
                        {

                        }
                        break;
                    case SLOT_TYPE.ACTION:
                        {

                        }
                        break;
                }

                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];

        var tempItem = currentHolder.itemUI.Bag.items[targetHolder.itemUI.Index];

        bool isSameItem = targetItem.ItemData == tempItem.ItemData;

        if (isSameItem && targetItem.ItemData.stackable)
        {
            targetItem.Amount += tempItem.Amount;
            tempItem.ItemData = null;
            tempItem.Amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = targetItem;
            targetItem = tempItem;
        }
    }
}
