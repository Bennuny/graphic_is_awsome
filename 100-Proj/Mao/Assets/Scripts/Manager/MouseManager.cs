using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// [System.Serializable]
// public class EventVector3 : UnityEvent<Vector3> { };

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    private RaycastHit _hitinfo;

    public Texture2D Point, Doorwar, Attack, Target, Arrow;

    public event Action<Vector3> OnMouseClicked;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    private void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(Input.mousePosition);

        SetCursorTexture();

        MouseControl();
    }

    private void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hitinfo))
        {
            // TODO

            switch (_hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    {
                        Cursor.SetCursor(Target, new Vector2(16, 16), CursorMode.Auto);
                    }
                    break;
            }

        }
    }

    private void MouseControl()
    {
        if (Input.GetMouseButton(0) && _hitinfo.collider != null && _hitinfo.collider.gameObject != null)
        {
            if (_hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(_hitinfo.point);
            }
        }
    }
}
