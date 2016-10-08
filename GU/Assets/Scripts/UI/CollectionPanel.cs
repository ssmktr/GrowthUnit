using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionPanel : UIBasePanel {
    enum TabType
    {
        Human,
        Undead,
        Monster,
    };
    TabType eTabType = TabType.Human;

    public GameObject Center, CardInfo, ModelRoot;
    public GameObject[] TabGroup;

    UnitIconSlot SelectUnit;
    int SelectId = 0;
    GameObject SLOT, ModelObj;

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < TabGroup.Length; ++i)
            UIEventListener.Get(TabGroup[i]).onClick = OnClickTab;

        SLOT = Resources.Load("UI/Slot/UnitIconSlot") as GameObject;
    }

    public override void LateInit()
    {
        base.LateInit();

        SetTab();
    }

    void OnClickTab(GameObject sender)
    {
        TabType TabType = (TabType)System.Enum.Parse(typeof(TabType), sender.name);
        if (TabType != eTabType)
        {
            eTabType = TabType;
            SetTab();
        }
    }

    void SetTab()
    {
        for (int i = 0; i < TabGroup.Length; ++i)
        {
            if ((int)eTabType == i)
            {
                TabGroup[i].transform.FindChild("BackGround").gameObject.SetActive(true);
                SelectUnit = null;
                SelectId = 0;

                CreateList();
            }
            else
            {
                TabGroup[i].transform.FindChild("BackGround").gameObject.SetActive(false);
            }
        }
    }

    ObjectPaging Paging;
    public UIScrollView ScrollView;
    public UIGrid Grid;
    List<int> UnitData = new List<int>();
    void CreateList()
    {
        UnitData.Clear();
        // 인간만 추가
        foreach (UnitDataBase.Data data in DataManager.DicUnitDataBase.Values)
        {
            if (data.type == ((int)eTabType + 1))
                UnitData.Add(data.id);
        }

        if (ScrollView.transform.localPosition != Vector3.zero)
        {
            ScrollView.transform.localPosition = Vector3.zero;
            ScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (Paging == null)
            Paging = ObjectPaging.CreatePagingPanel(ScrollView.gameObject, Grid.gameObject, SLOT, 4, 12, UnitData.Count, 10, HumanPaingCallBack);
        else
            Paging.NowCreate(UnitData.Count);

        ScrollView.enabled = true;
        ScrollView.ResetPosition();
        ScrollView.enabled = UnitData.Count > 8;
    }

    void HumanPaingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (UnitData.Count > idx)
        {
            content.Init(UnitIconSlot.UnitSlotType.SlotBase, UnitData[idx]);
            UIEventListener.Get(obj).onClick = OnClickSlot;
                
            if (SelectId == 0)
            {
                SelectId = content.SlotData.id;
                SelectUnit = content;
                CreateModel();
            }
            content.SetSelect(SelectId == content.SlotData.id);
        }
        else
            content.Init(null);
    }
    
    void OnClickSlot(GameObject sender)
    {
        if (sender == null)
            return;

        UnitIconSlot content = sender.GetComponent<UnitIconSlot>();
        if (content != null)
        {
            if (SelectUnit != null)
                SelectUnit.SetSelect(false);

            SelectId = content.SlotData.id;
            SelectUnit = content;
            SelectUnit.SetSelect(true);

            CreateModel();
        }
    }

    void CreateModel()
    {
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        if (ModelObj == null)
        {
            UnitDataBase.Data UnitData = DataManager.GetUnitData(SelectId);
            if (UnitData != null)
            {
                string UnitName = DataManager.GetUnitResourceData(UnitData.id).assetbundlename;
                if (!string.IsNullOrEmpty(UnitName))
                {
                    AssetBundleLoad.Instance.AssetUnitLoad(UnitName, (obj) => {
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localScale = Vector3.one * 100 * UnitData.cardsize;
                        UIManager.SetLayer(obj.transform, 8);

                        Renderer[] render = obj.GetComponentsInChildren<Renderer>();
                        for (int i = 0; i < render.Length; ++i)
                            render[i].sortingOrder += 1;

                        ModelObj = obj;

                    }, ModelRoot);
                }
            }
        }
    }
}
