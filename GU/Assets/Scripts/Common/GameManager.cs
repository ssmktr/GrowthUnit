using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public static void GoScene(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }

    public static void ViewDebug(object value)
    {
        if (GameInfo.DevelopMode)
            Debug.Log(value);
    }
}
