using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// [System.Serializable]
// public class EventVector3 : UnityEvent<Vector3> { };

public class MouseManager : Singleton<MouseManager>
{
    private RaycastHit _hitinfo;

    public Texture2D Point, Doorwar, Attack, Target, Arrow;

    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked;

    protected override void Awake()
    {
        base.Awake();

        //DontDestroyOnLoad(this);
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
            switch (_hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    {
                        Cursor.SetCursor(Target, new Vector2(16, 16), CursorMode.Auto);
                    }
                    break;
                case "Enemy":
                    {
                        Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
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
            else if (_hitinfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(_hitinfo.collider.gameObject);
            }
            else if (_hitinfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(_hitinfo.collider.gameObject);
            }
        }
    }
}
