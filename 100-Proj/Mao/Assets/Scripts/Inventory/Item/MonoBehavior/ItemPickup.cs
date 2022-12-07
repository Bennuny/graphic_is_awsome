using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: add item to bag
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.Amount);

            InventoryManager.Instance.inventoryUI.RefreshUI();

            // equip weapon
            // GameManager.Instance.playerStats.EquipWeapon(itemData);

            Destroy(gameObject);
        }
    }
}
