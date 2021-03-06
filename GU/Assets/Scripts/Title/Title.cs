﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Title : MonoBehaviour {

    public static bool GameReady = false;
    public UILabel PercentLbl;

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

                    if (DicData.ContainsKey("textureversion"))
                        GameInfo.TextureVersion = JsonUtil.GetIntValue(DicData, "textureversion");

                    if (DicData.ContainsKey("soundversion"))
                        GameInfo.SoundVersion = JsonUtil.GetIntValue(DicData, "soundversion");

                    if (DicData.ContainsKey("effectversion"))
                        GameInfo.EffectVersion = JsonUtil.GetIntValue(DicData, "effectversion");

                    // 게임에 필요한 데이터 받기
                    StartCoroutine(_Login());
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
        UiProgressBar.gameObject.SetActive(true);

        // 에셋번들
        yield return StartCoroutine(_LoadTextureData());
        yield return StartCoroutine(_LoadUnitData());

        // Json 데이터
        yield return StartCoroutine(_LoadUnitTableData());
        yield return StartCoroutine(_LoadStageTableData());
        yield return StartCoroutine(_LoadNameTableData());
        yield return StartCoroutine(_LoadResourceTableData());
        yield return StartCoroutine(_LoadConfigTableData());

        // Sqlite 데이터
        yield return StartCoroutine(SqliteManager.Instance.RequestLoadUserData());
        yield return StartCoroutine(SqliteManager.Instance.RequestLoadMyUnit());
        if (GameManager.HaveUnitData.Count <= 0)
        {
            // 유닛이 없으면 101, 201, 301 중에 랜덤하게 1마리 생성
            int idx = Random.Range(1, 4) * 100 + 1;
            yield return StartCoroutine(SqliteManager.Instance.RequestGetUnit(idx));
            yield return StartCoroutine(SqliteManager.Instance.RequestLoadMyUnit());
        }

        // 게임 준비 끝 메인 씬으로 이동
        UiProgressBar.gameObject.SetActive(false);

        GameReady = true;
        GameManager.Instance.GoScene("Main");
    }

    #region DATA
    #region
    IEnumerator _LoadTextureData()
    {
        AssetBundleLoad.TextureLoadReady = false;
        WWW www = WWW.LoadFromCacheOrDownload(GameInfo.AssetBundleUrl + "TextureData/TextureData.unity3d", GameInfo.TextureVersion);
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
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
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            www.assetBundle.Unload(false);
            AssetBundleLoad.UnitLoadReady = true;
        }
        else
            GameManager.ViewDebug(www.error);
    }
    #endregion
    #region UNITJSONDATA
    IEnumerator _LoadUnitTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/UnitData.json");
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            GameManager.ViewDebug("UnitData : " + www.text);
            SetUnitTableData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetUnitTableData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicUnitDataBase.Clear();
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

                if (DicData.ContainsKey("stringid"))
                    data.stringid = JsonUtil.GetIntValue(DicData, "stringid");

                if (DicData.ContainsKey("type"))
                    data.type = JsonUtil.GetIntValue(DicData, "type");

                if (DicData.ContainsKey("classtype"))
                    data.classtype = JsonUtil.GetIntValue(DicData, "classtype");

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

                if (DicData.ContainsKey("attackrange"))
                    data.attackrange = JsonUtil.GetFloatValue(DicData, "attackrange");

                if (DicData.ContainsKey("cardsize"))
                    data.cardsize = JsonUtil.GetFloatValue(DicData, "cardsize");

                if (DicData.ContainsKey("battlesize"))
                    data.battlesize = JsonUtil.GetFloatValue(DicData, "battlesize");

                DataManager.DicUnitDataBase.Add(data.id, data);
            }
        }
    }
    #endregion
    #region STAGEDATA
    IEnumerator _LoadStageTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/StageData.json");
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            GameManager.ViewDebug("StageData : " + www.text);
            SetStageTableData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetStageTableData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicStageDataBase.Clear();
        List<object> ListData = Json.Deserialize(Content) as List<object>;
        if (ListData != null)
        {
            for (int i = 0; i < ListData.Count; ++i)
            {
                Dictionary<string, object> DicData = ListData[i] as Dictionary<string, object>;
                if (DicData != null)
                {
                    StageDataBase.Data data = new StageDataBase.Data();
                    if (DicData.ContainsKey("id"))
                        data.id = JsonUtil.GetIntValue(DicData, "id");

                    if (DicData.ContainsKey("accountexp"))
                        data.accountexp = JsonUtil.GetIntValue(DicData, "accountexp");

                    if (DicData.ContainsKey("unitexp"))
                        data.unitexp = JsonUtil.GetIntValue(DicData, "unitexp");

                    if (DicData.ContainsKey("rewardgold"))
                        data.rewardgold = JsonUtil.GetIntValue(DicData, "rewardgold");

                    if (DicData.ContainsKey("rewardgold"))
                        data.rewardgold = JsonUtil.GetIntValue(DicData, "rewardgold");

                    if (DicData.ContainsKey("enemylist"))
                    {
                        data.ListEnemyId.Clear();
                        List<object> ListEnemy = Json.Deserialize(JsonUtil.GetStringValue(DicData, "enemylist")) as List<object>;
                        if (ListEnemy != null)
                        {
                            for (int iEnemy = 0; iEnemy < ListEnemy.Count; ++iEnemy)
                                data.ListEnemyId.Add(int.Parse(ListEnemy[iEnemy].ToString()));
                        }
                    }

                    if (DicData.ContainsKey("enemycount"))
                        data.enemycount = JsonUtil.GetIntValue(DicData, "enemycount");

                    if (DicData.ContainsKey("bosslist"))
                    {
                        data.ListBossId.Clear();
                        List<object> ListBoss = Json.Deserialize(JsonUtil.GetStringValue(DicData, "bosslist")) as List<object>;
                        if (ListBoss != null)
                        {
                            for (int iBoss = 0; iBoss < ListBoss.Count; ++iBoss)
                                data.ListBossId.Add(int.Parse(ListBoss[iBoss].ToString()));
                        }
                    }

                    if (DicData.ContainsKey("bosscount"))
                        data.bosscount = JsonUtil.GetIntValue(DicData, "bosscount");

                    DataManager.DicStageDataBase.Add(data.id, data);
                }
            }
        }
    }
    #endregion
    #region NAMEDATA
    IEnumerator _LoadNameTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/NameData.json");
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            GameManager.ViewDebug("NameData : " + www.text);
            SetNameTableData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetNameTableData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicNameDataBase.Clear();
        List<object> ListData = Json.Deserialize(Content) as List<object>;
        for (int i = 0; i < ListData.Count; ++i)
        {
            Dictionary<string, object> DicData = ListData[i] as Dictionary<string, object>;
            if (DicData != null)
            {
                NameDataBase.Data data = new NameDataBase.Data();

                if (DicData.ContainsKey("id"))
                    data.id = JsonUtil.GetIntValue(DicData, "id");

                if (DicData.ContainsKey("kor"))
                    data.kor = JsonUtil.GetStringValue(DicData, "kor");

                DataManager.DicNameDataBase.Add(data.id, data);
            }
        }
    }
    #endregion
    #region RESOURCEDATA
    IEnumerator _LoadResourceTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/ResourceData.json");
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            GameManager.ViewDebug("ResourceData : " + www.text);
            SetResourceTableData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetResourceTableData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicUnitResourceData.Clear();
        List<object> ListData = Json.Deserialize(Content) as List<object>;
        for (int i = 0; i < ListData.Count; ++i)
        {
            Dictionary<string, object> DicData = ListData[i] as Dictionary<string, object>;
            if (DicData != null)
            {
                UnitDataBase.ResourceData data = new UnitDataBase.ResourceData();

                if (DicData.ContainsKey("id"))
                    data.id = JsonUtil.GetIntValue(DicData, "id");

                if (DicData.ContainsKey("type"))
                    data.type = JsonUtil.GetIntValue(DicData, "type");

                if (DicData.ContainsKey("assetbundlename"))
                    data.assetbundlename = JsonUtil.GetStringValue(DicData, "assetbundlename");

                switch (data.type)
                {
                    case 1:
                        DataManager.DicUnitResourceData.Add(data.id, data);
                        break;
                };
                
            }
        }
    }
    #endregion
    #region CONFIGSJONDATA
    IEnumerator _LoadConfigTableData()
    {
        WWW www = new WWW(GameInfo.AssetBundleUrl + "TableData/ConfigData.json");
        while (!www.isDone)
        {
            UiProgressBar.value = www.progress;
            PercentLbl.text = string.Format("{0}%", (int)(UiProgressBar.value * 100));
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            GameManager.ViewDebug("ConfigData : " + www.text);
            SetConfigTableData(www.text);
        }
        else
            GameManager.ViewDebug(www.error);
    }

    void SetConfigTableData(string _Content)
    {
        string Content = _Content.Trim();
        DataManager.DicConfig.Clear();
        Dictionary<string, object> DicData = Json.Deserialize(Content) as Dictionary<string, object>;
        if (DicData != null)
        {
            if (DicData.ContainsKey("CreateGold"))
                DataManager.DicConfig.Add("CreateGold", JsonUtil.GetIntValue(DicData, "CreateGold"));

            if (DicData.ContainsKey("CreateDia"))
                DataManager.DicConfig.Add("CreateDia", JsonUtil.GetIntValue(DicData, "CreateDia"));

            if (DicData.ContainsKey("CreateHeart"))
                DataManager.DicConfig.Add("CreateHeart", JsonUtil.GetIntValue(DicData, "CreateHeart"));
        }
    }
    #endregion
    #endregion
}
