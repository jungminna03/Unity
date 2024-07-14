using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoder<Key, Value> // 'Dictionary'�� �����ϴ� *�������̽�
{
    // **Dictionary<Key, Value>�� ��ȯ�ϴ� MakeDict�Լ� ����
    Dictionary<Key, Value> MakeDict();
}

public class DataManager // �����͸� �����ϴ� Ŭ���� 'DataManager'
{
    // Dictionary<int, Data.Stat>�� ��ȯ�ϴ� ������ ���� �� �ʱ�ȭ / �б� O, ���� X 
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