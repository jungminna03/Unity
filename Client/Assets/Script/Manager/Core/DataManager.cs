using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoder<Key, Value> // 'Dictionary'를 생성하는 *인터페이스
{
    // **Dictionary<Key, Value>를 반환하는 MakeDict함수 생성
    Dictionary<Key, Value> MakeDict();
}

public class DataManager // 데이터를 관리하는 클래스 'DataManager'
{
    // Dictionary<int, Data.Stat>를 반환하는 변수를 생성 및 초기화 / 읽기 O, 쓰기 X 
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();


    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoder<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}