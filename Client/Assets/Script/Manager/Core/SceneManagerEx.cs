using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    // 현재 Scene에서 BaseScene컴포넌트를 가진 오브젝트를 찾아 반환
    public BaseScene QurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // 현재 Scene을 지우고 새로운 Scene 로드
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    // 열거형 클래스 enum에서 Define.Scene자료형을 지정하고, type형태로 반환
    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    // 이전 씬 정리
    public void Clear()
    {
        QurrentScene.Clear();
    }
}