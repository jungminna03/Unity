using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;

    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    // ? 
    //HashSet 특정 데이터를 가져옴 -> hash란 다양한 길이의 데이터를 고정된 데이터로 메핑
    public Action<int> OnMonsterSpawnEvent = null;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnMonsterSpawnEvent != null)
                    OnMonsterSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.gameObject.GetComponent<BaseController>();

        if (bc == null)
            return Define.WorldObject.Unkown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = go.GetWorldObjectType();

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnMonsterSpawnEvent != null)
                            OnMonsterSpawnEvent.Invoke(-1);
                    }

                }
                break;
            case Define.WorldObject.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
