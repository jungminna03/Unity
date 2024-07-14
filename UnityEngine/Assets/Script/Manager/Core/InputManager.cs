using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null; //키 이벤트에 대한 액션 저장
    public Action<Define.MouseEvent> MouseEvent = null; //마우스 이벤트에 대한 액션 저장

    bool _pressed = false; // 마우스 눌렸는지 확인
    float _pressedTime = 0; //마우스 눌린 시간

    public void OnUpdate()
    {
        //터치한 오브젝트가 UI위에 존재할 경우 반환
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Input.anyKey && KeyAction != null) //키 이벤트 처리
            KeyAction.Invoke();


        if (MouseEvent != null) // 마우스 이벤트 처리
        {
            if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼 누를 때
            {
                if (!_pressed) // 눌렸다면,
                {
                    MouseEvent.Invoke(Define.MouseEvent.PointerDown); // 버튼이 처음 눌렀으면, PointerDown 이벤트 호출
                    _pressedTime = Time.time; // 버튼 눌린 시간 저장
                }
                MouseEvent.Invoke(Define.MouseEvent.Press); // 버튼이 계속 눌러져있으면 Press 이벤트 호출
                _pressed = true; // 버튼이 눌렸다.
            }
            else // 마우스 왼쪽 버튼 뗐을 때
            {
                if (_pressed) // 눌리지않았다면,
                {
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseEvent.Invoke(Define.MouseEvent.Click); //버튼이 눌린시간이 0.2초 이하라면 Click 이벤트 호출
                    }
                    MouseEvent.Invoke(Define.MouseEvent.PointerUp); //PointerUp 이벤트 호출
                }
                _pressed = false; //버튼이 안눌렸다.
                _pressedTime = 0; //눌린 시간 초기화
            }
        }
    }

    public void Clear() //등록된 액션들 초기화
    {
        KeyAction = null;
        MouseEvent = null;
    }
}