using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Title : MonoBehaviour {

    public static bool GameReady = false;

    public UICamera UiCamera;
    public UIProgressBar UiProgressBar;

	void Start () {
        GameReady = false;
        UiProgressBar.gameObject.SetActive(false);
        UiProgressBar.value = 0;
        ClientVersionCheck();
	}

    void ClientVersionCheck()
    {
        StartCoroutine(_ClientVersionCheck());
    }

    IEnumerator _ClientVersionCheck()
    {
        WWW www = new WWW(GameInfo.NetworkUrl + "VersionBase.json");
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
                    if (DicData.ContainsKey("unitversion"))
                        GameInfo.UnitVersion = JsonUtil.GetIntValue(DicData, "unitversion");

                    if (DicData.ContainsKey("tableversion"))
                        GameInfo.TableVersion = JsonUtil.GetIntValue(DicData, "tableversion");

                    if (DicData.ContainsKey("textureversion"))
                        GameInfo.TextureVersion = JsonUtil.GetIntValue(DicData, "textureversion");

                    if (DicData.ContainsKey("soundversion"))
                        GameInfo.SoundVersion = JsonUtil.GetIntValue(DicData, "soundversion");

                    if (DicData.ContainsKey("effectversion"))
                        GameInfo.EffectVersion = JsonUtil.GetIntValue(DicData, "effectversion");

                    if (DicData.ContainsKey("tableversion"))
                    {
                        // 게임에 필요한 데이터 받기
                        StartCoroutine(_Login());
                    }
                }
                else
                {
                    // 앱 업데이트 하러 가기
                    if (DicData.ContainsKey("updateurl"))
                    {
                        string updateurl = JsonUtil.GetStringValue(DicData, "updateurl");
                        Debug.Log(updateurl);
                        Application.OpenURL(updateurl);
                    }
                }
            }
        }
    }

    // 순차적으로 데이터 불러옴 (에셋번들 불러온 다음 Json데이터 불러옴)
    IEnumerator _Login()
    {
        // 에셋번들
        yield return StartCoroutine(_LoadTextureData());
        yield return StartCoroutine(_LoadUnitData());

        // Json 데이터
        yield return StartCoroutine(_LoadTableData());
        yield return StartCoroutine(_LoadConfigData());
    }

    #region DATA
    #region
    IEnumerator _LoadTextureData()
    {
        AssetBundleLoad.TextureLoadReady = false;
        WWW www = WWW.LoadFromCacheOrDownload(GameInfo.AssetBundleUrl + "TextureData/TextureData.unity3d", GameInfo.TextureVersion);
        while (!www.isDone)
        {
            UiProgressBar.gameObject.SetActive(true);
            UiProgressBar.value = www.progress;
            UiProgressBar.transform.FindChild("PercentLbl").GetComponent<UILabel>().text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            UiProgressBar.gameObject.SetActive(false);
            www.assetBundle.Unload(false);
            AssetBundleLoad.TextureLoadReady = true;
        }
        else
            GameManager.ViewDebug(www.error);
    }
    #endregion
    #region UNITDATA
    IEnumerator _LoadUnitData()
    {
        AssetBundleLoad.UnitLoadReady = false;
        WWW www = WWW.LoadFromCacheOrDownload(GameInfo.AssetBundleUrl + "UnitData/UnitData.unity3d", GameInfo.UnitVersion);
        while (!www.isDone)
        {
            UiProgressBar.gameObject.SetActive(true);
            UiProgressBar.value = www.progress;
            UiProgressBar.transform.FindChild("PercentLbl").GetComponent<UILabel>().text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            UiProgressBar.gameObject.SetActive(false);
            www.assetBundle.Unload(false);
            AssetBundleLoad.UnitLoadReady = true;
        }
        else
            GameManager.ViewDebug(www.error);
    }
    #endregion
    #region UNITDATA
    IEnumerator _LoadTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/UnitData.json");
        while (!www.isDone)
        {
            UiProgressBar.gameObject.SetActive(true);
            UiProgressBar.value = www.progress;
            UiProgressBar.transform.FindChild("PercentLbl").GetComponent<UILabel>().text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            UiProgressBar.gameObject.SetActive(false);
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

                if (DicData.ContainsKey("cardsize"))
                    data.cardsize = JsonUtil.GetFloatValue(DicData, "cardsize");

                DataManager.ListUnitDataBase.Add(data);
            }
        }
    }
    #endregion
    #region CONFIGDATA
    IEnumerator _LoadConfigData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/ConfigData.json");
        while (!www.isDone)
        {
            UiProgressBar.gameObject.SetActive(true);
            UiProgressBar.value = www.progress;
            UiProgressBar.transform.FindChild("PercentLbl").GetComponent<UILabel>().text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            UiProgressBar.gameObject.SetActive(false);
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

        GameReady = true;
        StartCoroutine(GameManager._SceneLoading("Main"));
    }
    #endregion
    #endregion

    
}
