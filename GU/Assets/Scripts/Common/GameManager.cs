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

    public static Dictionary<int, NetData.UnitData> HaveUnitData = new Dictionary<int, NetData.UnitData>();
    public static NetData.UnitData GetMyUnit(int _Gsn)
    {
        if (HaveUnitData.ContainsKey(_Gsn))
            return HaveUnitData[_Gsn];

        Debug.LogWarning("Not Have Unit GSN");
        return null;
    }

    public static AsyncOperation SceneSync = null;
    public static float SceneLoadingValue = 0f;

    public static void ViewDebug(object value)
    {
        if (GameInfo.DevelopMode)
            Debug.Log(value);
    }

    public void GoScene(string SceneName)
    {
        StartCoroutine(_SceneLoading(SceneName));
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
