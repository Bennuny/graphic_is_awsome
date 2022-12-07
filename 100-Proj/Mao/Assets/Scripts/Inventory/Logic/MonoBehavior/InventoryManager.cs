using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    // TODO: 最后创建模板
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;

    [Header("Inventory Container")]
    public ContainerUI inventoryUI;

    private void Start()
    {
        inventoryUI.RefreshUI();
    }
}
