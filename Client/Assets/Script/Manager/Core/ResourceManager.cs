using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    // *제네릭 클래스
    public T Load<T>(string path) where T : Object
    {
        //매개변수의 자료형이 GameObject일 경우
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int idx = name.LastIndexOf('/');
            if (idx >= 0)
            {
                name = name.Substring(idx + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>($"Prefebs/{path}");
        if (original == null)
        {
            Debug.Log($"Faild to load Prefeb : {path}");
            return null;
        }


        if (original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        int idx = go.name.IndexOf("(Clone)");
        if (idx > 0)
            go.name = go.name.Substring(0, idx);


        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) //gameobject 값이 null이면 파괴 되지 않음
        {
            return;
        }


        Poolable poolable = go.GetComponent<Poolable>(); // <- ? 
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}