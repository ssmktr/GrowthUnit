using UnityEngine;
using System.Collections;

public class UnitIconSlot : MonoBehaviour {

    public enum UnitSlotType
    {
        MyUnit = 0,
        SlotBase,
    };
    UnitSlotType eUnitSlotType = UnitSlotType.MyUnit;

    public GameObject Center, GradeGroup;
    public UISprite Icon, SelectSprite;
    public UILabel NameLbl, LvLbl;

    public int Id = 0;
    public UnitDataBase.SlotData SlotData = null;

    public void Init(params object[] _data)
    {
        SetSelect(false);
        if (_data == null)
        {
            Center.SetActive(false);
        }
        else
        {
            Center.SetActive(true);
            UnitDataBase.SlotData data = new UnitDataBase.SlotData();

            if (_data.Length > 0)
                eUnitSlotType = (UnitSlotType)_data[0];

            if (_data.Length > 1)
            {
                Id = (int)_data[1];
                switch (eUnitSlotType)
                {
                    case UnitSlotType.MyUnit:
                        data.CreateSlotData(GameManager.GetMyUnit(Id));
                        break;

                    case UnitSlotType.SlotBase:
                        data.CreateSlotData(DataManager.GetUnitData(Id));
                        break;
                };
            }

            SlotData = data;
            DataUpdate();
        }
    }

    public void DataUpdate()
    {
        if (SlotData != null)
        {
            UnitDataBase.Data Unit = DataManager.GetUnitData(SlotData.id);
            if (Unit != null)
            {
                string name = DataManager.GetName(Unit.stringid);
                NameLbl.text = name;
                Icon.spriteName = "icon_" + name;
            }

            LvLbl.text = SlotData.lv.ToString();
            SetGrade();
        }
    }

    void SetGrade()
    {
        for (int i = 0; i < 6; ++i)
            GradeGroup.transform.FindChild("star" + i).gameObject.SetActive(SlotData.grade > i);
    }

    public void SetSelect(bool _bSelect)
    {
        if (SelectSprite != null)
            SelectSprite.gameObject.SetActive(_bSelect);
    }
    
}
