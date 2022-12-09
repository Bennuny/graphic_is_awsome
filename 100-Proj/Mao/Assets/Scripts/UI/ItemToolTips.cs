using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTips : MonoBehaviour
{
    public Text Name;
    public Text Desc;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupTooltip(ItemData_SO item)
    {
        Name.text = item.Name;

        Desc.text = item.Description;
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = corners[2].x - corners[1].x;
        float height = corners[1].y - corners[0].y;

        //Debug.Log(mousePos.y);

        // 鼠标坐标以左下角为0，0

        if (mousePos.y < height)
        {
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        }
        else if (Screen.width - mousePos.x > width)
        {
            rectTransform.position = mousePos + Vector3.right * width * 0.5f;
        }
        else
        {
            rectTransform.position = mousePos - Vector3.right * width * 0.5f;
        }

        rectTransform.position = mousePos;        
    }
}
