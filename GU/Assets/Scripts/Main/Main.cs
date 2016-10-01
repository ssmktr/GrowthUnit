using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public UICamera UiCamera;
    public GameObject UIRoot, SystemRoot;

    IEnumerator Start () {
        if (!Title.GameReady)
        {
            GameManager.Instance.GoScene("Title");
            yield break;
        }

        int idx = Random.Range(0, DataManager.ListUnitDataBase.Count);
        yield return StartCoroutine(SqliteManager.RequestGetUnit(DataManager.ListUnitDataBase[idx].id));

        UIManager.Instance.SetManager(UIRoot, SystemRoot);
        UIManager.OpenUI("UpInfoPanel");
        UIManager.OpenUI("LobbyPanel");
    }
}

