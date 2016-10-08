using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MiniJSON;

public class InvenPanel : UIBasePanel {
    GameObject ModelObj;

    public GameObject BackBtn;
    public GameObject ModelRoot;
    public UILabel nameLbl;

    public UIScrollView ScrollView;
    public UIGrid Grid;
    public GameObject SLOT;
    ObjectPaging Paging;
    List<int> UnitUidData = new List<int>();

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

        CreateList(0);
        CreateModel(DataManager.ListUnitDataBase[Random.Range(0, DataManager.ListUnitDataBase.Count)]);
    }

    void CreateList(int idx)
    {
        UnitUidData.Clear();
        for (int i = 0; i < GameManager.HaveUnitData.Count; ++i)
        {
            UnitUidData.Add(GameManager.HaveUnitData[i].uid);
        }

        if (ScrollView.transform.localPosition != Vector3.zero)
        {
            ScrollView.transform.localPosition = Vector3.zero;
            ScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (Paging == null)
            Paging = ObjectPaging.CreatePagingPanel(ScrollView.gameObject, Grid.gameObject, SLOT, 4, 16, UnitUidData.Count, 10, PagingCallBack);
        else
            Paging.NowCreate(UnitUidData.Count);

        ScrollView.enabled = true;
        ScrollView.ResetPosition();
        ScrollView.enabled = UnitUidData.Count > 8;
    }

    void PagingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (UnitUidData.Count > idx)
        {
            UnitDataBase.SlotData data = new UnitDataBase.SlotData();
            data.CreateSlotData(DataManager.GetUnitData(UnitUidData[idx]));
            content.Init(data);
        }
        else
        {
            content.Init(null);
        }
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
            AssetBundleLoad.Instance.AssetUnitLoad(DataManager.GetName(_UnitData.stringid), (go) => {
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
