using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _moveIcon;

    enum CursorType
    {
        None,
        Attack,
        Move,
    }

    CursorType _cursorType = CursorType.None;

    int _mask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);

    private void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _moveIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Move");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Move)
                {
                    Cursor.SetCursor(_moveIcon, new Vector2(_moveIcon.width / 2, _moveIcon.height / 2), CursorMode.Auto);
                    _cursorType = CursorType.Move;
                }
            }
        }
    }
}
