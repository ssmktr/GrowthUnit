using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public UICamera UiCamera;
    public GameObject UIRoot, SystemRoot;

    IEnumerator Start () {
        if (!Title.GameReady)
        {
            //GameManager.GoScene("Title");
            StartCoroutine(GameManager._SceneLoading("Title"));
            yield break;
        }

        UIManager.Instance.SetManager(UIRoot, SystemRoot);

        UIManager.OpenUI("LobbyPanel");
        UIManager.OpenUI("UpInfoPanel");
    }
}

