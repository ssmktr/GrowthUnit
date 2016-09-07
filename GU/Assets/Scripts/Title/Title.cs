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
            GameManager.ViewDebug("VersionCheck : " + www.text);
            Login(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
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
                        StartCoroutine(_LoadTableData());
                        StartCoroutine(_LoadConfigData());
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

    #region JSONDATA
    #region UNITDATA
    IEnumerator _LoadTableData()
    {
        WWW www = new WWW("http://ssmktr.ivyro.net/GrowthUnit/AssetBundle/TableDatas/UnitData.json");
        yield return www;

        while (!www.isDone)
            yield return null;

        if (www.error == null)
        {
            GameManager.ViewDebug("UnitData : " + www.text);
            SetUnitDataBase(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetUnitDataBase(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.ListUnitDataBase.Clear();
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

                DataManager.ListUnitDataBase.Add(data);
            }
        }
    }
    #endregion
    #region CONFIGDATA
    IEnumerator _LoadConfigData()
    {
        WWW www = new WWW("http://ssmktr.ivyro.net/GrowthUnit/AssetBundle/TableDatas/ConfigData.json");
        yield return www;

        while (!www.isDone)
            yield return null;

        if (www.error == null)
        {
            GameManager.ViewDebug("ConfigData : " + www.text);
            SetConfigData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetConfigData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicConfig.Clear();
        Dictionary<string, object> DicData = Json.Deserialize(Content) as Dictionary<string, object>;
        if (DicData != null)
        {
            if (DicData.ContainsKey("CreateGold"))
                DataManager.DicConfig.Add("CreateGold", JsonUtil.GetIntValue(DicData, "CreateGold"));

            if (DicData.ContainsKey("CreateRuby"))
                DataManager.DicConfig.Add("CreateRuby", JsonUtil.GetIntValue(DicData, "CreateRuby"));

            if (DicData.ContainsKey("CreateHeart"))
                DataManager.DicConfig.Add("CreateHeart", JsonUtil.GetIntValue(DicData, "CreateHeart"));
        }
    }
    #endregion
    #endregion
}
