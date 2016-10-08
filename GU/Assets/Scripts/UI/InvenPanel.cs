using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvenPanel : UIBasePanel {
    GameObject ModelObj;

    public GameObject ModelRoot;
    public GameObject[] SortTab;
    public UILabel nameLbl;

    public UIScrollView ScrollView;
    public UIGrid Grid;
    public GameObject SLOT;
    ObjectPaging Paging;
    List<int> UnitUidData = new List<int>();
    UnitSortType eSortTab = UnitSortType.Get;
    UnitIconSlot SelectSlot = null;
    int SelectUid = 0;

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < SortTab.Length; ++i)
            UIEventListener.Get(SortTab[i]).onClick = OnClickSortTab;
    }

    void OnClickSortTab(GameObject sender)
    {
        UnitSortType _eSortTab = (UnitSortType)System.Enum.Parse(typeof(UnitSortType), sender.name);
        if (eSortTab != _eSortTab)
        {
            eSortTab = _eSortTab;
            ViewSortTab();
        }
    }

    void ViewSortTab()
    {
        for (int i = 0; i < SortTab.Length; ++i)
        {
            if ((int)eSortTab == i)
            {
                SortTab[i].transform.FindChild("Back").gameObject.SetActive(true);
                CreateList(i);
            }
            else
            {
                SortTab[i].transform.FindChild("Back").gameObject.SetActive(false);
            }
        }
    }

    public override void LateInit()
    {
        base.LateInit();

        SelectUid = 0;
        nameLbl.text = "";
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        // 소팅탭
        ViewSortTab();

        CreateList(0);
        CreateModel();
    }

    void CreateList(int idx)
    {
        UnitUidData.Clear();
        foreach (NetData.UnitData unit in GameManager.HaveUnitData.Values)
        {
            UnitUidData.Add(unit.uid);
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
            content.Init(UnitIconSlot.UnitSlotType.MyUnit, UnitUidData[idx]);
            UIEventListener.Get(content.gameObject).onClick = OnClickUnitSlot;

            if (SelectUid == 0)
            {
                SelectUid = UnitUidData[idx];
                SelectSlot = content;
            }

            content.SetSelect(SelectUid == UnitUidData[idx]);
        }
        else
        {
            content.Init(null);
        }
    }

    void OnClickUnitSlot(GameObject sender)
    {
        UnitIconSlot content = sender.GetComponent<UnitIconSlot>();
        if (sender == null || content == null || content.SlotData == null)
            return;

        if (SelectSlot != null)
            SelectSlot.SetSelect(false);

        SelectSlot = content;
        SelectUid = content.Id;
        SelectSlot.SetSelect(true);

        CreateModel();
    }

    void CreateModel()
    {
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
            nameLbl.text = "";
        }

        if (ModelObj == null)
        {
            NetData.UnitData UnitData = GameManager.GetMyUnit(SelectUid);
            if (UnitData != null)
            {
                UnitDataBase.Data Unit = DataManager.GetUnitData(UnitData.id);

                AssetBundleLoad.Instance.AssetUnitLoad(DataManager.GetUnitResourceData(UnitData.id).assetbundlename, (go) => {
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = new Vector3(-100, 100, 100) * Unit.cardsize;
                    UIManager.SetLayer(go.transform, 8);

                    Renderer[] render = go.GetComponentsInChildren<Renderer>();
                    for (int i = 0; i < render.Length; ++i)
                        render[i].sortingOrder += 1;

                    ModelObj = go;

                }, ModelRoot);

                nameLbl.text = DataManager.GetName(Unit.stringid);
            }
        }
    }
}
