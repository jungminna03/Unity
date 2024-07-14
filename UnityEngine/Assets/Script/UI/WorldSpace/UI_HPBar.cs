using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPSlider
    }

    Stat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        transform.position = transform.parent.position + (Vector3.up * transform.parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        SetHpRatio(_stat.HP / (float)_stat.MaxHp);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPSlider).GetComponent<Slider>().value = ratio;
    }
}
