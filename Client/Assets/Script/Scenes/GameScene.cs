using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    public override void Clear()
    {
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.UI.ShowSceneUI<UI_Inven>();

        gameObject.GetOrSetComponent<CusorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrSetComponent<CameraController>().SetPlayer(player);

        GameObject go = new GameObject { name = "Spawningpool" };

        SpawningPool pool = go.AddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }
}
