using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    // ���� Scene���� BaseScene������Ʈ�� ���� ������Ʈ�� ã�� ��ȯ
    public BaseScene QurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // ���� Scene�� ����� ���ο� Scene �ε�
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    // ������ Ŭ���� enum���� Define.Scene�ڷ����� �����ϰ�, type���·� ��ȯ
    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    // ���� �� ����
    public void Clear()
    {
        QurrentScene.Clear();
    }
}