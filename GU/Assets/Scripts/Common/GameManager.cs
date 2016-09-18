using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>  {

    public int Energy = 1000000;
    public int Gold = 0;
    public int Dia = 0;
    public int Heart = 0;

    public int MaxEnergy
    {
        get
        {

            return 10;
        }
    }

    public static AsyncOperation SceneSync = null;
    public static float SceneLoadingValue = 0f;

    public static void ViewDebug(object value)
    {
        if (GameInfo.DevelopMode)
            Debug.Log(value);
    }

    public static IEnumerator _SceneLoading(string _SceneName)
    {
        SceneLoadingValue = 0f;
        SceneSync = SceneManager.LoadSceneAsync(_SceneName);

        while (!SceneSync.isDone)
        {
            SceneLoadingValue = SceneSync.progress;
            yield return null;
        }

        yield return SceneSync;
    }
}
