using UnityEngine;
using System.Collections;

using MiniJSON;

public class InvenPanel : UIBasePanel {
    GameObject ModelObj;

    public GameObject BackBtn;
    public GameObject ModelRoot;
    public UILabel nameLbl;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            UIManager.Instance.Prev();
        };
    }

    public override void LateInit()
    {
        base.LateInit();

        nameLbl.text = "";
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        CreateModel(DataManager.ListUnitDataBase[Random.Range(0, DataManager.ListUnitDataBase.Count)]);
    }

    void CreateModel(UnitDataBase.Data _UnitData)
    {
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
            nameLbl.text = "";
        }

        if (ModelObj == null)
        {
            AssetBundleLoad.Instance.AssetUnitLoad(_UnitData.name, (go) => {
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(-100, 100, 100) * _UnitData.cardsize;
                UIManager.SetLayer(go.transform, 8);

                Renderer[] render = go.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < render.Length; ++i)
                    render[i].sortingOrder += 1;

                ModelObj = go;

            }, ModelRoot);

            nameLbl.text = _UnitData.name;
        }
    }
}
