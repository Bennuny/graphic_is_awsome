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
            if (InventoryManager.Instance.CheckActionUI(eventData.position) || InventoryManager.Instance.CheckEquipmentUI(eventData.position) || InventoryManager.Instance.CheckInventoryUI(eventData.position))
            //if (InventoryManager.Instance.CheckInventoryUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder)
                {
                    switch (targetHolder.slotType)
                    {
                        case SLOT_TYPE.BAG:
                            {
                                SwapItem();
                            }
                            break;
                        case SLOT_TYPE.WEAPON:
                            {
                                if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.Type == ITEM_TYPE.WEAPON)
                                {
                                    SwapItem();
                                }
                            }
                            break;
                        case SLOT_TYPE.ARMOR:
                            {
                                if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.Type == ITEM_TYPE.ARMOR)
                                {
                                    SwapItem();
                                }
                            }
                            break;
                        case SLOT_TYPE.ACTION:
                            {
                                if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.Type == ITEM_TYPE.USEABLE)
                                {
                                    SwapItem();
                                }
                            }
                            break;
                    }

                    currentHolder.UpdateItem();
                    targetHolder.UpdateItem();
                }
            }
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);

        //RectTransform t = transform as RectTransform;
        //// right top
        //t.offsetMax = -Vector2.one * 5;
        //// left bottom
        //t.offsetMin = Vector2.one * 5;

        //var tt = transform as RectTransform;
        //tt.offsetMax = -Vector2.one * 5;
        //tt.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        Debug.Log("target-Index: " + targetHolder.itemUI.Index + " <-> current-Index: " + currentHolder.itemUI.Index);

        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

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
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
