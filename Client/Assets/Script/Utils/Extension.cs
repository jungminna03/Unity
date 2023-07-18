using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static T GetOrSetComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrSetComponent<T>(go);
    }

    public static Define.WorldObject GetWorldObjectType(this GameObject go)
    {
        return Managers.Game.GetWorldObjectType(go);
    }
    
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
