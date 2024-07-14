using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null; //Ű �̺�Ʈ�� ���� �׼� ����
    public Action<Define.MouseEvent> MouseEvent = null; //���콺 �̺�Ʈ�� ���� �׼� ����

    bool _pressed = false; // ���콺 ���ȴ��� Ȯ��
    float _pressedTime = 0; //���콺 ���� �ð�

    public void OnUpdate()
    {
        //��ġ�� ������Ʈ�� UI���� ������ ��� ��ȯ
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Input.anyKey && KeyAction != null) //Ű �̺�Ʈ ó��
            KeyAction.Invoke();


        if (MouseEvent != null) // ���콺 �̺�Ʈ ó��
        {
            if (Input.GetMouseButton(0)) // ���콺 ���� ��ư ���� ��
            {
                if (!_pressed) // ���ȴٸ�,
                {
                    MouseEvent.Invoke(Define.MouseEvent.PointerDown); // ��ư�� ó�� ��������, PointerDown �̺�Ʈ ȣ��
                    _pressedTime = Time.time; // ��ư ���� �ð� ����
                }
                MouseEvent.Invoke(Define.MouseEvent.Press); // ��ư�� ��� ������������ Press �̺�Ʈ ȣ��
                _pressed = true; // ��ư�� ���ȴ�.
            }
            else // ���콺 ���� ��ư ���� ��
            {
                if (_pressed) // �������ʾҴٸ�,
                {
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseEvent.Invoke(Define.MouseEvent.Click); //��ư�� �����ð��� 0.2�� ���϶�� Click �̺�Ʈ ȣ��
                    }
                    MouseEvent.Invoke(Define.MouseEvent.PointerUp); //PointerUp �̺�Ʈ ȣ��
                }
                _pressed = false; //��ư�� �ȴ��ȴ�.
                _pressedTime = 0; //���� �ð� �ʱ�ȭ
            }
        }
    }

    public void Clear() //��ϵ� �׼ǵ� �ʱ�ȭ
    {
        KeyAction = null;
        MouseEvent = null;
    }
}