using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadyPanel : UIBasePanel {

    public UIScrollView ScrollView;
    public UIGrid Grid;
    public GameObject SLOT;
    ObjectPaging Paging;
    List<int> ListUid = new List<int>();

    public override void Init()
    {
        base.Init();
    }

    public override void LateInit()
    {
        base.LateInit();

        CreateList(0);
    }

    void CreateList(int idx)
    {
        ListUid.Clear();
        foreach (NetData.UnitData unit in GameManager.HaveUnitData.Values)
        {
            ListUid.Add(unit.uid);
        }

        if (ScrollView.transform.localPosition != Vector3.zero)
        {
            ScrollView.transform.localPosition = Vector3.zero;
            ScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (Paging == null)
            Paging = ObjectPaging.CreatePagingPanel(ScrollView.gameObject, Grid.gameObject, SLOT, 1, 10, ListUid.Count, 10, PagingCallBack, ObjectPaging.eScrollType.Horizontal);
        else
            Paging.NowCreate(ListUid.Count);

        ScrollView.enabled = true;
        ScrollView.ResetPosition();
        ScrollView.enabled = ListUid.Count > 7;
    }

    void PagingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (ListUid.Count > idx)
        {
            content.Init(UnitIconSlot.UnitSlotType.MyUnit, ListUid[idx]);
        }
        else
            content.Init(null);
    }
}
