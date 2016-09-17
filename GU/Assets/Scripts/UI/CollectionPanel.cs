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

    public GameObject Center, BackBtn;
    public GameObject[] TabGroup, BaseGroup;

    GameObject SLOT;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            UIManager.Instance.Prev();
        };

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
        eTabType = (TabType)System.Enum.Parse(typeof(TabType), sender.name);
        SetTab();
    }

    void SetTab()
    {
        for (int i = 0; i < TabGroup.Length; ++i)
        {
            if ((int)eTabType == i)
            {
                TabGroup[i].transform.FindChild("BackGround").gameObject.SetActive(true);
                BaseGroup[i].SetActive(true);
                if (eTabType == TabType.Human)
                    CreateHumanList();
                else if (eTabType == TabType.Undead)
                    CreateUndeadList();
                else if (eTabType == TabType.Monster)
                    CreateMonsterList();
            }
            else
            {
                TabGroup[i].transform.FindChild("BackGround").gameObject.SetActive(false);
                BaseGroup[i].SetActive(false);
            }
        }
    }

    #region HUMAN
    ObjectPaging HumanPaging;
    public UIScrollView HumanScrollView;
    public UIGrid HumanGrid;
    List<UnitDataBase.SlotData> UnitData = new List<UnitDataBase.SlotData>();
    void CreateHumanList()
    {
        UnitData.Clear();
        // 인간만 추가
        for (int i = 0; i < DataManager.ListUnitDataBase.Count; ++i)
        {
            if (DataManager.ListUnitDataBase[i].type == 1)
            {
                UnitDataBase.SlotData data = new UnitDataBase.SlotData();
                data.CreateSlotData(DataManager.ListUnitDataBase[i]);
                UnitData.Add(data);
            }
        }

        if (HumanScrollView.transform.localPosition != Vector3.zero)
        {
            HumanScrollView.transform.localPosition = Vector3.zero;
            HumanScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (HumanPaging == null)
            HumanPaging = ObjectPaging.CreatePagingPanel(HumanScrollView.gameObject, HumanGrid.gameObject, SLOT, 4, 12, UnitData.Count, 10, HumanPaingCallBack, ObjectPaging.eScrollType.Vertical);
        else
            HumanPaging.NowCreate(UnitData.Count);

        HumanScrollView.enabled = true;
        HumanScrollView.ResetPosition();
        HumanScrollView.enabled = UnitData.Count > 8;
    }

    void HumanPaingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (idx < UnitData.Count)
        {
            content.Init(UnitData[idx]);
        }
        else
            content.Init(null);
    }
    #endregion

    #region UNDEAD
    ObjectPaging UndeadPaging;
    public UIScrollView UndeadScrollView;
    public UIGrid UndeadGrid;
    List<UnitDataBase.SlotData> UndeadData = new List<UnitDataBase.SlotData>();
    void CreateUndeadList()
    {
        UndeadData.Clear();
        // 인간만 추가
        for (int i = 0; i < DataManager.ListUnitDataBase.Count; ++i)
        {
            if (DataManager.ListUnitDataBase[i].type == 2)
            {
                UnitDataBase.SlotData data = new UnitDataBase.SlotData();
                data.CreateSlotData(DataManager.ListUnitDataBase[i]);
                UndeadData.Add(data);
            }
        }

        if (UndeadScrollView.transform.localPosition != Vector3.zero)
        {
            UndeadScrollView.transform.localPosition = Vector3.zero;
            UndeadScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (UndeadPaging == null)
            UndeadPaging = ObjectPaging.CreatePagingPanel(UndeadScrollView.gameObject, UndeadGrid.gameObject, SLOT, 4, 12, UndeadData.Count, 10, UndeadPaingCallBack, ObjectPaging.eScrollType.Vertical);
        else
            UndeadPaging.NowCreate(UndeadData.Count);

        UndeadScrollView.enabled = true;
        UndeadScrollView.ResetPosition();
        UndeadScrollView.enabled = UndeadData.Count > 8;
    }

    void UndeadPaingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (idx < UndeadData.Count)
        {
            content.Init(UndeadData[idx]);
        }
        else
            content.Init(null);
    }
    #endregion

    #region Monster
    ObjectPaging MonsterPaging;
    public UIScrollView MonsterScrollView;
    public UIGrid MonsterGrid;
    List<UnitDataBase.SlotData> MonsterData = new List<UnitDataBase.SlotData>();
    void CreateMonsterList()
    {
        MonsterData.Clear();
        // 인간만 추가
        for (int i = 0; i < DataManager.ListUnitDataBase.Count; ++i)
        {
            if (DataManager.ListUnitDataBase[i].type == 3)
            {
                UnitDataBase.SlotData data = new UnitDataBase.SlotData();
                data.CreateSlotData(DataManager.ListUnitDataBase[i]);
                MonsterData.Add(data);
            }
        }

        if (MonsterScrollView.transform.localPosition != Vector3.zero)
        {
            MonsterScrollView.transform.localPosition = Vector3.zero;
            MonsterScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
        }

        if (MonsterPaging == null)
            MonsterPaging = ObjectPaging.CreatePagingPanel(MonsterScrollView.gameObject, MonsterGrid.gameObject, SLOT, 4, 12, MonsterData.Count, 10, MonsterPaingCallBack, ObjectPaging.eScrollType.Vertical);
        else
            MonsterPaging.NowCreate(MonsterData.Count);

        MonsterScrollView.enabled = true;
        MonsterScrollView.ResetPosition();
        MonsterScrollView.enabled = MonsterData.Count > 8;
    }

    void MonsterPaingCallBack(int idx, GameObject obj)
    {
        UnitIconSlot content = obj.GetComponent<UnitIconSlot>();
        if (idx < MonsterData.Count)
        {
            content.Init(MonsterData[idx]);
        }
        else
            content.Init(null);
    }
    #endregion
}
