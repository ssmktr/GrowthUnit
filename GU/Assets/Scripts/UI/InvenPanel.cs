using UnityEngine;
using System.Collections;

public class InvenPanel : UIBasePanel {
    GameObject ModelObj;

    public GameObject BackBtn;
    public GameObject ModelRoot;

    int SelectUnitId = 0;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("LobbyPanel");
        };
    }

    public override void LateInit()
    {
        base.LateInit();

        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        for (int i = 0; i < DataManager.ListUnitDataBase.Count; ++i)
        {
            CreateModel(DataManager.ListUnitDataBase[i]);
        }
    }

    void CreateModel(UnitDataBase.Data _UnitData)
    {
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        AssetBundleLoad.Instance.AssetUnitLoad(_UnitData.name, (go) => {
            ModelObj = go;
            ModelObj.transform.localPosition = Vector3.zero;
            ModelObj.transform.localScale = new Vector3(-100, 100, 100) * _UnitData.cardsize;

        }, ModelRoot);
    }
}
