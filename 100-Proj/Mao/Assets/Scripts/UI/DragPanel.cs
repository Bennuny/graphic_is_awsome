using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    RectTransform rectTransfrom;

    Canvas canvas;

    private void Awake()
    {
        rectTransfrom = GetComponent<RectTransform>();

        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransfrom.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Debug.Log(rectTransfrom.GetSiblingIndex());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransfrom.SetSiblingIndex(2);
    }
}
