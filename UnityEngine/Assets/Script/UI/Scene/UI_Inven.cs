using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    override public void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject Inven =  GetObject((int)GameObjects.GridPanel);

        foreach (Transform Items in Inven.transform)
        {
            Managers.Resource.Destroy(Items.gameObject);
        }

        for (int i = 0; i < 8; ++i)
        {
            GameObject Item = Managers.UI.MakeSubItem<UI_Inven_Item>(Inven.transform).gameObject;

            UI_Inven_Item item = Item.GetOrSetComponent<UI_Inven_Item>();
            item.SetInfo("БэЗа°Л" + i);
        }
    }
}
