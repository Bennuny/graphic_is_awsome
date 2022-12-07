using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            Debug.Log("refresh item " + i);
            Debug.Log(slotHolders[i]);
            Debug.Log(slotHolders[i].itemUI);

            slotHolders[i].itemUI.Index = i;

            slotHolders[i].UpdateItem();
        }
    }
}
