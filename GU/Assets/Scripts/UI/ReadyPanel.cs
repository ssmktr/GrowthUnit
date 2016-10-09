using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadyPanel : UIBasePanel {

    public UIScrollView ScrollView;
    public UIGrid Grid;
    public GameObject SLOT;
    ObjectPaging Paging;
    List<int> ListUid = new List<int>();
    List<int> ListSelectUid = new List<int>();

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
            UIEventListener.Get(content.gameObject).onClick = OnClickUnitSlot;

            // 선택한 유닛
            content.SetSelect(ListSelectUid.Contains(content.SlotData.uid));
        }
        else
            content.Init(null);
    }

    void OnClickUnitSlot(GameObject sender)
    {
        UnitIconSlot content = sender.GetComponent<UnitIconSlot>();
        if (sender == null || content == null || content.SlotData == null)
            return;

        // 선택했으면 빼고 선택 안됐으면 넣는다
        if (ListSelectUid.Contains(content.SlotData.uid))
        {
            ListSelectUid.Remove(content.SlotData.uid);
            content.SetSelect(false);
        }
        else 
        {
            if (ListSelectUid.Count < 3)
            {
                ListSelectUid.Add(content.SlotData.uid);
                content.SetSelect(true);
            }
            else
                UIManager.Instance.OpenMessagePopup("더 이상 선택 할 수 없습니다.", delegate() { });
        }
    }
}
