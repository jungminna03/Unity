using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    // UI enum���� ����(���� �ִ� �̸��� �Ȱ��ƾ���)
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    enum GameObjects
    {
        TestObj
    }

    enum Images
    { 

    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetText((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);
    }

    int _score;

    public void OnButtonClicked(PointerEventData data)
    {
        ++_score;

        Get<Text>((int)Texts.ScoreText).text = $"���� : {_score}��";
    }
}
