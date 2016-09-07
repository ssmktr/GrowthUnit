using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Title : MonoBehaviour {

    public UICamera UiCamera;

	void Start () {
        ClientVersionCheck();
	}


    void ClientVersionCheck()
    {
        StartCoroutine(_ClientVersionCheck());
    }

    IEnumerator _ClientVersionCheck()
    {
        WWW www = new WWW("http://ssmktr.ivyro.net/GrowthUnit/VersionBase.json");
        yield return www;

        while (!www.isDone)
            yield return null;

        if (www.error == null)
        {
            Debug.Log("VersionCheck : " + www.text);
            Login(www.text);
        }
        else
            Debug.LogError(www.error);
    }

    void Login(string _Content)
    {
        string Content = _Content.Trim();
        Dictionary<string, object> DicData = Json.Deserialize(Content) as Dictionary<string, object>;
        if (DicData != null)
        {
            if (DicData.ContainsKey("clientversion"))
            {
                double ClientVersion = JsonUtil.GetDoubleValue(DicData, "clientversion");
                if (ClientVersion <= GameInfo.ClientVersion)
                {
                    if (DicData.ContainsKey("tableversion"))
                    {
                        // 게임에 필요한 데이터 받기
                        StartCoroutine(_LoadTableDatas());
                    }
                }
                else
                {
                    // 앱 업데이트 하러 가기
                    Application.OpenURL("http://www.google.com/");
                }
            }
        }
    }

    #region UNITDATA
    IEnumerator _LoadTableDatas()
    {
        WWW www = new WWW("http://ssmktr.ivyro.net/GrowthUnit/AssetBundle/TableDatas/UnitDatas.json");
        yield return www;

        while (!www.isDone)
            yield return null;

        if (www.error == null)
        {
            Debug.Log("TableDatas : " + www.text);
            SetUnitDataBase(www.text);
        }
        else
            Debug.LogError(www.error);
    }

    void SetUnitDataBase(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.Instance.ListUnitDataBase.Clear();
        List<object> ListData = Json.Deserialize(Content) as List<object>;
        for (int i = 0; i < ListData.Count; ++i)
        {
            Dictionary<string, object> DicData = ListData[i] as Dictionary<string, object>;
            if (DicData != null)
            {
                UnitDataBase.Data data = new UnitDataBase.Data();

                if (DicData.ContainsKey("id"))
                    data.id = JsonUtil.GetIntValue(DicData, "id");

                if (DicData.ContainsKey("name"))
                    data.name = JsonUtil.GetStringValue(DicData, "name");

                if (DicData.ContainsKey("type"))
                    data.type = JsonUtil.GetIntValue(DicData, "type");

                if (DicData.ContainsKey("move_speed"))
                    data.move_speed = JsonUtil.GetFloatValue(DicData, "move_speed");

                if (DicData.ContainsKey("hp"))
                    data.hp = JsonUtil.GetFloatValue(DicData, "hp");

                if (DicData.ContainsKey("atk"))
                    data.atk = JsonUtil.GetIntValue(DicData, "atk");

                if (DicData.ContainsKey("def"))
                    data.def = JsonUtil.GetIntValue(DicData, "def");

                if (DicData.ContainsKey("cri"))
                    data.cri = JsonUtil.GetFloatValue(DicData, "cri");

                DataManager.Instance.ListUnitDataBase.Add(data);
            }
        }
    }
    #endregion
}
