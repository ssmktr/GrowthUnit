using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public UICamera UiCamera;
    public GameObject UIRoot, SystemRoot;

	void Start () {
        if (!Title.GameReady)
        {
            GameManager.GoScene("Title");
            return;
        }
        UIManager.Instance.SetManager(UIRoot, SystemRoot);

        UIManager.OpenUI("LobbyPanel");
        UIManager.OpenUI("UpInfoPanel");
    }
}
