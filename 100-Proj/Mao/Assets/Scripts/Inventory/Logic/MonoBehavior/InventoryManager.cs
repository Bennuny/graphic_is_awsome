using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;

        public RectTransform originalParent;
    }

    // TODO: 最后创建模板
    [Header("Inventory Data")]
    public InventoryData_SO inventoryDataTemplate;

    public InventoryData_SO actionDataTemplate;

    public InventoryData_SO equipmentDataTemplate;

    public InventoryData_SO inventoryData;

    public InventoryData_SO actionData;

    public InventoryData_SO equipmentData;

    [Header("Inventory Container")]
    public ContainerUI inventoryUI;

    public ContainerUI actionUI;

    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    public GameObject bagPanel;

    public GameObject statsPanel;

    bool isOpen = false;

    [Header("Stats Text")]
    public Text healthText;

    public Text combatText;

    [Header("Tooltip")]
    public ItemToolTips tooltip;

    protected override void Awake()
    {
        base.Awake();


        if (inventoryDataTemplate != null)
        {
            inventoryData = Instantiate(inventoryDataTemplate);
        }

        if (actionDataTemplate != null)
        {
            actionData = Instantiate(actionDataTemplate);
        }

        if (equipmentDataTemplate != null)
        {
            equipmentData = Instantiate(equipmentDataTemplate);
        }
    }

    private void Start()
    {
        LoadData();

        inventoryUI.RefreshUI();

        actionUI.RefreshUI();

        equipmentUI.RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }

        var playerStat = GameManager.Instance.playerStats;
        UpdateStatsText(playerStat.MaxHealth, (int)playerStat.MinDamage, (int)playerStat.MaxDamage);
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }


    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }


    public void UpdateStatsText(int health, int combatMin, int combatMax)
    {
        healthText.text = health.ToString();
        combatText.text = combatMin.ToString() + "-" + combatMax.ToString();
    }

    #region 拖拽物品是否在slot范围内

    public bool CheckInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}
