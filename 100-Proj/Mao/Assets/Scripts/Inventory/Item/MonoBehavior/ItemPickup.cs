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


            // equip weapon

            GameManager.Instance.playerStats.EquipWeapon(itemData);

            Destroy(gameObject);
        }
    }
}
